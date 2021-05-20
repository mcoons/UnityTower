using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerBuilder : Singleton<TowerBuilder>
{
    public GameObject _itemHolderPrefab;
    public GameObject _itemPrefab;
    public int _itemCount = 0;



    // Start is called before the first frame update
    void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
    }

    GameObject ConstructItem(int x, int y, int z)
    {
        GameObject itemParent;
        GameObject item;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));

        // create an item parent (Holder)
        itemParent = Instantiate(_itemHolderPrefab);
        itemParent.transform.position = new Vector3(0, 0, 0);
        itemParent.transform.rotation = Quaternion.identity;

        // instantiate an item prefab
        item = Instantiate(_itemPrefab);
        item.AddComponent<NameSelf>();
        item.GetComponent<NameSelf>().baseName = "Item";
        item.transform.position = new Vector3(0, 0, 0);
        item.transform.rotation = Quaternion.identity;
        item.name = "Item (" + x + "," + y + "," + z + ")";
        item.GetComponent<Item>().fadeIn = _itemCount * 100 + 100;
        item.GetComponent<Item>()._type = (TowerManager.ItemType) ItemManager.Instance.GetItem();

        // add to item parent
        item.transform.SetParent(itemParent.transform);

        _itemCount++;
        return itemParent;
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
