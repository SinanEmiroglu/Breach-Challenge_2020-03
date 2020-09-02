// Copyright (c) Breach AS. All rights reserved.

using DG.Tweening;
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
        private int _anImportantStateValue;
        private DudeMover _mover;
        private Collider _collider;

        public void SetDudeData(DudeData data)
        {
            DudeData = data;
            meshRenderer.material.color = DudeData.Color;
            _anImportantStateValue = DudeData.AnImportantStateValue;
        }

        private void Awake()
        {
            _mover = GetComponent<DudeMover>();
            _collider = GetComponent<Collider>();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            DudeData.AnImportantStateValue = Random.Range(1, 10000);
            DudeData.MoveSpeed = Random.Range(3, 6);
            DudeData.Color = GetRandomColorValue();

            meshRenderer.material.color = DudeData.Color;
            _anImportantStateValue = DudeData.AnImportantStateValue;
            _collider.enabled = true;
            _mover.enabled = true;
        }

        private void OnCollisionEnter(Collision collision)
        {
            Player player = collision.collider.GetComponent<Player>();

            if (player == null)
                return;

            var playerHealth = player.gameObject.GetComponent<Health>();

            if (collision.WasSide())
            {
                _mover.enabled = false;
                _collider.enabled = false;
                transform.DOShakeScale(0.2f, 2, 20, fadeOut: false)
                    .OnComplete(() => transform.DOScale(0, 0.25f)
                    .OnComplete(() => ReturnToPool()));

                playerHealth.TakeHit(damage);
            }
            else if (collision.WasTop())
            {
                _mover.enabled = false;
                _collider.enabled = false;
                DOTween.Sequence().Append(transform.DOScaleY(0.2f, 0.35f))
                    .Join(transform.DOScaleX(2, 0.35f))
                    .Join(transform.DOScaleZ(2, 0.35f))
                    .OnComplete(() => ReturnToPool());

                GameManager.Instance.BouncePlayer();
            }
        }

        private Color GetRandomColorValue() => new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
    }
}