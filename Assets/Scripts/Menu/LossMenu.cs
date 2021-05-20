using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LossMenu : MonoBehaviour
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
        GameManager.Instance._masterTypeCount = 3 + (int)(GameManager.Instance.levelSeed / 2);
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
