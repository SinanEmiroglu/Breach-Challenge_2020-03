// Copyright (c) Breach AS. All rights reserved.

using UnityEngine;

namespace Breach
{
    public class LevelManager : Singleton<LevelManager>
    {
        private const string SAVE_FILE_KEY = "Level";

        [SerializeField] private LevelData[] levels;

        public LevelData CurrentLevel { get; private set; }

        private void OnEnable()
        {
            GameManager.OnSave += SaveHandler;
            GameManager.OnLoad += LoadHandler;
            GameManager.OnWin += WinHandler;
        }

        private void WinHandler()
        {
            CurrentLevel.IsCompleted = true;
            if (CurrentLevel.NextLevel != null)
            {
                CurrentLevel = CurrentLevel.NextLevel;
            }
        }

        private void LoadHandler()
        {
            if (!SaveLoad.SaveExists(nameof(SAVE_FILE_KEY)))
                return;

            CurrentLevel = SaveLoad.Load<LevelData>(nameof(SAVE_FILE_KEY));
        }

        private void SaveHandler()
        {
            SaveLoad.Save(CurrentLevel, nameof(SAVE_FILE_KEY));
        }

        private void OnDisable()
        {
            GameManager.OnSave -= SaveHandler;
            GameManager.OnLoad -= LoadHandler;
            GameManager.OnWin -= WinHandler;
        }
    }
}