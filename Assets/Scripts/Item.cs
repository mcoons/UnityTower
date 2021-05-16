using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField] public Vector3 _globalPosition;

    public string baseName;
    public GameManager.ItemType _type = GameManager.ItemType.SPHERE_RED;

    //public bool beenChecked = false;
    public bool matched = false;


    public bool isDropping = false;
    public float itemDropStartTime = 0;
    public Vector3 itemStartPosition;
    public Vector3 itemDesiredPosition;


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
        if (isDropping)
        {
            itemObjectLerpDrop();
        }

        transform.name = baseName + " (" + Mathf.Round(transform.position.x) + "," + Mathf.Round(transform.position.y) + "," + Mathf.Round(transform.position.z) + ")";
        _globalPosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
    }



    public void itemObjectLerpDrop()
    {
        //Debug.Log("In towerObjectLerpRotation");
        float dropTime = 4.0f;  // 4.0f equates to .25 seconds

        // Rotate the groupBlock over a period of rotationTime using Lerp
        transform.position = Vector3.Lerp(itemStartPosition, itemDesiredPosition, (Time.time - itemDropStartTime) * dropTime);

        // Once rotationTime has elapsed and the Lerp is done set inRotation to false
        if (Time.time - itemDropStartTime > 1 / dropTime)
        {
            isDropping = false;
            transform.position = itemDesiredPosition;

            TowerManager.Instance.getColumnIntersects();
            transform.SetParent(TowerManager.Instance._levelTransforms[(int)itemDesiredPosition.y]);
        }
        transform.name = baseName + " (" + Mathf.Round(transform.position.x) + "," + Mathf.Round(transform.position.y) + "," + Mathf.Round(transform.position.z) + ")";
        _globalPosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
    }



    private void OnMouseUpAsButton()
    {
        if (_globalPosition.z >= 0)
            return;
        if (TowerManager.Instance.TowerInRotation())
            return;
        if (TowerManager.Instance.areLevelsInRotation())
            return;
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.RUNNING)
            return;

        if (matched == true)
        {
            TowerManager.Instance.RemoveMatches();
            TowerManager.Instance.PrepareItemDrop();
        }
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
