using UnityEngine;

// INHERITANCE
public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] GameObject _build;
    [SerializeField] GameObject _rotate;
    [SerializeField] GameObject _explode;
    [SerializeField] GameObject _win;
    [SerializeField] GameObject _loss;

    //    EventManager.Instance.OnObjectRemoved

    public void Rotate()
    {
        if (!_rotate.activeInHierarchy) _rotate.SetActive(true);
    }

    public void Explode()
    {
        if (!_explode.activeInHierarchy) _explode.SetActive(true);
    }

    public void Win()
    {
        if (!_win.activeInHierarchy) _win.SetActive(true);
    }

    public void Loss()
    {
        if (!_loss.activeInHierarchy) _loss.SetActive(true);
    }

    public void Build()
    {
        if (!_build.activeInHierarchy) _build.SetActive(true);
    }

    // POLYMORPHISM - overriding OnDestroy()
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

}
