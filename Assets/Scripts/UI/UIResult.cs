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

        private UIMainMenu _mainMenu;

        private void Awake()
        {
            _mainMenu = FindObjectOfType<UIMainMenu>();
        }

        private void OnEnable()
        {
            GameManager.OnGameOver += GameOverHandler;

            nextButton.onClick.AddListener(() => GameManager.Instance.BeginNextLevel());

            menuButton.onClick.AddListener(() =>
            {
                _mainMenu.SetActiveMainMenu(true);
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(2);
            });
        }

        private void GameOverHandler(bool isWon, string message)
        {
            resultText.text = message;

            if (isWon)
            {
                nextButton.interactable = true;
            }
            else
            {
                nextButton.interactable = false;
            }
        }

        private void OnDisable()
        {
            GameManager.OnGameOver -= GameOverHandler;
        }
    }
}