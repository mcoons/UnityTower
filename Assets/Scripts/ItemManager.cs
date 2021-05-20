using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dispenses the types of objects

public class ItemManager : Singleton<ItemManager>
{
    GrabBag _grabBag = new GrabBag();

    // Start is called before the first frame update
    void Start()
    {
        initBag();
    }

    public void initBag()
    {
        Random.InitState(GameManager.Instance.levelSeed);

        _grabBag.setMax(GameManager.Instance._masterTypeCount - 1);
        _grabBag.setDups((int)(TowerManager.Instance.towerLength *
            TowerManager.Instance.towerWidth *
            TowerManager.Instance.towerHeight / GameManager.Instance._masterTypeCount) + 1);
        _grabBag.fillBag();
        _grabBag.shakeBag();
    }

    public int GetItem()
    {
        return _grabBag.getRndNumber();
    }
}
