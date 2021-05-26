using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTime : MonoBehaviour
{
    public float delay = 1.0f;
    private IEnumerator delayDisableCoroutine;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
