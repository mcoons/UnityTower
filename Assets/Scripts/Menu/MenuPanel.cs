using UnityEngine;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] public Text _tagline;
    [SerializeField] public Button _restartButton;
    [SerializeField] public Button _nextButton;
    [SerializeField] public Button _quitButton;

    protected void Start()
    {
        _restartButton.onClick.AddListener(HandleRestartClicked);
        _nextButton.onClick.AddListener(HandleNextClicked);
        _quitButton.onClick.AddListener(HandleQuitClicked);
    }

    protected void OnEnable()
    {
        _tagline.GetComponent<UnityEngine.UI.Text>().text =
            "Type " + GameManager.Instance._masterTypeCount.ToString() + "\n" +
            "Tower #" + GameManager.Instance._levelSeed.ToString();
    }

    void HandleNextClicked()
    {
        //TODO: Turn to messaging
        GameManager.Instance._levelSeed++;
        GameManager.Instance._masterTypeCount = Mathf.Min(3 + (int)(GameManager.Instance._levelSeed / 5), 7);

        GameManager.Instance.RestartGame();
    }

    void HandleRestartClicked()
    {
        GameManager.Instance.RestartGame();
    }

    void HandleQuitClicked()
    {
        GameManager.Instance.QuitGame();
    }
}
