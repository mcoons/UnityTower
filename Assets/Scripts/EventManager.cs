using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class EventManager : Singleton<EventManager>
{

    // GameManager
    public Events.EventGameState OnGameStateChanged;

    // in update state
    //    OnGameStateChanged.Invoke(_currentGameState, previousGameState);



    // MainMenu
    public Events.EventFadeComplete OnMainMenuFadeComplete;


    // inOnFadeOutComplete
    //    OnMainMenuFadeComplete.Invoke(true);
    // inOnFadeInComplete
    //    OnMainMenuFadeComplete.Invoke(false);


    // PauseMenu
    //public Events.EventFadeComplete OnPlayMenuFadeComplete;


    // Item
    public Events.ObjectSelected OnObjectSelected;
    public Events.ObjectSelected OnObjectMatched;


    // from TowerManager
    public Events.EventTowerAnimationStart OnTowerAnimationStart;
    public Events.EventTowerAnimationComplete OnTowerAnimationComplete;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }


}
