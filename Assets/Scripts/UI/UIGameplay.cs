using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Breach
{
    public class UIGameplay : MonoBehaviour
    {
        [SerializeField] private Image healthImage;
        [SerializeField] private TextMeshProUGUI healthText;

        private Health _playerHealth;

        private void Awake()
        {
            _playerHealth = FindObjectOfType<PlayerMovement>().GetComponent<Health>();
        }

        private void OnEnable()
        {
            _playerHealth.OnHealthChanged += HealthChangedHandler;
        }

        private void HealthChangedHandler(int current, int max)
        {
            healthImage.fillAmount = current / (float)max;
            healthText.text = $"{current}/{max}";
        }

        private void OnDisable()
        {
            _playerHealth.OnHealthChanged -= HealthChangedHandler;
        }
    }
}