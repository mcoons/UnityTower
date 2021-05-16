using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerBuilder : Singleton<TowerBuilder>
{

    //GameObject _tower;
    //GameObject _level;
    //GameObject _itemHolder;
    public GameObject _itemPrefab;
    //GameObject _selector;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
    }

    GameObject ConstructItem(int x, int y, int z)
    {
        GameObject _itemParent;
        GameObject _item;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));


        // create an item parent (Holder)
        _itemParent = new GameObject("Holder (" + x + "," + y +"," + z + ")");
        _itemParent.AddComponent<NameSelf>();
        _itemParent.GetComponent<NameSelf>().baseName = "Holder";
        _itemParent.transform.position = new Vector3(0, 0, 0);
        _itemParent.transform.rotation = Quaternion.identity;

        // instantiate an item prefab
        _item = Instantiate(_itemPrefab);
        _item.AddComponent<NameSelf>();
        _item.GetComponent<NameSelf>().baseName = "Item";
        _item.transform.position = new Vector3(0, 0, 0);
        _item.transform.rotation = Quaternion.identity;
        _item.name = "Item (" + x + "," + y + "," + z + ")";

        // add to item parent
        _item.transform.SetParent(_itemParent.transform);


        return _itemParent;
    }


    GameObject ConstructLevel(int xWidth, int level, int zWidth)
    {
        GameObject _levelParent;
        GameObject _item;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));

        // create a level parent
        _levelParent = new GameObject("Level (0," + level + ",0)");
        _levelParent.AddComponent<NameSelf>();
        _levelParent.GetComponent<NameSelf>().baseName = "Level";
        _levelParent.AddComponent<Level>();
        _levelParent.transform.position = new Vector3(0, 0, 0);
        _levelParent.transform.rotation = Quaternion.identity;


        for (int xIndex = -(int)(xWidth /2); xIndex <= (int)(xWidth /2); xIndex++)
        {
            for (int zIndex = -(int)(zWidth / 2); zIndex <= (int)(zWidth / 2); zIndex++)
            {
                // construct an item
                _item = ConstructItem(xIndex, level, zIndex);

                // move to position
                _item.transform.position = new Vector3(xIndex, 0, zIndex);

                // add to level parent
                _item.transform.SetParent(_levelParent.transform);

                //_item.GetComponent<Item>()._globalPosition = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z));
                _item.transform.name = "Item (" + xIndex + "," + level + "," + zIndex + ")";


            }

        }

         return _levelParent;
    }

    public GameObject ConstructTower(int xWidth, int yHeight, int zDepth)
    {
        GameObject _towerParent;
        GameObject _level;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));

        // create a tower parent
        _towerParent = new GameObject("Tower");


        for (int index = 0; index < yHeight; index ++)
        {
            // construct a level
            _level = ConstructLevel(xWidth, index, zDepth);

            // move to position
            _level.transform.position = new Vector3(0, index, 0);

            // add to tower parent
            _level.transform.SetParent(_towerParent.transform);

        }

        return _towerParent;

    }



}
