using UnityEngine;
using UnityEngine.SceneManagement;

public class TowerBuilder : Singleton<TowerBuilder>
{
    public GameObject _itemHolderPrefab;
    public GameObject _itemPrefab;
    [SerializeField] private int _itemCount = 0;

    void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));
    }

    GameObject ConstructItem(int x, int y, int z)
    {
        GameObject itemParent;
        GameObject item;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));

        itemParent = Instantiate(_itemHolderPrefab);
        itemParent.transform.position = new Vector3(0, 0, 0);
        itemParent.transform.rotation = Quaternion.identity;

        // instantiate an item prefab
        item = Instantiate(_itemPrefab);
        item.name = "Item (" + x + "," + y + "," + z + ")";
        item.transform.position = new Vector3(0, 0, 0);
        item.transform.rotation = Quaternion.identity;
        item.AddComponent<NameSelf>();
        item.GetComponent<NameSelf>().baseName = "Item";
        item.GetComponent<Item>().fadeIn = _itemCount * 100 + 100;
        item.GetComponent<Item>()._type = (TowerManager.ItemType) ItemManager.Instance.GetItem();

        item.transform.SetParent(itemParent.transform);

        _itemCount++;
        return itemParent;
    }

    GameObject ConstructLevel(int xWidth, int level, int zWidth)
    {
        GameObject levelParent;
        GameObject item;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));

        levelParent = new GameObject("Level (0," + level + ",0)");
        levelParent.transform.position = new Vector3(0, 0, 0);
        levelParent.transform.rotation = Quaternion.identity;
        levelParent.AddComponent<NameSelf>();
        levelParent.GetComponent<NameSelf>().baseName = "Level";
        levelParent.AddComponent<Level>();

        for (int zIndex = -(int)(zWidth / 2); zIndex <= (int)(zWidth / 2); zIndex++)
        {
            for (int xIndex = -(int)(xWidth /2); xIndex <= (int)(xWidth /2); xIndex++)
            {
                item = ConstructItem(xIndex, level, zIndex);
                item.transform.position = new Vector3(xIndex, 0, zIndex);
                item.transform.SetParent(levelParent.transform);
                item.transform.name = "Item (" + xIndex + "," + level + "," + zIndex + ")";
            }

        }

         return levelParent;
    }

    public GameObject ConstructTower(int xWidth, int yHeight, int zDepth)
    {
        GameObject towerParent;
        GameObject level;

        SceneManager.SetActiveScene(SceneManager.GetSceneByName("Main"));

        towerParent = new GameObject("Tower");

        for (int index = 0; index < yHeight; index ++)
        {
            level = ConstructLevel(xWidth, index, zDepth);
            level.transform.position = new Vector3(0, index, 0);
            level.transform.SetParent(towerParent.transform);
        }

        return towerParent;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
