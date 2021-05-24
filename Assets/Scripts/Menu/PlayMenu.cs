using UnityEngine;
using UnityEngine.UI;

public class PlayMenu : MonoBehaviour
{
    [SerializeField] private Animation _playMenuAnimator;
    [SerializeField] private AnimationClip _fadeOutAnimation;
    [SerializeField] private AnimationClip _fadeInAnimation;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text selectedText;


    //TODO convert to dynamic sizable x,y,z
    [SerializeField] private Button LeftLevel5;
    [SerializeField] private Button LeftLevel4;
    [SerializeField] private Button LeftLevel3;
    [SerializeField] private Button LeftLevel2;
    [SerializeField] private Button LeftLevel1;
    [SerializeField] private Button LeftLevel0;

    //TODO convert to dynamic sizable x,y,z
    [SerializeField] private Button RightLevel5;
    [SerializeField] private Button RightLevel4;
    [SerializeField] private Button RightLevel3;
    [SerializeField] private Button RightLevel2;
    [SerializeField] private Button RightLevel1;
    [SerializeField] private Button RightLevel0;

    [SerializeField] private Button RightTower;
    [SerializeField] private Button LeftTower;

    [SerializeField] private Button Clear;

    [SerializeField] private GameObject _buttonPanel;

    [SerializeField] private Text countRed;
    [SerializeField] private Text countGreen;
    [SerializeField] private Text countBlue;
    [SerializeField] private Text countOrange;
    [SerializeField] private Text countYellow;
    [SerializeField] private Text countIndigo;
    [SerializeField] private Text countViolet;


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

        Clear.onClick.AddListener(HandleClearClicked);


        EventManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        //EventManager.Instance.OnPlayMenuFadeComplete.AddListener(OnFadeOutComplete);
    }

    private void Update()
    {
        if (!(GameManager.Instance.CurrentGameState == GameManager.GameState.RUNNING ||
              GameManager.Instance.CurrentGameState == GameManager.GameState.WIN ||
              GameManager.Instance.CurrentGameState == GameManager.GameState.LOSS ))
            return;

        scoreText.text = GameManager.Instance.levelScore.ToString();
        selectedText.text = TowerManager.Instance.matchCount.ToString() + " Selected";

        countRed.text = TowerManager.Instance.typeCounts[0].ToString();
        countGreen.text = TowerManager.Instance.typeCounts[1].ToString();
        countBlue.text = TowerManager.Instance.typeCounts[2].ToString();
        countOrange.text = TowerManager.Instance.typeCounts[3].ToString();
        countYellow.text = TowerManager.Instance.typeCounts[4].ToString();
        countIndigo.text = TowerManager.Instance.typeCounts[5].ToString();
        countViolet.text = TowerManager.Instance.typeCounts[6].ToString();
    }

    public void HandleClearClicked()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.RUNNING)
            return;
        if (TowerManager.Instance.dropCount != 0)
            return;

        TowerManager.Instance.ClearItemStates();
    }

    public void HandleLevelRotationClicked(int level, int direction)
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.RUNNING)
            return;
        if (TowerManager.Instance.dropCount != 0)
            return;
        if (TowerManager.Instance.gameLost || TowerManager.Instance.gameWon)
            return;

        TowerManager.Instance.RotateLevel(level, direction);
    }

    public void HandleTowerRotationClicked(int direction)
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.RUNNING)
            return;
        if (TowerManager.Instance.dropCount != 0)
            return;

        TowerManager.Instance.RotateTower(direction);
    }


    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if ((previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING))
            FadeIn();

        if (previousState == GameManager.GameState.PAUSED && currentState == GameManager.GameState.PREGAME)
            FadeOut();
    }

    public void FadeIn()
    {
        _buttonPanel.SetActive(true);

        _playMenuAnimator.Stop();
        _playMenuAnimator.clip = _fadeInAnimation;
        _playMenuAnimator.Play();
    }

    public void FadeOut()
    {
        UIManager.Instance.SetDummyCameraActive(false);

        _playMenuAnimator.Stop();
        _playMenuAnimator.clip = _fadeOutAnimation;
        _playMenuAnimator.Play();
    }


    public void OnFadeOutComplete()
    {
        _buttonPanel.SetActive(false);
    }

}
