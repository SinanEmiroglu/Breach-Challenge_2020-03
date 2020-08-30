// Copyright (c) Breach AS. All rights reserved.
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Breach
{
    public class GameManager : Singleton<GameManager>
    {
        public static event System.Action OnSave = delegate { };
        public static event System.Action OnLoad = delegate { };
        public static event System.Action OnWin = delegate { };
        public static event System.Action OnLose = delegate { };
        public static event System.Action<int> OnScoreUpdated = delegate { };
        public static event System.Action<float> OnRemainingTimeUpdated = delegate { };

        private float _tickLength = 1f;
        private float _lastTick;
        private bool _isGamePaused = false;
        private PlayerMovementController _player;
        private LevelData _currentLevelData;

        public void Save() => OnSave?.Invoke();
        public void Load() => OnLoad?.Invoke();

        internal void BouncePlayer()
        {
            OnScoreUpdated?.Invoke(_currentLevelData.CurrentScore++);
            _player.Jump();
            Debug.Log("Score: " + _currentLevelData.CurrentScore);
            if (_currentLevelData.CurrentScore >= _currentLevelData.ScoreToWin)
            {
                HandleGameWon();
            }
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

        public void QuitGame()
        {
            Save();
            Application.Quit();
        }

        private void Start()
        {
            _player = FindObjectOfType<PlayerMovementController>();

            if (LevelManager.TryGetInstance(out LevelManager manager))
            {
                _currentLevelData = manager.CurrentLevel;
                OnScoreUpdated?.Invoke(_currentLevelData.CurrentScore);
            }
        }
        private void Update()
        {
            if (Time.time - _lastTick > _tickLength)
            {
                _currentLevelData.RemainingTimeOfLevel--;
                _lastTick = Time.time;
            }

            if (_currentLevelData.RemainingTimeOfLevel <= 0)
            {
                HandleGameLost();
                return;
            }
            else
            {
                OnRemainingTimeUpdated?.Invoke(_currentLevelData.RemainingTimeOfLevel);
            }
        }

        private void HandleGameWon()
        {
            OnRemainingTimeUpdated?.Invoke(0);
            OnWin?.Invoke();
            SceneManager.LoadScene("menu");
        }

        private void HandleGameLost()
        {
            OnLose?.Invoke();
            SceneManager.UnloadSceneAsync("Level");
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