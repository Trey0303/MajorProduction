using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemData", menuName = "CollectableObjects/ScriptableItem", order = 1)]
public class CollectableObj : ScriptableObject
{
    public string itemName;
    public string description;
    public GameObject itemObject;

    public virtual void display()
    {

    }
}
