using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    // level
    // load/unload levels
    // game state
    // generate other persistent systems

    public enum GameState
    {
        PREGAME,
        RUNNING,
        PAUSED,
        OPTIONS,
        COLORSELECTION,
        WIN,
        LOSS
    }

    public int totalScore = 0;
    public int levelScore = 0;


    public GameObject[] SystemPrefabs;  // list of system prefabs to load that live in the scope of boot
    private List<GameObject> _instancedSystemPrefabs;  // list of which SystemPrefabs have been instanced
    [SerializeField] List<AsyncOperation> _loadOperations;  // used to track active load operations 

    [SerializeField] GameState _currentGameState = GameState.PREGAME;
    [SerializeField] private string _currentLevelName = string.Empty;
    public int levelSeed = 1;
    public int _masterTypeCount = 3;

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        private set { _currentGameState = value; }
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _instancedSystemPrefabs = new List<GameObject>();
        _loadOperations = new List<AsyncOperation>();

        InstantiateSystemPrefabs();

        EventManager.Instance.OnMainMenuFadeComplete.AddListener(HandleMainMenuFadeComplete);
        EventManager.Instance.OnGameLoss.AddListener(OnGameLoss);
        EventManager.Instance.OnGameWin.AddListener(OnGameWin);
    }

    private void Update()
    {
        if (_currentGameState == GameState.PREGAME)
            return;

        if (Input.GetKeyDown(KeyCode.Escape) /* &&
            (_currentGameState == GameState.RUNNING ||
            _currentGameState == GameState.PAUSED ||
            _currentGameState == GameState.OPTIONS ||
            _currentGameState == GameState.COLORSELECTION)*/
            )
            TogglePause();
    }

    private void OnLoadOperationComplete(AsyncOperation ao)
    {
        Debug.Log("In OnLoadOperationComplete");
        Debug.Log("_loadOperations.Count: " + _loadOperations.Count);
        if (_loadOperations.Contains(ao))
        {
            _loadOperations.Remove(ao);

            if (_loadOperations.Count == 0)
            {
                UpdateState(GameState.RUNNING);
                AudioManager.Instance.Build();
            }
        }
    }

    private void OnUnloadOperationComplete(AsyncOperation ao)
    {
        _loadOperations.Remove(ao);
    }

    void HandleMainMenuFadeComplete(bool fadeOut)
    {
        if (!fadeOut)
            UnloadLevel(_currentLevelName);
    }

    void UpdateState(GameState state)
    {
        Debug.Log("In UpdateState");
        GameState previousGameState = _currentGameState;
        _currentGameState = state;

        switch (_currentGameState)
        {
            case GameState.PREGAME:
                Debug.Log("Pre Game");

                GameManager.Instance.levelScore = 0;
                Time.timeScale = 1.0f;
                break;

            case GameState.RUNNING:
                Debug.Log("Running Game");

                Time.timeScale = 1.0f;
                break;

            case GameState.PAUSED:
                Debug.Log("Pausing Game");
                Time.timeScale = 0.0f;
                break;

            case GameState.OPTIONS:
                Debug.Log("Options Menu");
                Time.timeScale = 0.0f;
                break;

            case GameState.COLORSELECTION:
                Debug.Log("Colors Menu");
                Time.timeScale = 0.0f;
                break;

            case GameState.WIN:
                Debug.Log("Game Won");
                Time.timeScale = 1.0f;
                totalScore += levelScore;
                break;

            case GameState.LOSS:
                Debug.Log("Game Lost");
                Time.timeScale = 1.0f;
                break;
            default:

                break;

        }

        Debug.Log(previousGameState + " -> " + _currentGameState);
        EventManager.Instance.OnGameStateChanged.Invoke(_currentGameState, previousGameState);

    }

    void InstantiateSystemPrefabs()
    {
        GameObject prefabInstance;
        for (int i = 0; i < SystemPrefabs.Length; i++)
        {
            prefabInstance = Instantiate(SystemPrefabs[i]);
            _instancedSystemPrefabs.Add(prefabInstance);
        }
    }

    public void LoadLevel(string levelName)
    {
        // loading additively so boot scene persists
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError("[Game Manager] Unable to load level " + levelName);
            return;
        }

        // used to track active load operations 
        _loadOperations.Add(ao);

        Debug.Log("[Game Manager] Adding Load Operation for " + levelName);

        // add the event listener
        ao.completed += OnLoadOperationComplete;
        _currentLevelName = levelName;
    }

    public void UnloadLevel(string levelName)
    {
        AsyncOperation ao =  SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError("[Game Manager] Unable to unload level " + levelName);
            return;
        }

        // used to track active load operations
        _loadOperations.Add(ao);

        Debug.Log("[Game Manager] Adding UNLoad Operation for " + levelName);


        // add the event listener
        ao.completed += OnUnloadOperationComplete;

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        for (int i = 0; i < _instancedSystemPrefabs.Count; i++)
            Destroy(_instancedSystemPrefabs[i]);

        _instancedSystemPrefabs.Clear();

        EventManager.Instance.OnMainMenuFadeComplete.RemoveListener(HandleMainMenuFadeComplete);
        EventManager.Instance.OnGameLoss.RemoveListener(OnGameLoss);
        EventManager.Instance.OnGameWin.RemoveListener(OnGameWin);
    }

    public void StartGame()
    {
        LoadLevel("Main");
    }

    public void TogglePause()
    {
        //UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
        if (_currentGameState == GameState.PAUSED)
        {
            UpdateState(GameState.RUNNING);
        }
        else
        
        if (_currentGameState == GameState.RUNNING && TowerManager.Instance.gameWon)
        {
            UpdateState(GameState.WIN);
        }

        else
        if (_currentGameState == GameState.RUNNING && TowerManager.Instance.gameLost)
        {
            UpdateState(GameState.LOSS);
        }
        else
        if (_currentGameState == GameState.RUNNING )
        {
            UpdateState(GameState.PAUSED);
        }
        else
        if (_currentGameState == GameState.OPTIONS)
        {
            UpdateState(GameState.PAUSED);
        }
        else
        if (_currentGameState == GameState.COLORSELECTION)
        {
            UpdateState(GameState.OPTIONS);
        }
        else
        if (_currentGameState == GameState.WIN || _currentGameState == GameState.LOSS)
        {
            UpdateState(GameState.RUNNING);
        }
    }

    public void OnOptions()
    {
        UpdateState(GameState.OPTIONS);
    }

    public void OnColorSelection()
    {
        UpdateState(GameState.COLORSELECTION);
    }

    public void RestartGame()
    {
        UpdateState(GameState.PREGAME);
    }

    public void OnGameLoss()
    {
        UpdateState(GameState.LOSS);
    }

    public void OnGameWin()
    {
        UpdateState(GameState.WIN);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

     
}
