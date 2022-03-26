using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public enum ItemType
    {
        collectable,
        health
    }

    public ItemType itemType;
    public int amount;
}
