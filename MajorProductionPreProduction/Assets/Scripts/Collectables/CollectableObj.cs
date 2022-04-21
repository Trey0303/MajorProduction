using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "ItemData", menuName = "CollectableObjects/ScriptableItem", order = 1)]
public class CollectableObj : ScriptableObject
{
    public string name;
    public string description;

    public virtual void display()
    {

    }
}
