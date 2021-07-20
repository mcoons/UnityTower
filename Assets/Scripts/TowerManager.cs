using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    [SerializeField] public int towerLength = 3;
    [SerializeField] public int towerWidth  = 3;
    [SerializeField] public int towerHeight = 6;

    [SerializeField] public GameObject blinkPrefab;

    public bool gameLost = false;
    public string lossMsg = "";
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

    public List<Color> ItemColors = new List<Color>();


    [SerializeField] public List<int> typeCounts = new List<int>();
    [SerializeField] public List<List<GameObject>> typeGroups = new List<List<GameObject>>();

    public Material[] _itemMaterials;
    public Material[] _selectorMaterials;
    public int dropCount = 0;				// Current count of objects dropping
    public int matchCount = 0;

    //TODO convert to dynamic sizable x,y,z
    //[SerializeField] public List<Collider[]> hitColliders = new List<Collider[]>();

    //[SerializeField] public List<Collider[]> columnItems = new List<Collider[]>();

    [SerializeField] Collider[] hitColliders0;
    [SerializeField] Collider[] hitColliders1;
    [SerializeField] Collider[] hitColliders2;
    [SerializeField] Collider[] hitColliders3;
    [SerializeField] Collider[] hitColliders4;
    [SerializeField] Collider[] hitColliders5;
    [SerializeField] Collider[] hitColliders6;
    [SerializeField] Collider[] hitColliders7;
    [SerializeField] Collider[] hitColliders8;


    [SerializeField] private GameObject _towerObject;
    public List<Transform> _levelTransforms = new List<Transform>();

    #region Rotation Variables

    public List<bool> isLevelInRotation = new List<bool>();
    List<float> levelRotationStartTime = new List<float>();
    List<Quaternion> levelStartRotation = new List<Quaternion>();
    List<Quaternion> levelDesiredRotation = new List<Quaternion>();



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

    void Start()
    {
        EventManager.Instance.OnObjectAdded.AddListener(HandleOnObjectAdded);
        EventManager.Instance.OnObjectMatched.AddListener(HandleOnObjectMatched);
        EventManager.Instance.OnObjectRemoved.AddListener(HandleOnObjectRemoved);
        EventManager.Instance.OnObjectSelected.AddListener(HandleOnObjectSelected);
        EventManager.Instance.OnObjectDropComplete.AddListener(HandleObjectDropComplete);

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


        //columnItems.Clear();
        //int index = 0;
        //for (int zIndex = -1; zIndex <= 1; zIndex++)
        //{
        //    for (int xIndex = -1; xIndex <= 1; xIndex++)
        //    {

        //        Collider[] c = Physics.OverlapBox(new Vector3(xIndex, 0, zIndex), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        //        Array.Sort(c, delegate (Collider c1, Collider c2)
        //        {
        //            return c1.transform.position.y.CompareTo(c2.transform.position.y);
        //        });
        //        columnItems.Add(c);
        //        index++;
        //    }

        //}


        UpdateGemTypeCount(GameManager.Instance._masterTypeCount);

    }

    public void HandleObjectDropComplete()
    {
        dropCount--;
        if (dropCount == 0)
        {
            CheckLoss();
        }
        getColumnIntersects();
    }

    public void HandleOnObjectMatched(string sender, TowerManager.ItemType type, Vector3 gPosition)
    {
        TowerManager.Instance.matchCount++;
    }

    private void HandleOnObjectAdded(ItemType type, GameObject go)
    {
        ChangeTypeCount(type, 1);
        AddToTypeGroup(type, go);
    }

    private void HandleOnObjectRemoved()
    {
        CalculateScore();
        RemoveMatches();
        PrepareItemDrop("match");
        if (dropCount == 0)
        {
            CheckLoss();
        }
        CheckWin();
    }

    private void HandleOnObjectSelected(string sender, TowerManager.ItemType type, Vector3 gPosition)
    {

    }

    public void CalculateScore()
    {
        GameManager.Instance.levelScore += TowerManager.Instance.matchCount * TowerManager.Instance.matchCount;
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

        //columnItems.Clear();
        //int index = 0;
        //for (int zIndex = -1; zIndex <= 1; zIndex++)
        //{
        //    for (int xIndex = -1; xIndex <= 1; xIndex++)
        //    {

        //        Collider[] c = Physics.OverlapBox(new Vector3(xIndex, 0, zIndex), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        //        Array.Sort(c, delegate (Collider c1, Collider c2)
        //        {
        //            return c1.transform.position.y.CompareTo(c2.transform.position.y);
        //        });
        //        columnItems.Add(c);
        //        index++;
        //    }

        //}



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

        Array.Sort(hitColliders0, delegate (Collider c1, Collider c2)
        {
            return c1.transform.position.y.CompareTo(c2.transform.position.y);
        });

        Array.Sort(hitColliders1, delegate (Collider c1, Collider c2)
        {
            return c1.transform.position.y.CompareTo(c2.transform.position.y);
        });

        Array.Sort(hitColliders2, delegate (Collider c1, Collider c2)
        {
            return c1.transform.position.y.CompareTo(c2.transform.position.y);
        });

        Array.Sort(hitColliders3, delegate (Collider c1, Collider c2)
        {
            return c1.transform.position.y.CompareTo(c2.transform.position.y);
        });

        Array.Sort(hitColliders4, delegate (Collider c1, Collider c2)
        {
            return c1.transform.position.y.CompareTo(c2.transform.position.y);
        });

        Array.Sort(hitColliders5, delegate (Collider c1, Collider c2)
        {
            return c1.transform.position.y.CompareTo(c2.transform.position.y);
        });

        Array.Sort(hitColliders6, delegate (Collider c1, Collider c2)
        {
            return c1.transform.position.y.CompareTo(c2.transform.position.y);
        });

        Array.Sort(hitColliders7, delegate (Collider c1, Collider c2)
        {
            return c1.transform.position.y.CompareTo(c2.transform.position.y);
        });

        Array.Sort(hitColliders8, delegate (Collider c1, Collider c2)
        {
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



    public bool CheckWin()
    {
        if (getItemCount() == 0) gameWon = true;

        if (gameWon)
        {
            EventManager.Instance.OnGameWin.Invoke();
            AudioManager.Instance.Win();
        }

        return gameWon;
    }


    public bool CheckLoss()
    {
        gameLost = false;
        lossMsg = "";
        Debug.Log("---------------------------");
        Debug.Log("In CheckEndGame");
        for (int i = 0; i < GameManager.Instance._masterTypeCount; i++)
        {
            // Any single Type alone
            Debug.Log("IsLoneSurvivor((TowerManager.ItemType)i): " + IsLoneSurvivor((TowerManager.ItemType)i).ToString());
            gameLost = gameLost || IsLoneSurvivor((TowerManager.ItemType)i);

            // all in the center column
            Debug.Log("AreAllCenter((TowerManager.ItemType)i): " + AreAllCenter((TowerManager.ItemType)i).ToString());
            gameLost = gameLost || AreAllCenter((TowerManager.ItemType)i);

            // all in the center column and corner columns
            //Debug.Log("AreAllCenterCorner((TowerManager.ItemType)i): " + AreAllCenterCorner((TowerManager.ItemType)i).ToString());
            //gameLost = gameLost || AreAllCenterCorner((TowerManager.ItemType)i);

            // all on bottom and not neighbors


        }


        if (gameLost)
        {
            Debug.Log(lossMsg);
            EventManager.Instance.OnGameLoss.Invoke();
            AudioManager.Instance.Loss();
        }


        return gameLost;

    }


    bool IsLoneSurvivor(TowerManager.ItemType itemType)
    {
        if (typeCounts[(int)itemType] == 1)
        {
            lossMsg = "There is only one " + itemType.ToString();
            return true;
        }

        return false;
    }

    bool AreAllCenter(TowerManager.ItemType itemType)
    {
        getColumnIntersects();

        if (typeGroups[(int)itemType].Count == 0)
            return false;

        foreach (GameObject go in typeGroups[(int)itemType])
            if (!(go.GetComponent<Item>()._globalPosition.x == 0 && go.GetComponent<Item>()._globalPosition.z == 0))

                //if (!(Mathf.Round(go.transform.position.x) == 0 && Mathf.Round(go.transform.position.z) == 0))
                return false;

        lossMsg = itemType.ToString() + " are all in the center";

        return true;
    }

    bool AreAllCenterCorner(TowerManager.ItemType itemType)
    {
        if (typeGroups[(int)itemType].Count == 0)
            return false;

        foreach (var go in typeGroups[(int)itemType])
        {
            Debug.Log(go.name);
            if (Mathf.Abs(go.GetComponent<Item>()._globalPosition.x) + Mathf.Abs(go.GetComponent<Item>()._globalPosition.z) == 1)
                return false;
        }

        lossMsg = itemType.ToString() + " are all in the center and corners";

        return true;
    }

    bool AreAllLonelyBottomDwellers(TowerManager.ItemType itemType)
    {
        foreach (var item in typeGroups[(int)itemType])
            if (item.transform.position.y > 0)
                return false;

        return true;
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

        AudioManager.Instance.Rotate();

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

        AudioManager.Instance.Rotate();
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

        //int index = 0;
        //for (int zIndex = -(int)(towerWidth / 2); zIndex <= (int)(towerWidth / 2); zIndex++)
        //{
        //    for (int xIndex = -(int)(towerLength / 2); xIndex <= (int)(towerLength / 2); xIndex++)
        //    {
        //        PrepareColumnDrop(columnItems[index], from);
        //    }
        //}

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
        matchCount = 0;
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

        EventManager.Instance.OnObjectAdded.RemoveListener(HandleOnObjectAdded);
        EventManager.Instance.OnObjectMatched.RemoveListener(HandleOnObjectMatched);
        EventManager.Instance.OnObjectRemoved.RemoveListener(HandleOnObjectRemoved);
        EventManager.Instance.OnObjectSelected.RemoveListener(HandleOnObjectSelected);
        EventManager.Instance.OnObjectDropComplete.RemoveListener(HandleObjectDropComplete);

        _levelTransforms.Clear();
        Destroy(_towerObject);

        base.OnDestroy();
    }

}
