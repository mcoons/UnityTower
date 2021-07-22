using UnityEngine;
using UnityEngine.UI;

public class HelpMenu : MenuPanel
{
    [SerializeField] public Button _resumeButton;

    new void Start()
    {
        base.Start();
        _resumeButton.onClick.AddListener(HandleResumeClicked);
    }

    new void OnEnable()
    {
        // override
    }

    void HandleResumeClicked()
    {
        GameManager.Instance.TogglePause();
    }

}
