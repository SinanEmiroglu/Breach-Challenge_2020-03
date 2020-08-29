// Copyright (c) Breach AS. All rights reserved.
using UnityEngine;
using Random = UnityEngine.Random;

namespace Breach
{
    public class Dude : Spawnable
    {
        [SerializeField] private MeshRenderer meshRenderer;

        public DudeData DudeData;
        /// <summary>
        /// This is a state that needs to be saved and restored along with the saved game state
        /// </summary>
        /// 
        int _anImportantStateValue;

        public void SetDudeData(DudeData data)
        {
            DudeData = data;
            meshRenderer.material.color= DudeData.Color;
            _anImportantStateValue = DudeData.AnImportantStateValue;
        }

        private void OnEnable()
        {
            DudeData.AnImportantStateValue = Random.Range(1, 10000);
            DudeData.MoveSpeed = Random.Range(1, 4);
            DudeData.Color = GetRandomColorValue();

            meshRenderer.material.color = DudeData.Color;
            _anImportantStateValue = DudeData.AnImportantStateValue;
        }

        private Color GetRandomColorValue()
        {
            return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
        }
    }
}