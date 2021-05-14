using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{
    [SerializeField] private Animation _playMenuAnimator;
    [SerializeField] private AnimationClip _fadeOutAnimation;
    [SerializeField] private AnimationClip _fadeInAnimation;

    [SerializeField] private Button LeftLevel5;
    [SerializeField] private Button LeftLevel4;
    [SerializeField] private Button LeftLevel3;
    [SerializeField] private Button LeftLevel2;
    [SerializeField] private Button LeftLevel1;
    [SerializeField] private Button LeftLevel0;

    [SerializeField] private Button RightLevel5;
    [SerializeField] private Button RightLevel4;
    [SerializeField] private Button RightLevel3;
    [SerializeField] private Button RightLevel2;
    [SerializeField] private Button RightLevel1;
    [SerializeField] private Button RightLevel0;

    [SerializeField] private Button RightTower;
    [SerializeField] private Button LeftTower;

    [SerializeField] private GameObject _buttonPanel;

    void Start()
    {
        LeftLevel5.onClick.AddListener(delegate { HandleLevelRotationClicked(5, -1); });
        LeftLevel4.onClick.AddListener(delegate { HandleLevelRotationClicked(4, -1); });
        LeftLevel3.onClick.AddListener(delegate { HandleLevelRotationClicked(3, -1); });
        LeftLevel2.onClick.AddListener(delegate { HandleLevelRotationClicked(2, -1); });
        LeftLevel1.onClick.AddListener(delegate { HandleLevelRotationClicked(1, -1); });
        LeftLevel0.onClick.AddListener(delegate { HandleLevelRotationClicked(0, -1); });

        RightLevel5.onClick.AddListener(delegate { HandleLevelRotationClicked(5, 1); });
        RightLevel4.onClick.AddListener(delegate { HandleLevelRotationClicked(4, 1); });
        RightLevel3.onClick.AddListener(delegate { HandleLevelRotationClicked(3, 1); });
        RightLevel2.onClick.AddListener(delegate { HandleLevelRotationClicked(2, 1); });
        RightLevel1.onClick.AddListener(delegate { HandleLevelRotationClicked(1, 1); });
        RightLevel0.onClick.AddListener(delegate { HandleLevelRotationClicked(0, 1); });

        LeftTower.onClick.AddListener(delegate { HandleTowerRotationClicked(-1); });
        RightTower.onClick.AddListener(delegate { HandleTowerRotationClicked(1); });


        EventManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        //EventManager.Instance.OnPlayMenuFadeComplete.AddListener(OnFadeOutComplete);

    }

    public void HandleLevelRotationClicked(int level, int direction)
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.RUNNING)
            return;

        Debug.Log("In PlayMenu.HandleLevelRotationClicked: " + level + " - " + direction );
        TowerManager.Instance.RotateLevel(level, direction);
    }

    public void HandleTowerRotationClicked(int direction)
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.RUNNING)
            return;

        Debug.Log("In PlayMenu.HandleTowerRotationClicked: " + direction);
        TowerManager.Instance.RotateTower(direction);

    }


    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        Debug.Log("In PlayMenu.HandleGameStateChanged");
        if ((previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING))
        {
            FadeIn();
        }

        if (previousState == GameManager.GameState.PAUSED && currentState == GameManager.GameState.PREGAME)
        {
            FadeOut();
        }
    }

    public void FadeIn()
    {
        _buttonPanel.SetActive(true);
        Debug.Log("In PlayMenu.FadeIn");

        _playMenuAnimator.Stop();
        _playMenuAnimator.clip = _fadeInAnimation;
        _playMenuAnimator.Play();

    }

    public void FadeOut()
    {
        Debug.Log("In PlayMenu.FadeOut");

        UIManager.Instance.SetDummyCameraActive(false);

        _playMenuAnimator.Stop();
        _playMenuAnimator.clip = _fadeOutAnimation;
        _playMenuAnimator.Play();
    }


    public void OnFadeOutComplete()
    {
        Debug.Log("In PlayMenu.OnFadeOutComplete");

        _buttonPanel.SetActive(false);

    }

}
