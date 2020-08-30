// Copyright (c) Breach AS. All rights reserved.
using System;
using System.CodeDom;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Breach
{
    public class Dude : Spawnable
    {
        [SerializeField] int damage = 1;
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
            meshRenderer.material.color = DudeData.Color;
            _anImportantStateValue = DudeData.AnImportantStateValue;
        }

        private void OnCollisionEnter(Collision collision)
        {
            var player = collision.collider.GetComponent<PlayerMovement>();

            if (player == null)
                return;

            var playerHealth = player.GetComponent<Health>();

            if (collision.WasSide())
            {
                playerHealth.TakeHit(damage);
            }
            else if (collision.WasTop())
            {
                GameManager.Instance.BouncePlayer();
            }

            ReturnToPool();
        }

        private void OnEnable()
        {
            DudeData.AnImportantStateValue = Random.Range(1, 10000);
            DudeData.MoveSpeed = Random.Range(1, 4);
            DudeData.Color = GetRandomColorValue();

            meshRenderer.material.color = DudeData.Color;
            _anImportantStateValue = DudeData.AnImportantStateValue;
        }

        private Color GetRandomColorValue() => new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
    }
}