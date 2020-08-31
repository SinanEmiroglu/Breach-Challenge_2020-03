using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Breach
{
    public class UIResult : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI resultText;
        [SerializeField] private Button replayButton;
        [SerializeField] private Button nextButton;
        [SerializeField] private Button menuButton;

        private int _actualScore;
        private int _expectedScore;
        private UIMainMenu _mainMenu;

        private void Awake()
        {
            _mainMenu = FindObjectOfType<UIMainMenu>();
        }

        private void OnEnable()
        {
            GameManager.OnGameOver += GameOverHandler;
            GameManager.OnScoreUpdated += (actual, expected) =>
            {
                _actualScore = actual;
                _expectedScore = expected;
            };

            replayButton.onClick.AddListener(() => GameManager.Instance.ReplayLastLevel());

            nextButton.onClick.AddListener(() => GameManager.Instance.BeginNextLevel());

            menuButton.onClick.AddListener(() =>
            {
                _mainMenu.SetActiveMainMenu(true);
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(2);
            });
        }

        private void GameOverHandler(bool isWon)
        {
            if (isWon)
            {
                nextButton.interactable = true;
                resultText.text = $"Congratulation! Score: <b>{_actualScore}/{_expectedScore}</b>";
            }
            else
            {
                nextButton.interactable = false;
                resultText.text = $"Time's Up! Score: <b>{_actualScore}/{_expectedScore}</b>";
            }
        }
    }
}