using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Breach
{
    public class UIGameplay : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI scoreText;
        [SerializeField] private TextMeshProUGUI levelNameText;

        [Space, Header("Health Bar")]
        [SerializeField] private Image healthImage;
        [SerializeField] private TextMeshProUGUI healthText;
        [SerializeField] private TextMeshProUGUI remainingTimeText;

        [Space, Header("Menu")]
        [SerializeField] private GameObject pausePanel;
        [SerializeField] private GameObject ingameMenuPanel;
        [SerializeField] private Button menuButton;
        [SerializeField] private Button returnButton;
        [SerializeField] private Button saveButton;
        [SerializeField] private Button loadButton;
        [SerializeField] private Button quitButton;

        private Health _playerHealth;
        private GameManager _gameManager;

        private void Awake()
        {
            _playerHealth = FindObjectOfType<PlayerMovementController>().GetComponent<Health>();
        }

        private void OnEnable()
        {
            _playerHealth.OnHealthChanged += HealthChangedHandler;
            GameManager.OnRemainingTimeUpdated += (time) => remainingTimeText.text = time.ToString();
            GameManager.OnScoreUpdated += (actual, expected) => scoreText.text = $"{actual}/{expected}";
            GameManager.OnLevelLoaded += (data) => levelNameText.text = data.LevelName;
        }

        private void Start()
        {
            if (GameManager.TryGetInstance(out GameManager manager))
                _gameManager = manager;

            menuButton.onClick.AddListener(() =>
            {
                ingameMenuPanel.SetActive(false);
                _gameManager.PausePlayGame();
                pausePanel.SetActive(true);
            });

            returnButton.onClick.AddListener(() =>
            {
                ingameMenuPanel.SetActive(true);
                _gameManager.PausePlayGame();
                pausePanel.SetActive(false);
            });

            saveButton.onClick.AddListener(() =>
            {
                ingameMenuPanel.SetActive(true);
                _gameManager.Save();
                _gameManager.PausePlayGame();
                pausePanel.SetActive(false);
            });

            loadButton.onClick.AddListener(() =>
            {
                ingameMenuPanel.SetActive(true);
                _gameManager.Load();
                _gameManager.PausePlayGame();
                pausePanel.SetActive(false);
            });

            quitButton.onClick.AddListener(() =>
            {
                _gameManager.Save();
                Application.Quit();
            });
        }

        private void HealthChangedHandler(int current, int max)
        {
            healthImage.fillAmount = current / (float)max;
            healthText.text = $"{current}/{max}";
        }

        private void OnDestroy()
        {
            _playerHealth.OnHealthChanged -= HealthChangedHandler;
        }
    }
}