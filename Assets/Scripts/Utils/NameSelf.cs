
using UnityEngine;

public class NameSelf : MonoBehaviour
{
    public string baseName;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.name = baseName +" (" + Mathf.Round( transform.position.x ) + "," + Mathf.Round( transform.position.y ) + "," + Mathf.Round( transform.position.z ) + ")";
    }
}
