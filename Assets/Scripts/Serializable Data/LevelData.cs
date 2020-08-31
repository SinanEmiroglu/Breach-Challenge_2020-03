// Copyright (c) Breach AS. All rights reserved.

namespace Breach
{
    [System.Serializable]
    public class LevelData
    {
        public string LevelName;
        public int ScoreToWin;
        public int CurrentScore;
        public int DudeNumberToSpawn;
        public float RemainingTimeOfLevel;

        public LevelData(int scoreToWin, int currentScore, int dudeNumberToSpawn, float remainingTimeOfLevel)
        {
            ScoreToWin = scoreToWin;
            CurrentScore = currentScore;
            DudeNumberToSpawn = dudeNumberToSpawn;
            RemainingTimeOfLevel = remainingTimeOfLevel;
        }
    }
}