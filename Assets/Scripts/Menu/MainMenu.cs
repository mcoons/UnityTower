using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    // Track animation component
    // Track animation clips for fade in/out
    // Function that can receive animation events
    // Functions to play fade in/out animations

    [SerializeField] private Animation _mainMenuAnimator;
    [SerializeField] private AnimationClip _fadeOutAnimation;
    [SerializeField] private AnimationClip _fadeInAnimation;

    //public Events.EventFadeComplete OnMainMenuFadeComplete;

    private void Start()
    {
        EventManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if ( (previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING))
            FadeOut();

        if (previousState != GameManager.GameState.PREGAME && currentState == GameManager.GameState.PREGAME )
            FadeIn();
    }

    public void OnFadeOutComplete()
    {
        EventManager.Instance.OnMainMenuFadeComplete.Invoke(true);
    }


    public void OnFadeInComplete()
    {
        EventManager.Instance.OnMainMenuFadeComplete.Invoke(false);
        UIManager.Instance.SetDummyCameraActive(true);
    }

    public void FadeIn()
    {
        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeInAnimation;
        _mainMenuAnimator.Play();
    }

    public void FadeOut()
    {
        UIManager.Instance.SetDummyCameraActive(false);

        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeOutAnimation;
        _mainMenuAnimator.Play();
    }
}

