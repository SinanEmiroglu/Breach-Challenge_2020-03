// Copyright (c) Breach AS. All rights reserved.
using UnityEngine;
using Random = UnityEngine.Random;

namespace Breach
{
    public class Dude : Spawnable
    {
        [Header("Dude Attributes")]
        [SerializeField] private int damage = 1;
        [SerializeField] private MeshRenderer meshRenderer;

        [HideInInspector] public DudeData DudeData;

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

        protected override void OnEnable()
        {
            base.OnEnable();

            DudeData.AnImportantStateValue = Random.Range(1, 10000);
            DudeData.MoveSpeed = Random.Range(3, 6);
            DudeData.Color = GetRandomColorValue();

            meshRenderer.material.color = DudeData.Color;
            _anImportantStateValue = DudeData.AnImportantStateValue;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Player player = collision.collider.GetComponent<Player>();

            if (player == null)
                return;

            var playerHealth = player.gameObject.GetComponent<Health>();

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

        private Color GetRandomColorValue() => new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
    }
}