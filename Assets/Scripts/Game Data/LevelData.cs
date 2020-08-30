// Copyright (c) Breach AS. All rights reserved.

namespace Breach
{
    [System.Serializable]
    public class LevelData
    {
        public bool IsCompleted;
        public int ScoreToWin;
        public int CurrentScore;
        public int DudeNumberToSpawn;
        public float RemainingTimeOfLevel;

        public LevelData(bool isCompleted, int scoreToWin, int currentScore, int dudeNumberToSpawn, float remainingTimeOfLevel)
        {
            IsCompleted = isCompleted;
            ScoreToWin = scoreToWin;
            CurrentScore = currentScore;
            DudeNumberToSpawn = dudeNumberToSpawn;
            RemainingTimeOfLevel = remainingTimeOfLevel;
        }
    }
}