using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MenuPanel
{
    [SerializeField] private Button _helpButton;
    [SerializeField] private Button _optionsButton;

    [SerializeField] public Button _resumeButton;


    new void Start()
    {
        base.Start();
        _helpButton.onClick.AddListener(HandleHelpClicked);
        _optionsButton.onClick.AddListener(HandleOptionsClicked);

        _resumeButton.onClick.AddListener(HandleResumeClicked);

    }

    void HandleOptionsClicked()
    {
        //TODO: Turn to messaging
        Debug.Log("Options Clicked");
        GameManager.Instance.OnOptions();
    }


    void HandleHelpClicked()
    {
        //TODO: Turn to messaging
        Debug.Log("Help Clicked");
        GameManager.Instance.OnHelp();
    }


    void HandleResumeClicked()
    {
        GameManager.Instance.TogglePause();
    }

}
