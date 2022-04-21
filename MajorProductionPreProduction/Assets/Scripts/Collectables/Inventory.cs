using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory: MonoBehaviour
{
    public List<ItemProgress> itemList = new List<ItemProgress>();

    void Start()
    {
        for (int i = 0; i < itemList.Count; i++)
        {
            //exp[i] = skillData[i].exp; 
            if (itemList.Count != 0)
            {
                itemList[i].AddItem();

            }
        }
    }
}
