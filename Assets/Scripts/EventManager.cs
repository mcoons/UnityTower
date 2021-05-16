using System.Collections;
using System.Collections.Generic;
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

    [System.Serializable] public class EventObjectSelected : UnityEvent<string, GameManager.ItemType, Vector3> { }
    [System.Serializable] public class EventObjectMatched : UnityEvent<string, GameManager.ItemType, Vector3> { }
    [System.Serializable] public class EventUnselectAll : UnityEvent { }
    [System.Serializable] public class EventObjectsRemoved : UnityEvent { }
}

public class EventManager : Singleton<EventManager>
{

    // GameManager
    public CustomEvents.EventGameState OnGameStateChanged;

    // MainMenu
    public CustomEvents.EventFadeComplete OnMainMenuFadeComplete;

    // Item
    public CustomEvents.EventObjectSelected OnObjectSelected;
    public CustomEvents.EventObjectMatched OnObjectMatched;

    // TODO
    public CustomEvents.EventUnselectAll OnUnselectAll;
    public CustomEvents.EventObjectsRemoved OnObjectsRemoved;

    // TowerManager
    public CustomEvents.EventTowerAnimationStart OnTowerAnimationStart;
    public CustomEvents.EventTowerAnimationComplete OnTowerAnimationComplete;

    public CustomEvents.EventLevelAnimationStart OnLevelAnimationStart;
    public CustomEvents.EventLevelAnimationComplete OnLevelAnimationComplete;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
