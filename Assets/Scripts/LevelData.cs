// Copyright (c) Breach AS. All rights reserved.

using UnityEngine;

namespace Breach
{
    [System.Serializable]
    public class LevelData : MonoBehaviour
    {
        public bool IsCompleted;
        public int ScoreToWin;
        public int CurrentScore;
        public int DudeNumberToSpawn;
        public float RemainingTimeOfLevel;
        public LevelData NextLevel;
    }
}