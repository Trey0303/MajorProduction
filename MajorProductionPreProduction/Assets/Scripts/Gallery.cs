using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Gallery : MonoBehaviour
{
    //[Header("Buttons")]
    //private Button OpenGallery;
    //private Button ExitGallery;

    private GameObject gallery;

    public Text description;

    private GameObject displayItem;

    public List<Button> itemButtons;

    private static GameObject selectedButton;

    public Transform targetPos;

    // Start is called before the first frame update
    void Start()
    {
        gallery = GameObject.Find("IndexGallery");
        gallery.SetActive(false);

        targetPos = GameObject.Find("targetPosition").transform;

        //description = GameObject.Find("description").GetComponent<Text>();
        description.text = "";

        //hide all items
        for(int i = 0; i < itemButtons.Count; i++)
        {
            //itemButtons[i].name = Inventory.itemList[i].itemData.itemName;

            if(i < Inventory.itemList.Count)
            {
                itemButtons[i].name = Inventory.itemList[i].itemData.itemName;
                itemButtons[i].interactable = true;
                itemButtons[i].gameObject.GetComponentInChildren<Text>().text = "" + itemButtons[i].name;
            }
            else
            {
                itemButtons[i].interactable = false;

            }

        }

        //display viewable items
        //for(int i = 0; i < Inventory.itemList.Count; i++)
        //{
        //    itemButtons[i].name = Inventory.itemList[i].itemData.itemName;
        //    itemButtons[i].interactable = true;
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenGallery()
    {
        gallery.SetActive(true);
    }

    public void CloseGallery()
    {
        gallery.SetActive(false);
    }

    public void DisplayItem()
    {
        selectedButton = EventSystem.current.currentSelectedGameObject;

        if(displayItem != null)
        {
            Destroy(displayItem);
        }

        for(int i = 0; i < itemButtons.Count; i++)
        {
            if(itemButtons[i].gameObject.name == selectedButton.name)
            {
                for (int j = 0; j < Inventory.itemList.Count; j++)
                {
                    if(j == i)
                    {
                        description.text = "" + Inventory.itemList[i].itemData.description;
                        //displayItem = Inventory.itemList[i].itemData.itemObject;
                        displayItem = Instantiate(Inventory.itemList[i].itemData.itemObject, targetPos.position, targetPos.rotation);
                        //displayItem.transform.position = targetPos.position;
                        return;
                    }
                }

            }

        }

    }
}
