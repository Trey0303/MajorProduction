using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemProgress
{
    public CollectableObj itemData;

    public string name;

    public string description;

    public void AddItem()
    {
        name = itemData.name;
        description = itemData.description;
    }

    public void AddItem(CollectableObj newitem)
    {
        name = newitem.name;
        description = newitem.description;
    }
}
