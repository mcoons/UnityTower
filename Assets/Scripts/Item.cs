using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField] Vector3 _globalPosition;

    public string baseName;
    public GameManager.ItemType _type = GameManager.ItemType.SPHERE_RED;

    //public bool beenChecked = false;
    public bool matched = false;


    // Start is called before the first frame update
    void Start()
    {
        _globalPosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));

        EventManager.Instance.OnObjectMatched.AddListener(HandleOnObjectMatched);

        int index = Random.Range(0, 4);
        if (index == 0)
            _type = GameManager.ItemType.SPHERE_RED;
        if (index == 1)
            _type = GameManager.ItemType.SPHERE_GREEN;
        if (index == 2)
            _type = GameManager.ItemType.SPHERE_BLUE;

        SetMaterial();
    }

    private void Update()
    {
        transform.name = baseName + " (" + Mathf.Round(transform.position.x) + "," + Mathf.Round(transform.position.y) + "," + Mathf.Round(transform.position.z) + ")";
        _globalPosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
    }

    private void OnMouseUpAsButton()
    {
        if (_globalPosition.z >= 0)
            return;
        if (TowerManager.Instance.TowerInRotation())
            return;
        if (TowerManager.Instance.LevelsInRotation())
            return;
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.RUNNING)
            return;

        if (matched == true)
            TowerManager.Instance.RemoveMatches();
        else
            HandleOnObjectSelected(transform.name, _type, _globalPosition);
    }

    private void HandleOnObjectSelected(string sender, GameManager.ItemType type, Vector3 gPosition)
    {

        TowerManager.Instance.ClearItemStates();

        matched = true;
        TowerManager.Instance.matchCount++;
        transform.GetChild(0).gameObject.SetActive(true);
        EventManager.Instance.OnObjectMatched.Invoke(transform.name, type, _globalPosition);

    }

    public void HandleOnObjectMatched(string sender, GameManager.ItemType type, Vector3 gPosition)
    {
        if (matched)
            return;
        if (Vector3.Distance(gPosition, _globalPosition) > 1.05)
            return;

        //Debug.Log(gPosition + " <-> " + _globalPosition + " = " + Vector3.Distance(gPosition, _globalPosition));

        if (_type == type)
        {
            matched = true;
            TowerManager.Instance.matchCount++;
            transform.GetChild(0).gameObject.SetActive(true);
            EventManager.Instance.OnObjectMatched.Invoke(transform.name, type, _globalPosition);
        }
    }

    private void SetMaterial()
    {
        GetComponent<Renderer>().material = GameManager.Instance._materials[(int)_type];
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnObjectMatched.RemoveListener(HandleOnObjectMatched);
    }
}
