
using UnityEngine;
using UnityEngine.Events;

public class Events
{
    [System.Serializable] public class EventFadeComplete : UnityEvent<bool> { }
    [System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }
    [System.Serializable] public class EventTowerAnimationStart : UnityEvent { }
    [System.Serializable] public class EventTowerAnimationComplete : UnityEvent { }

    [System.Serializable] public class ObjectSelected : UnityEvent<GameManager.ItemType, Vector3> {}

}
