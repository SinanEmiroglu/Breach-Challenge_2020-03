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
        private LevelData _currentLevelData;
        private UIMainMenu _mainMenu;
        private PlayerMovementController _playerController;
        private LevelData[] _levels;

        /// <summary>
        /// Player starts a new level or continue saved one.
        /// </summary>
        public void BeginLevel(GameState state) => StartCoroutine(NewGameCor(state));

        /// <summary>
        /// Player starts a next level after they complete current one successfully.
        /// </summary>
        public void BeginNextLevel() => StartCoroutine(NextGameCor());

        /// <summary>
        /// Global save invoker.
        /// </summary>
        public void Save()
        {
            _currentLevelData.TimeLeftInSec = _remainingTime;
            SaveLoad.Save(_currentLevelData, SAVE_FILE_KEY);
            OnSave?.Invoke();
        }

        /// <summary>
        /// Global load invoker.
        /// </summary>
        public void Load() => OnLoad?.Invoke();

        /// <summary>
        /// Called by dudes after they are hit with top. Also, checking game is over or not.
        /// </summary>
        public void BouncePlayer()
        {
            OnScoreUpdated?.Invoke(++_currentLevelData.CurrentScore, _currentLevelData.ScoreToWin);
            _playerController.Bounce();

            if (_currentLevelData.CurrentScore >= _currentLevelData.ScoreToWin)
            {
                GameOver(true);
            }
        }

        /// <summary>
        /// Called by Menu button in game to freeze the play time.
        /// </summary>
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

        /// <summary>
        /// Called by UIGameplay -> Return Menu button.
        /// </summary>
        public void ReturnMenu()
        {
            PausePlayGame();
            _mainMenu.SetActiveMainMenu(true);
            SceneManager.UnloadSceneAsync(1);
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

        /// <summary>
        /// Hard-coded level data.
        /// </summary>
        private void InitializeLevelData()
        {
            _levels = new[] {new LevelData(levelName: "First Challenge", scoreToWin: 4, currentScore: 0, timeLeftInSec: 80),
                             new LevelData(levelName: "Second Challenge", scoreToWin: 8, currentScore: 0, timeLeftInSec: 120)};
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

        /// <summary>
        /// Game over handler to invoke OnGameOver event.
        /// </summary>
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

        /// <summary>
        /// Callbacks are "New Game" or "Continue" in the main menu.
        /// </summary>
        private IEnumerator NewGameCor(GameState state)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

            yield return new WaitUntil(() => operation.isDone);

            _mainMenu.SetActiveMainMenu(false);
            _playerController = FindObjectOfType<PlayerMovementController>();
            _isGameStarted = true;

            if (state == GameState.Fresh)
            {
                InitializeLevelData();
                _currentLevelData = GetLevelDataByIndex(_levelIndex);
            }
            else if (state == GameState.Saved)
            {
                _currentLevelData = GetSavedLevelData();
                Load();
            }

            _remainingTime = _currentLevelData.TimeLeftInSec;

            OnLevelLoaded?.Invoke(_currentLevelData);
            OnScoreUpdated?.Invoke(_currentLevelData.CurrentScore, _currentLevelData.ScoreToWin);
            StopCoroutine("NewGameCor");
        }

        /// <summary>
        /// Callback is "Next Level" in the result screen.
        /// </summary>
        private IEnumerator NextGameCor()
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive);

            yield return new WaitUntil(() => operation.isDone);

            _playerController = FindObjectOfType<PlayerMovementController>();
            _currentLevelData = GetLevelDataByIndex(++_levelIndex);
            _remainingTime = _currentLevelData.TimeLeftInSec;

            OnLevelLoaded?.Invoke(_currentLevelData);
            OnScoreUpdated?.Invoke(_currentLevelData.CurrentScore, _currentLevelData.ScoreToWin);

            if (_currentLevelData != null)
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
            StopCoroutine("NextGameCor");
        }
    }
}