using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MenuPanel
{
    [SerializeField] private Button _colorsButton;
    [SerializeField] private Button _resumeButton;

    private new void Start()
    {
        base.Start();
        _colorsButton.onClick.AddListener(HandleColorClicked);

        _resumeButton.onClick.AddListener(HandleResumeClicked);
    }

    void HandleColorClicked()
    {
        //TODO: Turn to messaging
        Debug.Log("Colors Clicked");
        GameManager.Instance.OnColorSelection();
    }

    void HandleResumeClicked()
    {
        //TODO: Turn to messaging
        GameManager.Instance.TogglePause();
    }

}
