using UnityEngine;
using UnityEngine.UI;

public class LossMenu : MenuPanel
{
    [SerializeField] private Button _optionsButton;

    private new void Start()
    {
        base.Start();
        _optionsButton.onClick.AddListener(HandleOptionsClicked);
    }

    private new void OnEnable()
    {
        _tagline.GetComponent<UnityEngine.UI.Text>().text =
            "Type " + GameManager.Instance._masterTypeCount.ToString() + "\n" +
            "Tower " + GameManager.Instance._levelSeed.ToString() + "\n\n" +
            "Press 'Esc' to toggle\nTower view";
    }

    void HandleOptionsClicked()
    {
        //TODO: Turn to messaging
        Debug.Log("Options Clicked");
        GameManager.Instance.OnOptions();
    }

}
