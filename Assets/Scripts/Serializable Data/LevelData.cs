// Copyright (c) Breach AS. All rights reserved.

namespace Breach
{
    [System.Serializable]
    public class LevelData
    {
        public string LevelName;
        public int ScoreToWin;
        public int CurrentScore;
        public float TimeLeftInSec;

        public LevelData(string levelName, int scoreToWin, int currentScore, float timeLeftInSec)
        {
            LevelName = levelName;
            ScoreToWin = scoreToWin;
            CurrentScore = currentScore;
            TimeLeftInSec = timeLeftInSec;
        }
    }
}