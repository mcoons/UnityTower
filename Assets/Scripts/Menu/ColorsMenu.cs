using UnityEngine;
using UnityEngine.UI;

public class ColorsMenu : MenuPanel
{
    [SerializeField] private Button _resumeButton;

    new void Start()
    {
        base.Start();
        _resumeButton.onClick.AddListener(HandleResumeClicked);
    }

    new void OnEnable()
    {
        _tagline.GetComponent<UnityEngine.UI.Text>().text =
            "Type " + GameManager.Instance._masterTypeCount.ToString() + "\n" +
            "Tower #" + GameManager.Instance._levelSeed.ToString();
    }

    void HandleResumeClicked()
    {
        //TODO: Turn to messaging
        GameManager.Instance.TogglePause();
    }

}
