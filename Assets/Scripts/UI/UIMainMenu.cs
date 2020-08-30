using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Breach
{
    public class UIMainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject menuPanel;
        [SerializeField] private Button continueButton;
        [SerializeField] private Button newGameButton;
        [SerializeField] private Button quitButton;

        private void Start()
        {
            continueButton.interactable = false;

            if (SaveLoad.SaveExists("Level"))
                continueButton.interactable = true;

            continueButton.onClick.AddListener(() =>
            {
                continueButton.interactable = false;
                newGameButton.interactable = false;
                SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive).completed += (opr) =>
                {
                    newGameButton.interactable = true;
                    GameManager.Instance.Load();
                    menuPanel.SetActive(false);
                };
            });

            newGameButton.onClick.AddListener(() =>
            {
                newGameButton.interactable = false;

                SceneManager.LoadSceneAsync(1, LoadSceneMode.Additive).completed += (opr) =>
                {
                    if (opr.isDone)
                    {
                        newGameButton.interactable = true;
                        //GameManager.Instance.Load();
                        menuPanel.SetActive(false);

                    }
                };
            });

            quitButton.onClick.AddListener(() => Application.Quit());
        }
    }
}