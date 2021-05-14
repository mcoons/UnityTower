using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField] Vector3 _globalPosition;

    public string baseName;
    public GameManager.ItemType _type = GameManager.ItemType.SPHERE_RED;

    public bool beenChecked = false;
    public bool matches = false;




    // Start is called before the first frame update
    void Start()
    {
        EventManager.Instance.OnObjectSelected.AddListener(OnObjectSelected);
        EventManager.Instance.OnObjectMatched.AddListener(OnObjectMatched);
        SetMaterial();
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void LateUpdate()
    {
        transform.name = baseName + " (" + Mathf.Round(transform.position.x) + "," + Mathf.Round(transform.position.y) + "," + Mathf.Round(transform.position.z) + ")";
        _globalPosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));

        if (_globalPosition.y == 4)
        {
            _type = GameManager.ItemType.SPHERE_BLUE;
            SetMaterial();
        }
    }

    private void OnMouseDown()
    {
        if (_globalPosition.z >= 0) return;
        Debug.Log(transform.name + " was clicked.");


        EventManager.Instance.OnObjectSelected.Invoke(_type, _globalPosition);
    }

    public void OnObjectSelected(GameManager.ItemType type, Vector3 gPosition)
    {

        if (gPosition == _globalPosition)
        {   // it is me
            matches = true;
            transform.GetChild(0).gameObject.SetActive(true);

            return;
        }
        else
        {   // it is not me
            matches = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }


        EventManager.Instance.OnObjectMatched.Invoke(type, _globalPosition);


        //// check if I am a neighbor
        //if (Vector3.Distance(gPosition, _globalPosition) < 1.25 && _type == type)
        //{
        //    matches = true;
        //    transform.GetChild(0).gameObject.SetActive(true);
        //    EventManager.Instance.OnObjectMatched.Invoke(_type, _globalPosition);
        //}
        //else
        //{

        //}


    }

    public void OnObjectMatched(GameManager.ItemType type, Vector3 gPosition)
    {
        if (matches) return;

        if (Vector3.Distance(gPosition, _globalPosition) < 1.05 && _type == type)
        {
            matches = true;
            Debug.Log(transform.name + " was matched");
            EventManager.Instance.OnObjectMatched.Invoke(type, _globalPosition);
            transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    public void SetMaterial()
    {
        GetComponent<Renderer>().material = GameManager.Instance._materials[(int)_type];
    }
}
