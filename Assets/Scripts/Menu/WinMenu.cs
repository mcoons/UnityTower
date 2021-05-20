using UnityEngine;
using UnityEngine.UI;

public class WinMenu : MonoBehaviour
{
    [SerializeField] private Button NextButton;
    [SerializeField] private Button RestartButton;
    [SerializeField] private Button QuitButton;

    private void Start()
    {
        NextButton.onClick.AddListener(HandleNextClicked);
        RestartButton.onClick.AddListener(HandleRestartClicked);
        QuitButton.onClick.AddListener(HandleQuitClicked);

    }

    void HandleNextClicked()
    {
        GameManager.Instance.levelSeed++;
        GameManager.Instance._masterTypeCount = 3 + (int)(GameManager.Instance.levelSeed / 5);

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
