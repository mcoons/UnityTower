using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    [SerializeField] private Text   _tagline;
    [SerializeField] private Button _colorsButton;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _quitButton;

    private void Start()
    {
        _colorsButton.onClick.AddListener(HandleColorClicked);
        _resumeButton.onClick.AddListener(HandleResumeClicked);
        _nextButton.onClick.AddListener(HandleNextClicked);
        _restartButton.onClick.AddListener(HandleRestartClicked);
        _quitButton.onClick.AddListener(HandleQuitClicked);
    }

    private void OnEnable()
    {
        _tagline.GetComponent<UnityEngine.UI.Text>().text =
            "Type " + GameManager.Instance._masterTypeCount.ToString() + "\n" +
            "Tower " + GameManager.Instance.levelSeed.ToString();
    }


    void HandleColorClicked()
    {
        //TODO: Turn to messaging
        Debug.Log("Colors Clicked");
        GameManager.Instance.OnColorSelection();
    }

    void HandleNextClicked()
    {
        //TODO: Turn to messaging
        GameManager.Instance.levelSeed++;
        GameManager.Instance._masterTypeCount = Mathf.Min( 3 + (int)(GameManager.Instance.levelSeed / 5), 7);

        GameManager.Instance.RestartGame();
    }

    void HandleResumeClicked()
    {
        //TODO: Turn to messaging
        GameManager.Instance.TogglePause();
    }

    void HandleRestartClicked()
    {
        //TODO: Turn to messaging
        GameManager.Instance.RestartGame();
    }

    void HandleQuitClicked()
    {
        //TODO: Turn to messaging
        GameManager.Instance.QuitGame();
    }

}
