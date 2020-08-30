// Copyright (c) Breach AS. All rights reserved.

namespace Breach
{
    public class LevelManager : Singleton<LevelManager>
    {
        private const string SAVE_FILE_KEY = "Level";

        public LevelData CurrentLevel { get; private set; }

        private int _levelIndex = 0;
        private LevelData[] _levels;

        private void Start()
        {
            _levels = new[] {
                new LevelData(isCompleted: false, scoreToWin: 6, currentScore: 0, dudeNumberToSpawn: 15, remainingTimeOfLevel: 60),
                new LevelData(isCompleted: false, scoreToWin: 4, currentScore: 0, dudeNumberToSpawn: 20, remainingTimeOfLevel: 80)
            };

            CurrentLevel = _levels[_levelIndex];
        }

        private void OnEnable()
        {
            GameManager.OnSave += SaveHandler;
            GameManager.OnLoad += LoadHandler;
            GameManager.OnWin += WinHandler;
        }

        private void WinHandler()
        {
            CurrentLevel.IsCompleted = true;
            CurrentLevel = _levels[_levelIndex++];
            SaveHandler();
        }

        private void LoadHandler()
        {
            if (SaveLoad.SaveExists(SAVE_FILE_KEY))
            {
                CurrentLevel = SaveLoad.Load<LevelData>(SAVE_FILE_KEY);
            }
        }

        private void SaveHandler()
        {
            SaveLoad.Save(CurrentLevel, SAVE_FILE_KEY);
        }

        private void OnDisable()
        {
            GameManager.OnSave -= SaveHandler;
            GameManager.OnLoad -= LoadHandler;
            GameManager.OnWin -= WinHandler;
        }
    }
}