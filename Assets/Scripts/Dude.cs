// Copyright (c) Breach AS. All rights reserved.
using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Breach
{
    public class Dude : MonoBehaviour
    {
        public DudeData DudeData;
        public event Action<Dude> OnDestroyed = delegate { };

        /// <summary>
        /// This is a state that needs to be saved and restored along with the saved game state
        /// </summary>
        /// 
        int _anImportantStateValue;

        public void SetDudeData(DudeData data)
        {
            DudeData = data;
            DudeData.MoveSpeed = Random.Range(29, 31);
            _anImportantStateValue = data.AnImportantStateValue;
        }

        private void OnDestroy()
        {
            OnDestroyed?.Invoke(this);
        }
    }
}