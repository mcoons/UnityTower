using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : Singleton<UIManager>
{

    [SerializeField] private MainMenu _mainMenu;
    [SerializeField] private Camera _dummyCamera;
    [SerializeField] private PauseMenu _pauseMenu;
    [SerializeField] private PlayMenu _playMenu;
    [SerializeField] private LossMenu _lossMenu;
    [SerializeField] private OptionsMenu _optionsMenu;
    [SerializeField] private ColorsMenu _colorsMenu;
    [SerializeField] private WinMenu _winMenu;

    //public Events.EventFadeComplete OnMainMenuFadeComplete;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        EventManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
        EventManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        _pauseMenu.gameObject.SetActive(currentState == GameManager.GameState.PAUSED);
        _playMenu.gameObject.SetActive(currentState != GameManager.GameState.PREGAME);
        _lossMenu.gameObject.SetActive(currentState == GameManager.GameState.LOSS);
        _winMenu.gameObject.SetActive(currentState == GameManager.GameState.WIN);
        _optionsMenu.gameObject.SetActive(currentState == GameManager.GameState.OPTIONS);
        _colorsMenu.gameObject.SetActive(currentState == GameManager.GameState.COLORSELECTION);
    }

    void HandleMainMenuFadeComplete(bool fadeOut)
    {
        //Debug.Log("In UIManager.HandleMainMenuFadeComplete");

        //EventManager.Instance.OnMainMenuFadeComplete.Invoke(fadeOut);
    }


    private void Update()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.PREGAME)
            return;

        if (SceneManager.sceneCount > 1)
            return;

        if (Input.GetKeyDown(KeyCode.Space))
            GameManager.Instance.StartGame();
    }

    public void SetDummyCameraActive(bool active)
    {
        _dummyCamera.gameObject.SetActive(active);
    }

    protected override void OnDestroy()
    {

        EventManager.Instance.OnMainMenuFadeComplete.RemoveListener(HandleMainMenuFadeComplete);
        EventManager.Instance.OnGameStateChanged.RemoveListener(HandleGameStateChanged);

        base.OnDestroy();

    }
}