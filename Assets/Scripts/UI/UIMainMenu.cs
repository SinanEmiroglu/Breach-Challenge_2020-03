using UnityEngine;
using UnityEngine.UI;

namespace Breach
{
    public class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button clearSavesButton;
        [SerializeField] private Button quitButton;

        public void SetActiveMainMenu(bool value)
        {
            menuPanel.SetActive(value);

            if (value)
            {
                continueButton.interactable = false;

                if (SaveLoad.SaveExists(GameManager.SAVE_FILE_KEY))
                    continueButton.interactable = true;
            }
        }

        private void OnEnable()
        {
            SetActiveMainMenu(true);
        }

        private void Start()
        {
            continueButton.onClick.AddListener(() => GameManager.Instance.BeginLevel(GameState.Saved));
            newGameButton.onClick.AddListener(() => GameManager.Instance.BeginLevel(GameState.Fresh));
            clearSavesButton.onClick.AddListener(() => { SaveLoad.DeleteAllSaveFiles(); continueButton.interactable = false; });
            quitButton.onClick.AddListener(() => Application.Quit());
        }
    }
}