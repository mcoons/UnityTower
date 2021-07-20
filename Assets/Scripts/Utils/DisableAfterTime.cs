using System.Collections;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    public float delay = 1.0f;
    private IEnumerator delayDisableCoroutine;

    private void OnEnable()
    {
        delayDisableCoroutine = WaitAndDisable();
        StartCoroutine(delayDisableCoroutine);
    }

    private IEnumerator WaitAndDisable()
    {
        while (true)
        {
            yield return new WaitForSeconds(delay);
            gameObject.SetActive(false);
        }
    }
}
