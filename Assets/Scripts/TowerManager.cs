using System;
using System.Collections;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;

public class TowerManager : Singleton<TowerManager>
{
    public GameObject TowerPrefab;
    [SerializeField] private GameObject _towerObject;
    [SerializeField] private Transform[] _levelTransforms = new Transform[6];

    //public Events.EventTowerAnimationStart OnTowerAnimationStart;
    //public Events.EventTowerAnimationComplete OnTowerAnimationComplete;



    public int dropCount = 0;				// Current count of objects dropping


    #region Level Rotation Variables

    [SerializeField]  bool[] levelInRotation = new bool[6];
    [SerializeField]  float[] levelRotationStartTime = new float[6];
    [SerializeField]  Quaternion[] levelStartRotation = new Quaternion[6];
    [SerializeField]  Quaternion[] levelDesiredRotation = new Quaternion[6];

    #endregion

    #region Tower Rotation Variables

    public bool towerInRotation = false;
    public float towerRotationStartTime = -1;
    public Quaternion towerStartRotation;
    public Quaternion towerDesiredRotation;

    #endregion

    public bool TowerInRotation() { return towerInRotation; }
    public bool LevelsInRotation() { return (levelInRotation[0] || levelInRotation[1] || levelInRotation[2] || levelInRotation[3] || levelInRotation[4] || levelInRotation[5]); }

    private static Timer timer1;
    // Start is called before the first frame update
    void Start()
    {
        _towerObject = Instantiate(TowerPrefab, this.transform);

        for (int i = 0; i < 6; i++)
        {
            _levelTransforms[i] = _towerObject.transform.GetChild(i);
        }


    }

    // Update is called once per frame
    void Update()
    {
        if (TowerInRotation())
        {
            //Debug.Log("Update: Tower is in rotation");
            towerObjectLerpRotation();
        }


        for (int i = 0; i < 6; i++)
        {
            if (levelInRotation[i])
            {
                levelObjectLerpRotation(i);
            }
        }
    }


    // Prepare Tower for rotation
    public void RotateTower(int direction)
    {
        //Debug.Log("In RotateTower");

        if (towerInRotation || dropCount > 0)
        {
            //Debug.Log("Returning!");
            return;
        }

        if (LevelsInRotation()) return;

        // Rotate the whole tower 
        towerStartRotation = new Quaternion(_towerObject.transform.rotation.x, _towerObject.transform.rotation.y, _towerObject.transform.rotation.z, _towerObject.transform.rotation.w);
        //Debug.Log("towerStartRotation: " + towerStartRotation);
        towerRotationStartTime = Time.time;
        //Debug.Log("towerRotationStartTime: " + towerRotationStartTime);

        towerInRotation = true;

        // Temporarily rotate and set the desired position
        _towerObject.transform.Rotate(0, -90 * direction, 0, Space.World);
        towerDesiredRotation = new Quaternion(_towerObject.transform.rotation.x, _towerObject.transform.rotation.y, _towerObject.transform.rotation.z, _towerObject.transform.rotation.w);
        //Debug.Log("towerDesiredRotation: " + towerDesiredRotation);

        _towerObject.transform.Rotate(0, 90 * direction, 0, Space.World);

        //AudioFile[(int)SoundQueueIndex.Rotate].Play();
        //Debug.Log("End of RotateTower");

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
            //Debug.Log("Tower Rotation Complete");
            towerInRotation = false;
            _towerObject.transform.rotation = towerDesiredRotation;
        }
    }



    //Prepare Level for rotation
    public void RotateLevel(int myLevel, int direction)
    {
        //Debug.Log("In RotateLevel");

        if (levelInRotation[myLevel] || towerInRotation || dropCount > 0) return;

        //neighborList.Clear();   // In rotate level right


        //markerList.ForEach(m => Destroy(m));
        //markerList.Clear();     // In rotate level right

        // Rotate the objects in that level clockwise/right
        levelStartRotation[myLevel] = new Quaternion(_levelTransforms[myLevel].rotation.x, _levelTransforms[myLevel].rotation.y, _levelTransforms[myLevel].rotation.z, _levelTransforms[myLevel].rotation.w);
        levelRotationStartTime[myLevel] = Time.time;
        levelInRotation[myLevel] = true;

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
        //Debug.Log("In levelObjectLerpRotation");

        float rotationTime = 4.0f;  // 4.0f; // This equates to .25 seconds

        // Rotate the groupBlock over a period of rotationTime using Lerp
        _levelTransforms[lvl].transform.rotation = Quaternion.Slerp(levelStartRotation[lvl], levelDesiredRotation[lvl], (Time.time - levelRotationStartTime[lvl]) * rotationTime);

        // Once rotationTime has elapsed and the Lerp is done set inRotation to false
        if (Time.time - levelRotationStartTime[lvl] > .25f)
        {
            levelInRotation[lvl] = false;
            _levelTransforms[lvl].rotation = levelDesiredRotation[lvl];
        }
    }


}
