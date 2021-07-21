using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LossMenu : MonoBehaviour
{
    [SerializeField] private Text _tagline;
    [SerializeField] private Button _optionsButton;
    [SerializeField] private Button _nextButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _quitButton;

    private void Start()
    {
        _optionsButton.onClick.AddListener(HandleOptionsClicked);
        _nextButton.onClick.AddListener(HandleNextClicked);
        _restartButton.onClick.AddListener(HandleRestartClicked);
        _quitButton.onClick.AddListener(HandleQuitClicked);

    }

    private void OnEnable()
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

    void HandleNextClicked()
    {
        //TODO: Turn to messaging
        GameManager.Instance._levelSeed++;
        GameManager.Instance._masterTypeCount = Mathf.Min(3 + (int)(GameManager.Instance._levelSeed / 5), 7);

        GameManager.Instance.RestartGame();
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
