using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;


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
        PAUSED
    }

    public enum ItemType
    {
        SPHERE_RED,
        SPHERE_GREEN,
        SPHERE_BLUE,

        MATCHED,

        WIZARD,
        DEATH
    }

    public Material[] _materials;

    //public Events.EventGameState OnGameStateChanged;

    public GameObject[] SystemPrefabs;  // list of system prefabs to load that live in the scope of boot
    private List<GameObject> _instancedSystemPrefabs;  // list of which SystemPrefabs have been instanced

    [SerializeField] private string _currentLevelName = string.Empty;
    [SerializeField] List<AsyncOperation> _loadOperations;  // used to track active load operations 

    [SerializeField] GameState _currentGameState = GameState.PREGAME;

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
    }

    private void Update()
    {
        if (_currentGameState == GameState.PREGAME)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
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
            }
        }
        Debug.Log("Load finished");
    }

    private void OnUnloadOperationComplete(AsyncOperation ao)
    {
        _loadOperations.Remove(ao);
        Debug.Log("Unload finished");
    }

    void HandleMainMenuFadeComplete(bool fadeOut)
    {
        if (!fadeOut)
        {
            UnloadLevel(_currentLevelName);
        }
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

            default:

                break;

        }

        Debug.Log(previousGameState + " -> " + _currentGameState);
        //OnGameStateChanged.Invoke(_currentGameState, previousGameState);
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
        Debug.Log("In LoadLevel");

        // loading additively so boot scene persists
        AsyncOperation ao = SceneManager.LoadSceneAsync(levelName, LoadSceneMode.Additive);
        if (ao == null)
        {
            Debug.LogError("[Game Manager] Unable to load level " + levelName);
            return;
        }

        // used to track active load operations 
        _loadOperations.Add(ao);

        Debug.Log("Adding Load Operation for " + levelName);

        // add the event listener
        ao.completed += OnLoadOperationComplete;
        _currentLevelName = levelName;
    }

    public void UnloadLevel(string levelName)
    {
        Debug.Log("In UnloadLevel");

        AsyncOperation ao =  SceneManager.UnloadSceneAsync(levelName);
        if (ao == null)
        {
            Debug.LogError("[Game Manager] Unable to unload level " + levelName);
            return;
        }

        // used to track active load operations
        _loadOperations.Add(ao);

        Debug.Log("Adding UNLoad Operation for " + levelName);


        // add the event listener
        ao.completed += OnUnloadOperationComplete;

    }

    protected override void OnDestroy()
    {
        base.OnDestroy();

        GameObject prefabInstance;
        for (int i = 0; i < _instancedSystemPrefabs.Count; i++)
        {
            Destroy(_instancedSystemPrefabs[i]);
        }

        _instancedSystemPrefabs.Clear();
    }

    public void StartGame()
    {
        Debug.Log("In StartGame");
        LoadLevel("Main");
    }

    public void TogglePause()
    {
        Debug.Log("In TogglePause");
        UpdateState(_currentGameState == GameState.RUNNING ? GameState.PAUSED : GameState.RUNNING);
    }

    public void RestartGame()
    {
        Debug.Log("In RestartGame");
        UpdateState(GameState.PREGAME);
    }


    public void QuitGame()
    {
        Application.Quit();
    }

     
}
