using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    [SerializeField] public int towerLength = 3;
    [SerializeField] public int towerWidth = 3;
    [SerializeField] public int towerHeight = 6;
    //[SerializeField] public int gemTypeCount;
    public bool gameLost = false;
    public bool gameWon = false;

    public enum ItemType
    {
        SPHERE_RED,
        SPHERE_GREEN,
        SPHERE_BLUE,
        SPHERE_ORANGE,
        SPHERE_YELLOW,
        SPHERE_INDIGO,
        SPHERE_VIOLET
    }


    [SerializeField] public List<int> typeCounts = new List<int>();
    [SerializeField] public List<List<GameObject>> typeGroups = new List<List<GameObject>>();

    public Material[] _itemMaterials;
    public Material[] _selectorMaterials;

    //public GameObject TowerPrefab;
    [SerializeField] private GameObject _towerObject;
    //[SerializeField] private Transform[] _levelTransforms = new Transform[towerHeight];
    public List<Transform> _levelTransforms = new List<Transform>();


    //TODO convert to dynamic sizable x,y,z
    [SerializeField] Collider[] hitColliders0;
    [SerializeField] Collider[] hitColliders1;
    [SerializeField] Collider[] hitColliders2;
    [SerializeField] Collider[] hitColliders3;
    [SerializeField] Collider[] hitColliders4;
    [SerializeField] Collider[] hitColliders5;
    [SerializeField] Collider[] hitColliders6;
    [SerializeField] Collider[] hitColliders7;
    [SerializeField] Collider[] hitColliders8;


    #region Level Rotation Variables

    public List<bool> isLevelInRotation = new List<bool>();
    List<float> levelRotationStartTime = new List<float>();
    List<Quaternion> levelStartRotation = new List<Quaternion>();
    List<Quaternion> levelDesiredRotation = new List<Quaternion>();

    public int dropCount = 0;				// Current count of objects dropping
    public int matchCount = 0;

    private IEnumerator delayDestroyCoroutine;

    #endregion

    #region Tower Rotation Variables

    bool isTowerInRotation = false;
    float towerRotationStartTime = -1;
    Quaternion towerStartRotation;
    Quaternion towerDesiredRotation;

    #endregion

    public bool TowerInRotation() { return isTowerInRotation; }

    //TODO convert to dynamic sizable x,y,z
    public bool areLevelsInRotation() { return (isLevelInRotation[0] || isLevelInRotation[1] || isLevelInRotation[2] || isLevelInRotation[3] || isLevelInRotation[4] || isLevelInRotation[5]); }
    public bool isDropping() { return (dropCount > 0); }

    public int getItemCount()
    {
        int count = 0;

        foreach (int typeCount in typeCounts)
        {
            count += typeCount;
        }


        return count;
    }

    //private void Awake()
    //{
        
    //}

    //private void OnEnable()
    //{
        
    //}


    // Start is called before the first frame update
    void Start()
    {
        ItemManager.Instance.initBag();
        _towerObject = TowerBuilder.Instance.ConstructTower(towerLength, towerHeight, towerWidth);

        //Debug.Break();


        for (int i = 0; i < towerHeight; i++)
        {
            _levelTransforms.Add(_towerObject.transform.GetChild(i));

            isLevelInRotation.Add(false);
            levelRotationStartTime.Add(-1);
            levelStartRotation.Add(Quaternion.identity);
            levelDesiredRotation.Add(Quaternion.identity);

            //Debug.Log("-------------  Printing Level " + i);
            //PrintLevel(i);
        }


        UpdateGemTypeCount(GameManager.Instance._masterTypeCount);

    }

    public void UpdateGemTypeCount(int newCount)
    {
        GameManager.Instance._masterTypeCount = newCount;
        typeCounts.Clear();
        typeGroups.Clear();
        for (int i = 0; i < GameManager.Instance._masterTypeCount; i++)
        {
            typeCounts.Add(0);
            typeGroups.Add(new List<GameObject> { });
        }
    }

    public void getColumnIntersects()
    {
        //TODO convert to dynamic sizable x,y,z
        hitColliders0 = Physics.OverlapBox(new Vector3(-1, 0, -1), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        hitColliders1 = Physics.OverlapBox(new Vector3(0, 0, -1), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        hitColliders2 = Physics.OverlapBox(new Vector3(1, 0, -1), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        hitColliders3 = Physics.OverlapBox(new Vector3(-1, 0, 0), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        hitColliders4 = Physics.OverlapBox(new Vector3(0, 0, 0), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        hitColliders5 = Physics.OverlapBox(new Vector3(1, 0, 0), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        hitColliders6 = Physics.OverlapBox(new Vector3(-1, 0, 1), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        hitColliders7 = Physics.OverlapBox(new Vector3(0, 0, 1), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        hitColliders8 = Physics.OverlapBox(new Vector3(1, 0, 1), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);


        //TODO convert to dynamic sizable x,y,z - loop this

        Array.Sort(hitColliders0, delegate (Collider c1, Collider c2) {
            return c1.transform.position.y.CompareTo(c2.transform.position.y); 
        });

        Array.Sort(hitColliders1, delegate (Collider c1, Collider c2) {
            return c1.transform.position.y.CompareTo(c2.transform.position.y); 
        });

        Array.Sort(hitColliders2, delegate (Collider c1, Collider c2) {
            return c1.transform.position.y.CompareTo(c2.transform.position.y); 
        });

        Array.Sort(hitColliders3, delegate (Collider c1, Collider c2) {
            return c1.transform.position.y.CompareTo(c2.transform.position.y); 
        });

        Array.Sort(hitColliders4, delegate (Collider c1, Collider c2) {
            return c1.transform.position.y.CompareTo(c2.transform.position.y); 
        });

        Array.Sort(hitColliders5, delegate (Collider c1, Collider c2) {
            return c1.transform.position.y.CompareTo(c2.transform.position.y); 
        });

        Array.Sort(hitColliders6, delegate (Collider c1, Collider c2) {
            return c1.transform.position.y.CompareTo(c2.transform.position.y); 
        });

        Array.Sort(hitColliders7, delegate (Collider c1, Collider c2) {
            return c1.transform.position.y.CompareTo(c2.transform.position.y); 
        });

        Array.Sort(hitColliders8, delegate (Collider c1, Collider c2) {
            return c1.transform.position.y.CompareTo(c2.transform.position.y); 
        });

    }

    // Update is called once per frame
    void Update()
    {
        if (TowerInRotation())
            towerObjectLerpRotation();
        else
            if (areLevelsInRotation())
                for (int i = 0; i < towerHeight; i++)
                {
                    if (isLevelInRotation[i])
                        levelObjectLerpRotation(i);
                }


        getColumnIntersects();

    }

    public void ChangeTypeCount(ItemType type, int count)
    {
        typeCounts[(int)type] += count;
    }

    public void AddToTypeGroup(ItemType type, GameObject item)
    {
        typeGroups[(int)type].Add(item);
    }

    public void RemoveFromTypeGroup(ItemType type, GameObject item)
    {
        typeGroups[(int)type].Remove(item);
    }



    public bool CheckLoss()
    {
        Debug.Log("In CheckEndGame");
        for (int i = 0; i < GameManager.Instance._masterTypeCount; i++)
        {
            // Any single Type alone

            if (typeCounts[i] == 1) gameLost = true;

            // all in the center column

            // all in the center column and corner columns

            // all on bottom and not neighbors

             
        }

        if (gameLost) EventManager.Instance.OnGameLoss.Invoke();
        return gameLost;

    }

    public bool CheckWin()
    {
        if (getItemCount() == 0) gameWon = true;

        if (gameWon) EventManager.Instance.OnGameWin.Invoke();

        return gameWon;
    }

    /**********************************/
    /*      Tower/Level Methods       */
    /**********************************/

    // Prepare Tower for rotation
    public void RotateTower(int direction)
    {
        //Debug.Log("In RotateTower");

        if (isTowerInRotation || dropCount > 0)
            return;
        if (areLevelsInRotation())
            return;

        // Rotate the whole tower 
        towerStartRotation = new Quaternion(_towerObject.transform.rotation.x, _towerObject.transform.rotation.y, _towerObject.transform.rotation.z, _towerObject.transform.rotation.w);
        towerRotationStartTime = Time.time;

        isTowerInRotation = true;

        // Temporarily rotate and set the desired position
        _towerObject.transform.Rotate(0, -90 * direction, 0, Space.World);
        towerDesiredRotation = new Quaternion(_towerObject.transform.rotation.x, _towerObject.transform.rotation.y, _towerObject.transform.rotation.z, _towerObject.transform.rotation.w);

        _towerObject.transform.Rotate(0, 90 * direction, 0, Space.World);

        //AudioFile[(int)SoundQueueIndex.Rotate].Play();

    }

    // Rotate the tower object over time
    void towerObjectLerpRotation()
    {
        //Debug.Log("In towerObjectLerpRotation");
        float rotationTime = 2.0f;  // 4.0f equates to .25 seconds

        // Rotate the groupBlock over a period of rotationTime using Lerp
        _towerObject.transform.rotation = Quaternion.Slerp(towerStartRotation, towerDesiredRotation, (Time.time - towerRotationStartTime) * rotationTime);

        // Once rotationTime has elapsed and the Lerp is done set inRotation to false
        if (Time.time - towerRotationStartTime > 1 / rotationTime)
        {
            isTowerInRotation = false;
            _towerObject.transform.rotation = towerDesiredRotation;

            getColumnIntersects();
        }
    }

    //Prepare Level for rotation
    public void RotateLevel(int myLevel, int direction)
    {
        if (isLevelInRotation[myLevel] || isTowerInRotation || dropCount > 0)
            return;

        TowerManager.Instance.ClearItemStates();

        // Rotate the objects in that level clockwise/right
        levelStartRotation[myLevel] = new Quaternion(_levelTransforms[myLevel].rotation.x, _levelTransforms[myLevel].rotation.y, _levelTransforms[myLevel].rotation.z, _levelTransforms[myLevel].rotation.w);
        levelRotationStartTime[myLevel] = Time.time;
        isLevelInRotation[myLevel] = true;

        // Temporarily rotate to the desired position
        _levelTransforms[myLevel].Rotate(0, -90 * direction, 0, Space.World);

        // Set the desired rotation for lerping
        levelDesiredRotation[myLevel] = new Quaternion(_levelTransforms[myLevel].rotation.x, _levelTransforms[myLevel].rotation.y, _levelTransforms[myLevel].rotation.z, _levelTransforms[myLevel].rotation.w);

        // Undo the previous rotation
        _levelTransforms[myLevel].Rotate(0, 90 * direction, 0, Space.World);

        //AudioFile[(int)SoundQueueIndex.Rotate].Play();
    }

    // Rotate the level object over time
    void levelObjectLerpRotation(int lvl)
    {
        float rotationTime = 2.0f;  // 4.0f; // This equates to .25 seconds

        // Rotate the groupBlock over a period of rotationTime using Lerp
        _levelTransforms[lvl].transform.rotation = Quaternion.Slerp(levelStartRotation[lvl], levelDesiredRotation[lvl], (Time.time - levelRotationStartTime[lvl]) * rotationTime);

        // Once rotationTime has elapsed and the Lerp is done set inRotation to false
        if (Time.time - levelRotationStartTime[lvl] > 1/rotationTime)
        {
            isLevelInRotation[lvl] = false;
            _levelTransforms[lvl].rotation = levelDesiredRotation[lvl];

            PrepareItemDrop("rotate");   
        }
    }

    /**********************************/
    /*          Item Methods          */
    /**********************************/

    // Clears matched flag and Active child Selector
    public void ClearItemStates()
    {
        matchCount = 0;

        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject go in items)
        {
            go.GetComponent<Item>().selected = false;
            go.GetComponent<Item>().matched = false;
            go.transform.GetChild(0).gameObject.SetActive(false);
        }
    }



    public void PrepareItemDrop(string from)
    {
        getColumnIntersects();

        //TODO convert to dynamic sizable x,y,z - combine these into the same loop
        PrepareColumnDrop(hitColliders0, from);
        PrepareColumnDrop(hitColliders1, from);
        PrepareColumnDrop(hitColliders2, from);
        PrepareColumnDrop(hitColliders3, from);
        PrepareColumnDrop(hitColliders4, from);
        PrepareColumnDrop(hitColliders5, from);
        PrepareColumnDrop(hitColliders6, from);
        PrepareColumnDrop(hitColliders7, from);
        PrepareColumnDrop(hitColliders8, from);
    }

    public void PrepareColumnDrop(Collider[] hitColliders, string from)
    {
        int index = 0;
        foreach (Collider c in hitColliders)
        {
            if (c.transform.GetComponent<Item>().matched)
                continue;

            if ((int)Mathf.Round(c.transform.position.y) == index)
            {
                index++;
                continue;
            }

            c.GetComponent<Item>().itemDropStartTime = Time.time + (from == "match" ? 1 : 0);
            c.GetComponent<Item>().itemStartPosition = c.transform.position;
            c.GetComponent<Item>().itemDesiredPosition = new Vector3(c.transform.position.x, index, c.transform.position.z);
            c.GetComponent<Item>().isDropping = true;
            dropCount++;

            index++;
        }
    }

    // Remove GameObjects with matched = true
    public void RemoveMatches()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject go in items)
            if (go.GetComponent<Item>().matched)
            {
                RemoveFromTypeGroup(go.GetComponent<Item>()._type, go);
                ChangeTypeCount(go.GetComponent<Item>()._type, -1);

                delayDestroyCoroutine = WaitAndDestroy(go);
                StartCoroutine(delayDestroyCoroutine);
                go.GetComponent<Item>()._itemAnimator.Stop();
                go.GetComponent<Item>()._itemAnimator.clip = go.GetComponent<Item>()._explodeAnimation;
                go.GetComponent<Item>()._itemAnimator.Play();
            }

    }

    private IEnumerator WaitAndDestroy(GameObject go)
    {
        while (true)
        {
            yield return new WaitForSeconds(1);
            DestroyItem(go);

        }
    }

    public void DestroyItem(GameObject go)
    {
        Destroy(go);
    }

    public void PrintLevel(int level)
    {
        foreach (Transform child in _towerObject.transform.GetChild(level))
        {
            Debug.Log(child.name);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        _levelTransforms.Clear();
        Destroy(_towerObject);
    }

}
