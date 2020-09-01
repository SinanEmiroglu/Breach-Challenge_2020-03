// Copyright (c) Breach AS. All rights reserved.

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Breach
{
    public enum GameState { Fresh, Saved }

    public class GameManager : Singleton<GameManager>
    {
        public const string SAVE_FILE_KEY = "Level";

        public static event System.Action OnSave = delegate { };
        public static event System.Action OnLoad = delegate { };
        public static event System.Action<bool> OnGameOver = delegate { };
        public static event System.Action<int, int> OnScoreUpdated = delegate { };
        public static event System.Action<float> OnRemainingTimeUpdated = delegate { };
        public static event System.Action<LevelData> OnLevelLoaded = delegate { };

        private int _levelIndex;
        private float _lastTick;
        private float _remainingTime;
        private float _tickLength = 1f;
        private bool _isTimeOver = false;
        private bool _isGamePaused = false;
        private bool _isGameStarted = false;
        private PlayerMovementController _playerController;
        private UIMainMenu _mainMenu;
        private LevelData[] _levels;

        public LevelData CurrentLevelData { get; private set; }
        public void BeginLevel(GameState state) => StartCoroutine(BeginGame(state));
        public void BeginNextLevel() => StartCoroutine(BeginNextGame());

        public void Save()
        {
            CurrentLevelData.RemainingTimeOfLevel = _remainingTime;
            OnSave?.Invoke();
        }

        public void Load() => OnLoad?.Invoke();

        internal void BouncePlayer()
        {
            OnScoreUpdated?.Invoke(++CurrentLevelData.CurrentScore, CurrentLevelData.ScoreToWin);
            _playerController.Jump();

            if (CurrentLevelData.CurrentScore >= CurrentLevelData.ScoreToWin)
            {
                GameOver(true);
            }
        }

        public void ReplayLastLevel()
        {

        }

        public void PausePlayGame()
        {
            if (_isGamePaused)
            {
                Time.timeScale = 1;
            }
            else
            {
                Time.timeScale = 0;
            }
            _isGamePaused = !_isGamePaused;
        }

        protected override void Awake()
        {
            base.Awake();
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            InitializeLevelData();
            _mainMenu = FindObjectOfType<UIMainMenu>();
            _levelIndex = 0;
        }

        private void InitializeLevelData()
        {
            _levels = new[] {new LevelData(scoreToWin:5,currentScore:0,dudeNumberToSpawn:10,remainingTimeOfLevel:120),
                              new LevelData(scoreToWin:8,currentScore:2,dudeNumberToSpawn:20,remainingTimeOfLevel:120)};
        }

        private void Update()
        {
            if (!_isGameStarted || _isTimeOver)
                return;

            if (Time.time - _lastTick > _tickLength)
            {
                _remainingTime--;
                _lastTick = Time.time;
            }

            if (_remainingTime <= 0)
            {
                _isTimeOver = true;

                GameOver(false);
                OnRemainingTimeUpdated?.Invoke(0);
            }
            else
            {
                OnRemainingTimeUpdated?.Invoke(_remainingTime);
            }
        }

        private void GameOver(bool isWon)
        {
            _isGameStarted = false;
            _isTimeOver = false;
            SceneManager.LoadSceneAsync(2, LoadSceneMode.Additive).completed += (opr) =>
            {
                OnGameOver?.Invoke(isWon);
                SceneManager.UnloadSceneAsync(1);
            };
        }

        private LevelData GetLevelDataByIndex(int index)
        {
            if (index >= _levels.Length)
                return null;

            return _levels[index];
        }

        private LevelData GetSavedLevelData()
        {
            return SaveLoad.Load<LevelData>(SAVE_FILE_KEY);
        }

        private IEnumerator BeginGame(GameState state)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

            yield return new WaitUntil(() => operation.isDone);

            _mainMenu.SetActiveMainMenu(false);
            _playerController = FindObjectOfType<PlayerMovementController>();
            _isGameStarted = true;

            if (state == GameState.Fresh)
            {
                InitializeLevelData();
                CurrentLevelData = GetLevelDataByIndex(_levelIndex);
            }
            else if (state == GameState.Saved)
            {
                CurrentLevelData = GetSavedLevelData();
            }

            _remainingTime = CurrentLevelData.RemainingTimeOfLevel;

            OnLevelLoaded?.Invoke(CurrentLevelData);
            OnScoreUpdated?.Invoke(CurrentLevelData.CurrentScore, CurrentLevelData.ScoreToWin);
        }

        private IEnumerator BeginNextGame()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

            yield return new WaitUntil(() => operation.isDone);

            CurrentLevelData = GetLevelDataByIndex(++_levelIndex);

            if (CurrentLevelData != null)
            {
                _isGameStarted = true;
                SceneManager.UnloadSceneAsync(2);
            }
            else
            {
                SceneManager.UnloadSceneAsync(1);
                SceneManager.UnloadSceneAsync(2);
                _mainMenu.SetActiveMainMenu(true);
            }
        }

        //private void OnGUI()
        //{
        //    if (GUI.Button(new Rect(10, 10, 150, 50), "Save Level"))
        //    {
        //        Save();
        //        //print("Save the current level.");
        //    }

        //    if (GUI.Button(new Rect(170, 10, 150, 50), "Load Level"))
        //        Load();
        //    //print("Replaces current level with one loaded from a previous save");

        //    if (GUI.Button(new Rect(10, 70, 150, 50), "Save GameState"))
        //        print("Save transforms and velocities of all dynamic game object and states");

        //    if (GUI.Button(new Rect(170, 70, 150, 50), "Load GameState"))
        //        print("Loads previously saved dynamic game objects, restoring their transforms and velocities");

        //    GUI.TextField(new Rect(10, 130, 150, 20), "Remaining Time: " + _currentLevel.RemainingTimeOfLevel.ToString());

        //}
    }
}