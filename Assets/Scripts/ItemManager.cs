using UnityEngine;

// Dispenses the types of objects

// INHERITANCE
public class ItemManager : Singleton<ItemManager>
{
    GrabBag _grabBag = new GrabBag();

    // Start is called before the first frame update
    void Start()
    {
        //initBag();
    }

    public void initBag()
    {
        Random.InitState(GameManager.Instance._levelSeed);

        _grabBag.Max = GameManager.Instance._masterTypeCount - 1;
        _grabBag.Dups = (int)(TowerManager.Instance.towerLength *
            TowerManager.Instance.towerWidth *
            TowerManager.Instance.towerHeight / GameManager.Instance._masterTypeCount) + 1;
        _grabBag.fillBag();
        _grabBag.shakeBag();
    }

    public int GetItem()
    {
        return _grabBag.getRndNumber();
    }

    // POLYMORPHISM - overriding OnDestroy()
    protected override void OnDestroy()
    {
        base.OnDestroy();
    }
}
