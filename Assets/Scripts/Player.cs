using UnityEngine;

namespace Breach
{
    public class Player : MonoBehaviour
    {
        [HideInInspector] public PlayerData PlayerData;

        private Health _health;
        private Transform _transform;

        private void Awake()
        {
            _transform = transform;
            _health = GetComponent<Health>();
        }

        private void OnEnable()
        {
            _health.OnHealthChanged += HealthChangedHandler;
            GameManager.OnSave += Save;
            GameManager.OnLoad += Load;
        }

        private void Load()
        {
            if (SaveLoad.SaveExists("Player"))
            {
                PlayerData = SaveLoad.Load<PlayerData>("Player");

                _health.SetCurrentHealth(PlayerData.Health);
                _transform.position = PlayerData.Position;
                _transform.rotation = PlayerData.Rotation;
            }
        }

        private void Save()
        {
            SaveLoad.Save(PlayerData, "Player");
        }

        private void Update()
        {
            PlayerData.Position = _transform.position;
            PlayerData.Rotation = _transform.rotation;
        }

        private void HealthChangedHandler(int current, int max)
        {
            PlayerData.Health = current;
        }

        private void OnDisable()
        {
            _health.OnHealthChanged -= HealthChangedHandler;
        }
    }
}