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
        //GameManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        EventManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        Debug.Log("In MainMenu.HandleGameStateChanged");
        if ( (previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING))
        {
            FadeOut();
        }

        if (previousState != GameManager.GameState.PREGAME && currentState == GameManager.GameState.PREGAME )
        {
            FadeIn();
        }
    }

    public void OnFadeOutComplete()
    {
        Debug.Log("In MainMenu.OnFadeOutComplete");

        Debug.Log("Fade Out Complete");

        EventManager.Instance.OnMainMenuFadeComplete.Invoke(true);
    }


    public void OnFadeInComplete()
    {
        Debug.Log("In MainMenu.OnFadeInComplete");

        Debug.Log("Fade In Complete");

        EventManager.Instance.OnMainMenuFadeComplete.Invoke(false);

        UIManager.Instance.SetDummyCameraActive(true);
    }

    public void FadeIn()
    {
        Debug.Log("In MainMenu.FadeIn");

        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeInAnimation;
        _mainMenuAnimator.Play();

    }

    public void FadeOut()
    {
        Debug.Log("In MainMenu.FadeOut");

        UIManager.Instance.SetDummyCameraActive(false);

        _mainMenuAnimator.Stop();
        _mainMenuAnimator.clip = _fadeOutAnimation;
        _mainMenuAnimator.Play();
    }
}

