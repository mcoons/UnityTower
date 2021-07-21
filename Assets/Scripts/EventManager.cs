using UnityEngine;
using UnityEngine.Events;

public class CustomEvents
{
    [System.Serializable] public class EventFadeComplete : UnityEvent<bool> { }
    [System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }

    [System.Serializable] public class EventTowerAnimationStart : UnityEvent { }
    [System.Serializable] public class EventTowerAnimationComplete : UnityEvent { }

    [System.Serializable] public class EventLevelAnimationStart : UnityEvent { }
    [System.Serializable] public class EventLevelAnimationComplete : UnityEvent { }

    //[System.Serializable] public class EventObjectCountChange : UnityEvent<TowerManager.ItemType, int> { }
    [System.Serializable] public class EventObjectAdded : UnityEvent<TowerManager.ItemType, GameObject> { }

    [System.Serializable] public class EventObjectSelected : UnityEvent<string, TowerManager.ItemType, Vector3> { }
    [System.Serializable] public class EventObjectMatched : UnityEvent<string, TowerManager.ItemType, Vector3> { }
    [System.Serializable] public class EventUnselectAll : UnityEvent { }
    [System.Serializable] public class EventObjectRemoved : UnityEvent { }


    [System.Serializable] public class EventObjectDropStart : UnityEvent { }
    [System.Serializable] public class EventObjectDropComplete : UnityEvent { }

    [System.Serializable] public class EventGameLoss : UnityEvent { }
    [System.Serializable] public class EventGameWon : UnityEvent { }


}

//** usage examples **//
// EventManager.Instance.OnObjectMatched.AddListener(HandleOnObjectMatched);
// EventManager.Instance.OnObjectMatched.Invoke(transform.name, type, _globalPosition);

// INHERITANCE
public class EventManager : Singleton<EventManager>
{
    // GameManager
    public CustomEvents.EventGameState OnGameStateChanged;

    // MainMenu
    public CustomEvents.EventFadeComplete OnMainMenuFadeComplete;

    // Item
    public CustomEvents.EventObjectAdded OnObjectAdded;
    //public CustomEvents.EventObjectCountChange OnObjectCountChange;
    public CustomEvents.EventObjectSelected OnObjectSelected;
    public CustomEvents.EventObjectMatched OnObjectMatched;

    // TODO
    //public CustomEvents.EventUnselectAll OnUnselectAll;
    public CustomEvents.EventObjectRemoved OnObjectRemoved;

    // TowerManager
    //public CustomEvents.EventTowerAnimationStart OnTowerAnimationStart;
    //public CustomEvents.EventTowerAnimationComplete OnTowerAnimationComplete;

    //public CustomEvents.EventLevelAnimationStart OnLevelAnimationStart;
    //public CustomEvents.EventLevelAnimationComplete OnLevelAnimationComplete;

    //public CustomEvents.EventObjectDropStart OnObjectDropStart;
    public CustomEvents.EventObjectDropComplete OnObjectDropComplete;


    public CustomEvents.EventGameLoss OnGameLoss;
    public CustomEvents.EventGameLoss OnGameWin;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // POLYMORPHISM - overriding OnDestroy()
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
