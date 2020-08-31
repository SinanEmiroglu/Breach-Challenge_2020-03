using UnityEngine;
using UnityEngine.UI;

namespace Breach
{
    public class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button quitButton;

        public void SetActiveMainMenu(bool value)
        {
            menuPanel.SetActive(value);
        }

        private void Start()
        {
            continueButton.interactable = false;

            if (SaveLoad.SaveExists(GameManager.SAVE_FILE_KEY))
                continueButton.interactable = true;

            continueButton.onClick.AddListener(() =>
            {
                GameManager.Instance.BeginLevel(GameState.Saved);
            });

            newGameButton.onClick.AddListener(() =>
            {
                GameManager.Instance.BeginLevel(GameState.Fresh);
            });

            quitButton.onClick.AddListener(() => Application.Quit());
        }
    }
}