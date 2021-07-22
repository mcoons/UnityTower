using UnityEngine;
using UnityEngine.UI;

public class WinMenu : MenuPanel
{
    [SerializeField] private Button _optionsButton;

    private new void Start()
    {
        base.Start();
        _optionsButton.onClick.AddListener(HandleOptionsClicked);
    }

    void HandleOptionsClicked()
    {
        //TODO: Turn to messaging
        Debug.Log("Options Clicked");
        GameManager.Instance.OnOptions();
    }

}
