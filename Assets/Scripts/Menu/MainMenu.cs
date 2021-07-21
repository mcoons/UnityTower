using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Animation _mainMenuAnimator;
    [SerializeField] private AnimationClip _fadeOutAnimation;
    [SerializeField] private AnimationClip _fadeInAnimation;

    //public Events.EventFadeComplete OnMainMenuFadeComplete;

    public GameObject txt;

    private void Start()
    {
        EventManager.Instance.OnGameStateChanged.AddListener(HandleGameStateChanged);
        txt.GetComponent<UnityEngine.UI.Text>().text =
            "Can you master\n the \nTower of Puzzles?\n\n" +
            "Type " + GameManager.Instance._masterTypeCount.ToString() + "\n" +
            "Tower " + GameManager.Instance._levelSeed.ToString();
    }

    void HandleGameStateChanged(GameManager.GameState currentState, GameManager.GameState previousState)
    {
        if ( (previousState == GameManager.GameState.PREGAME && currentState == GameManager.GameState.RUNNING))
            FadeOut();

        if (previousState != GameManager.GameState.PREGAME && currentState == GameManager.GameState.PREGAME)
        {
            txt.GetComponent<UnityEngine.UI.Text>().text =
                "Can you master\n the \nTower of Puzzles?\n\n" +
            "Type " + GameManager.Instance._masterTypeCount.ToString() + "\n" +
            "Tower " + GameManager.Instance._levelSeed.ToString();
            FadeIn();
        }
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

