using System.Collections;
using UnityEngine;

public class Item : MonoBehaviour
{

    [SerializeField] public Vector3 _globalPosition;

    public string baseName;
    public TowerManager.ItemType _type = TowerManager.ItemType.SPHERE_RED;

    public float fadeIn;  // pause time used to show tower building at start
    public bool selected = false;
    public bool matched = false;
    public bool isDropping = false;
    public float itemDropStartTime = 0;
    public Vector3 itemStartPosition;
    public Vector3 itemDesiredPosition;

    private IEnumerator fadeInCoroutine;

    public Animation _itemAnimator;
    public AnimationClip _explodeAnimation;

    public GameObject selector;

    // Start is called before the first frame update
    void Start()
    {
        _globalPosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));

        EventManager.Instance.OnObjectMatched.AddListener(HandleOnObjectMatched);

        SetMaterial();

        EventManager.Instance.OnObjectAdded.Invoke(_type, gameObject);

        fadeInCoroutine = WaitAndFadeIn(fadeIn/4500 + .25f);
        StartCoroutine(fadeInCoroutine);

    }

    private IEnumerator WaitAndFadeIn(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            FadeIn();
        }
    }

    private void Update()
    {
        if (isDropping && Time.time >= itemDropStartTime)
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


            //TODO send a message
            EventManager.Instance.OnObjectDropComplete.Invoke();

            //TowerManager.Instance.dropCount--;
            //if (TowerManager.Instance.dropCount == 0)
            //{
            //    TowerManager.Instance.CheckLoss();
            //}
            //TowerManager.Instance.getColumnIntersects();



            transform.SetParent(GameObject.Find("Holder (" + Mathf.Round(transform.position.x) + "," + Mathf.Round(transform.position.y) + "," + Mathf.Round(transform.position.z) + ")").transform);
            
            transform.name = baseName + " (" + Mathf.Round(transform.position.x) + "," + Mathf.Round(transform.position.y) + "," + Mathf.Round(transform.position.z) + ")";
            _globalPosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
        }
    }

    private void FadeIn()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = true;
    }

    private void OnMouseUpAsButton()
    {
        if (GameManager.Instance.CurrentGameState != GameManager.GameState.RUNNING)
            return;

        //TODO convert to dynamic sizable x,y,z
        // Only allow front row to be selected
        if (_globalPosition.z >= 0)
            return;

        // Do not allow during animations
        if (TowerManager.Instance.TowerInRotation())
            return;
        if (TowerManager.Instance.areLevelsInRotation())
            return;
        if (TowerManager.Instance.dropCount != 0)
            return;
        if (TowerManager.Instance.gameLost || TowerManager.Instance.gameWon)
            return;


        if (matched == true && TowerManager.Instance.matchCount > 1)
        {
            _itemAnimator.Stop();
            _itemAnimator.clip = _explodeAnimation;
            _itemAnimator.Play();

            AudioManager.Instance.Explode();
            //TODO send a message
            EventManager.Instance.OnObjectRemoved.Invoke();

            //TowerManager.Instance.RemoveMatches();
            //TowerManager.Instance.PrepareItemDrop("match");
            //if (TowerManager.Instance.dropCount == 0)
            //{
            //    TowerManager.Instance.CheckLoss();
            //}
            //TowerManager.Instance.CheckWin();

        }
        else
            HandleOnObjectSelected(transform.name, _type, _globalPosition);
    }

    private void HandleOnObjectSelected(string sender, TowerManager.ItemType type, Vector3 gPosition)
    {

        TowerManager.Instance.ClearItemStates();

        selected = true;
        matched = true;
        transform.GetChild(0).gameObject.SetActive(true);

        EventManager.Instance.OnObjectMatched.Invoke(transform.name, type, _globalPosition);

    }

    public void HandleOnObjectMatched(string sender, TowerManager.ItemType type, Vector3 gPosition)
    {
        if (matched)
            return;

        if (Vector3.Distance(gPosition, _globalPosition) > 1.05)
            return;

        //Debug.Log(gPosition + " <-> " + _globalPosition + " = " + Vector3.Distance(gPosition, _globalPosition));

        if (_type == type)
        {
            matched = true;
            //TowerManager.Instance.matchCount++;
            transform.GetChild(0).gameObject.SetActive(true);
            EventManager.Instance.OnObjectMatched.Invoke(transform.name, type, _globalPosition);
        }
    }

    private void SetMaterial()
    {
        GetComponent<Renderer>().material = TowerManager.Instance._itemMaterials[(int)_type];
        selector.GetComponent<Renderer>().material = TowerManager.Instance._selectorMaterials[(int)_type];
    }

    private void OnDestroy()
    {
        EventManager.Instance.OnObjectMatched.RemoveListener(HandleOnObjectMatched);
    }
}
