using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    [SerializeField] public int towerLength = 3;
    [SerializeField] public int towerWidth = 3;
    [SerializeField] public int towerHeight = 6;

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


    //bool[] levelInRotation = new bool[towerHeight];
    //float[] levelRotationStartTime = new float[towerHeight];
    //Quaternion[] levelStartRotation = new Quaternion[towerHeight];
    //Quaternion[] levelDesiredRotation = new Quaternion[towerHeight];

    public int dropCount = 0;				// Current count of objects dropping
    public int matchCount = 0;

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

    private static Timer timer1;



    //private void Awake()
    //{
        
    //}

    //private void OnEnable()
    //{
        
    //}


    // Start is called before the first frame update
    void Start()
    {

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

        getColumnIntersects();

    }


    public void getColumnIntersects()
    {
        hitColliders0 = Physics.OverlapBox(new Vector3(-1, 0, -1), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        hitColliders1 = Physics.OverlapBox(new Vector3(0, 0, -1), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        hitColliders2 = Physics.OverlapBox(new Vector3(1, 0, -1), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        hitColliders3 = Physics.OverlapBox(new Vector3(-1, 0, 0), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        hitColliders4 = Physics.OverlapBox(new Vector3(0, 0, 0), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        hitColliders5 = Physics.OverlapBox(new Vector3(1, 0, 0), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        hitColliders6 = Physics.OverlapBox(new Vector3(-1, 0, 1), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        hitColliders7 = Physics.OverlapBox(new Vector3(0, 0, 1), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);
        hitColliders8 = Physics.OverlapBox(new Vector3(1, 0, 1), new Vector3(0.05f, 6f, 0.05f), Quaternion.identity);


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
        float rotationTime = 4.0f;  // 4.0f equates to .25 seconds

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

        // Rotate the matrix values of the level to match
        //AudioFile[(int)SoundQueueIndex.Rotate].Play();

    }

    // Rotate the level object over time
    void levelObjectLerpRotation(int lvl)
    {
        float rotationTime = 4.0f;  // 4.0f; // This equates to .25 seconds

        // Rotate the groupBlock over a period of rotationTime using Lerp
        _levelTransforms[lvl].transform.rotation = Quaternion.Slerp(levelStartRotation[lvl], levelDesiredRotation[lvl], (Time.time - levelRotationStartTime[lvl]) * rotationTime);

        // Once rotationTime has elapsed and the Lerp is done set inRotation to false
        if (Time.time - levelRotationStartTime[lvl] > .25f)
        {
            isLevelInRotation[lvl] = false;
            _levelTransforms[lvl].rotation = levelDesiredRotation[lvl];

            getColumnIntersects();
            PrepareItemDrop();
                
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
            go.GetComponent<Item>().matched = false;
            go.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    // Remove GameObjects with matched = true
    public void RemoveMatches()
    {
        GameObject[] items = GameObject.FindGameObjectsWithTag("Item");
        foreach (GameObject go in items)
            if (go.GetComponent<Item>().matched)
            {
                DestroyItem(go);
            }

    }

    public void PrepareItemDrop()
    {
        getColumnIntersects();

        int index = 0;
        foreach (Collider c in hitColliders0)
        {
            if (c.transform.GetComponent<Item>().matched)
                continue;

            if ((int)Mathf.Round(c.transform.position.y) == index)
            {
                index++;
                continue;
            }



            c.GetComponent<Item>().itemDropStartTime = Time.time;
            c.GetComponent<Item>().itemStartPosition = c.transform.position;
            c.GetComponent<Item>().itemDesiredPosition = new Vector3(c.transform.position.x, index, c.transform.position.z);
            c.GetComponent<Item>().isDropping = true;

            index++;
        }

        index = 0;
        foreach (Collider c in hitColliders1)
        {
            if (c.transform.GetComponent<Item>().matched)
                continue;

            if ((int)Mathf.Round(c.transform.position.y) == index)
            {
                index++;
                continue;
            }



            c.GetComponent<Item>().itemDropStartTime = Time.time;
            c.GetComponent<Item>().itemStartPosition = c.transform.position;
            c.GetComponent<Item>().itemDesiredPosition = new Vector3(c.transform.position.x, index, c.transform.position.z);
            c.GetComponent<Item>().isDropping = true;

            index++;
        }

        index = 0;
        foreach (Collider c in hitColliders2)
        {
            if (c.transform.GetComponent<Item>().matched)
                continue;

            if ((int)Mathf.Round(c.transform.position.y) == index)
            {
                index++;
                continue;
            }



            c.GetComponent<Item>().itemDropStartTime = Time.time;
            c.GetComponent<Item>().itemStartPosition = c.transform.position;
            c.GetComponent<Item>().itemDesiredPosition = new Vector3(c.transform.position.x, index, c.transform.position.z);
            c.GetComponent<Item>().isDropping = true;

            index++;
        }

        index = 0;
        foreach (Collider c in hitColliders3)
        {
            if (c.transform.GetComponent<Item>().matched)
                continue;

            if ((int)Mathf.Round(c.transform.position.y) == index)
            {
                index++;
                continue;
            }



            c.GetComponent<Item>().itemDropStartTime = Time.time;
            c.GetComponent<Item>().itemStartPosition = c.transform.position;
            c.GetComponent<Item>().itemDesiredPosition = new Vector3(c.transform.position.x, index, c.transform.position.z);
            c.GetComponent<Item>().isDropping = true;

            index++;
        }

        index = 0;
        foreach (Collider c in hitColliders4)
        {
            if (c.transform.GetComponent<Item>().matched)
                continue;

            if ((int)Mathf.Round(c.transform.position.y) == index)
            {
                index++;
                continue;
            }



            c.GetComponent<Item>().itemDropStartTime = Time.time;
            c.GetComponent<Item>().itemStartPosition = c.transform.position;
            c.GetComponent<Item>().itemDesiredPosition = new Vector3(c.transform.position.x, index, c.transform.position.z);
            c.GetComponent<Item>().isDropping = true;

            index++;
        }

        index = 0;
        foreach (Collider c in hitColliders5)
        {
            if (c.transform.GetComponent<Item>().matched)
                continue;

            if ((int)Mathf.Round(c.transform.position.y) == index)
            {
                index++;
                continue;
            }



            c.GetComponent<Item>().itemDropStartTime = Time.time;
            c.GetComponent<Item>().itemStartPosition = c.transform.position;
            c.GetComponent<Item>().itemDesiredPosition = new Vector3(c.transform.position.x, index, c.transform.position.z);
            c.GetComponent<Item>().isDropping = true;

            index++;
        }

        index = 0;
        foreach (Collider c in hitColliders6)
        {
            if (c.transform.GetComponent<Item>().matched)
                continue;

            if ((int)Mathf.Round(c.transform.position.y) == index)
            {
                index++;
                continue;
            }



            c.GetComponent<Item>().itemDropStartTime = Time.time;
            c.GetComponent<Item>().itemStartPosition = c.transform.position;
            c.GetComponent<Item>().itemDesiredPosition = new Vector3(c.transform.position.x, index, c.transform.position.z);
            c.GetComponent<Item>().isDropping = true;

            index++;
        }

        index = 0;
        foreach (Collider c in hitColliders7)
        {
            if (c.transform.GetComponent<Item>().matched)
                continue;

            if ((int)Mathf.Round(c.transform.position.y) == index)
            {
                index++;
                continue;
            }



            c.GetComponent<Item>().itemDropStartTime = Time.time;
            c.GetComponent<Item>().itemStartPosition = c.transform.position;
            c.GetComponent<Item>().itemDesiredPosition = new Vector3(c.transform.position.x, index, c.transform.position.z);
            c.GetComponent<Item>().isDropping = true;

            index++;
        }

        index = 0;
        foreach (Collider c in hitColliders8)
        {
            if (c.transform.GetComponent<Item>().matched) 
                continue;

            if ((int)Mathf.Round(c.transform.position.y) == index)
            {
                index++;
                continue;
            }


            c.GetComponent<Item>().itemDropStartTime = Time.time;
            c.GetComponent<Item>().itemStartPosition = c.transform.position;
            c.GetComponent<Item>().itemDesiredPosition = new Vector3(c.transform.position.x, index, c.transform.position.z);
            c.GetComponent<Item>().isDropping = true;

            index++;
        }




        //for (int i = 0; i < hitColliders0.Length; i++)
        //{
        //    if ((int)Mathf.Round(hitColliders0[i].transform.position.y) == i)
        //    {
        //        continue;
        //    }

        //    hitColliders0[i].GetComponent<Item>().itemDropStartTime = Time.time;
        //    hitColliders0[i].GetComponent<Item>().itemStartPosition = hitColliders0[i].transform.position;
        //    hitColliders0[i].GetComponent<Item>().itemDesiredPosition = new Vector3(hitColliders0[i].transform.position.x, i, hitColliders0[i].transform.position.z);
        //    hitColliders0[i].GetComponent<Item>().isDropping = true;
        //}

        //for (int i = 0; i < hitColliders1.Length; i++)
        //{
        //    //if ((int)Mathf.Round( hitColliders0[i].transform.position.y) == i)
        //    //{
        //    //    continue;
        //    //}

        //    hitColliders1[i].GetComponent<Item>().itemDropStartTime = Time.time;
        //    hitColliders1[i].GetComponent<Item>().itemStartPosition = hitColliders1[i].transform.position;
        //    hitColliders1[i].GetComponent<Item>().itemDesiredPosition = new Vector3(hitColliders1[i].transform.position.x, i, hitColliders1[i].transform.position.z);
        //    hitColliders1[i].GetComponent<Item>().isDropping = true;
        //}

        //for (int i = 0; i < hitColliders2.Length; i++)
        //{
        //    //if ((int)Mathf.Round( hitColliders0[i].transform.position.y) == i)
        //    //{
        //    //    continue;
        //    //}

        //    hitColliders2[i].GetComponent<Item>().itemDropStartTime = Time.time;
        //    hitColliders2[i].GetComponent<Item>().itemStartPosition = hitColliders2[i].transform.position;
        //    hitColliders2[i].GetComponent<Item>().itemDesiredPosition = new Vector3(hitColliders2[i].transform.position.x, i, hitColliders2[i].transform.position.z);
        //    hitColliders2[i].GetComponent<Item>().isDropping = true;
        //}
    }

    public void DestroyItem(GameObject go)
    {
        //int x = (int)Mathf.Round(go.GetComponent<Item>()._globalPosition.x);
        //int y = (int)Mathf.Round(go.GetComponent<Item>()._globalPosition.y);
        //int z = (int)Mathf.Round(go.GetComponent<Item>()._globalPosition.z);

        ////_columns[(x.ToString() + ",0," + z.ToString())].Remove(go.transform);

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
