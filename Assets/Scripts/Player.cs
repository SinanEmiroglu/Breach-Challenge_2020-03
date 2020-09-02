using UnityEngine;

namespace Breach
{
    public class Player : MonoBehaviour
    {
        const string SAVE_FILE_KEY = "Player";

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
            if (SaveLoad.SaveExists(SAVE_FILE_KEY))
            {
                PlayerData = SaveLoad.Load<PlayerData>(SAVE_FILE_KEY);

                _health.SetCurrentHealth(PlayerData.Health);
                _transform.position = PlayerData.Position;
                _transform.rotation = PlayerData.Rotation;
            }
        }

        private void Save()
        {
            SaveLoad.Save(PlayerData, SAVE_FILE_KEY);
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
            GameManager.OnSave -= Save;
            GameManager.OnLoad -= Load;
        }
    }
}