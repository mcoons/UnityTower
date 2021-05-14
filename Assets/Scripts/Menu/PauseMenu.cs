
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour {

    [SerializeField] private Button ResumeButton;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button QuitButton;

    private void Start()
    {
        ResumeButton.onClick.AddListener(HandleResumeClicked);
        RestartButton.onClick.AddListener(HandleRestartClicked);
        QuitButton.onClick.AddListener(HandleQuitClicked);

    }

    void HandleResumeClicked()
    {
        Debug.Log("In PauseMenu.HandleResumeClicked");

        GameManager.Instance.TogglePause();
    }

    void HandleRestartClicked()
    {
        Debug.Log("In PauseMenu.HandleRestartClicked");

        GameManager.Instance.RestartGame();
    }

    void HandleQuitClicked()
    {
        Debug.Log("In PauseMenu.HandleQuitClicked");

        GameManager.Instance.QuitGame();
    }

}
