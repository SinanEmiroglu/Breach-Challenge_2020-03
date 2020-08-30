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

        private float _tickLength = 1f;
        private float _lastTick;
        private PlayerMovement _player;
        private LevelData _currentLevel;

        public void Save() => OnSave?.Invoke();
        public void Load() => OnLoad?.Invoke();

        internal void BouncePlayer()
        {
            OnScoreUpdated?.Invoke(_currentLevel.CurrentScore++);

            if (_currentLevel.CurrentScore >= _currentLevel.ScoreToWin)
            {
                HandleGameWon();
            }

            _player.Jump();
        }

        public void SetLevelData(LevelData data)
        {
            _currentLevel = data;
        }

        private void Start()
        {
            _player = FindObjectOfType<PlayerMovement>();
            _currentLevel = LevelManager.Instance.CurrentLevel;
        }

        private void Update()
        {
            if (Time.time - _lastTick > _tickLength)
            {
                _currentLevel.RemainingTimeOfLevel--;
                _lastTick = Time.time;
            }

            if (_currentLevel.RemainingTimeOfLevel <= 0)
            {
                HandleGameLost();
                return;
            }
        }

        private void HandleGameWon()
        {
            OnWin?.Invoke();
            SceneManager.LoadScene("menu");
        }

        private void HandleGameLost()
        {
            OnLose?.Invoke();
            SceneManager.UnloadSceneAsync("Level");
        }

        private void OnGUI()
        {
            if (GUI.Button(new Rect(10, 10, 150, 50), "Save Level"))
            {
                Save();
                //print("Save the current level.");
            }

            if (GUI.Button(new Rect(170, 10, 150, 50), "Load Level"))
                Load();
            //print("Replaces current level with one loaded from a previous save");

            if (GUI.Button(new Rect(10, 70, 150, 50), "Save GameState"))
                print("Save transforms and velocities of all dynamic game object and states");

            if (GUI.Button(new Rect(170, 70, 150, 50), "Load GameState"))
                print("Loads previously saved dynamic game objects, restoring their transforms and velocities");

            GUI.TextField(new Rect(10, 130, 150, 20), "Remaining Time: " + _currentLevel.RemainingTimeOfLevel.ToString());

        }
    }
}