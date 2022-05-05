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

    private GameObject archives;
    private GameObject relicInfo;
    private GameObject library;
    public List<GameObject> chapterText;

    private GameObject pilots;

    private GameObject characterSelect;

    private GameObject profiles;
    
    private GameObject credits;

    public Text description;

    private GameObject displayItem;

    public List<Button> itemButtons;

    private static GameObject selectedButton;

    public Transform targetPos;

    public Text musicVol;

    public Text sfxVol;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        gallery = GameObject.Find("Index");
        gallery.SetActive(false);

        //ARCHIVES
        archives = GameObject.Find("Archives");
        archives.SetActive(false);

        relicInfo = GameObject.Find("RelicInfo");
        relicInfo.SetActive(false);

        //LIBRARY
        library = GameObject.Find("Library");
        library.SetActive(false);

        for(int i = 0; i < chapterText.Count; i++)
        {
            chapterText[i].SetActive(false);
        }

        //PILOTS
        pilots = GameObject.Find("Pilots");
        pilots.SetActive(false);

        characterSelect = GameObject.Find("CharacterSelect");
        characterSelect.SetActive(false);

        profiles = GameObject.Find("Profiles");
        profiles.SetActive(false);

        //CREDITS
        credits = GameObject.Find("Credits");
        credits.SetActive(false);

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

    public void Update()
    {
        float tempVol = Settings.globalMusicVol * 10;//move decimal point right by 1
        musicVol.text = "" + tempVol.ToString("F0");

        float tempSfxVol = Settings.globalSfxVol * 10;//move decimal point right by 1
        sfxVol.text = "" + tempSfxVol.ToString("F0");
    }

    public void OpenIndex()
    {
        gallery.SetActive(true);
    }

    //archiveMenu
    public void OpenArchives()
    {
        archives.SetActive(true);
    }

    public void OpenRelicInfo()
    {
        relicInfo.SetActive(true);
    }

    public void OpenLibrary()
    {
        library.SetActive(true);
    }


    //Open Library chapter
    public void OpenChapter1()
    {
        chapterText[0].SetActive(true);
    }

    public void OpenChapter2()
    {
        chapterText[1].SetActive(true);
    }

    public void OpenChapter3()
    {
        chapterText[2].SetActive(true);
    }

    public void OpenChapter4()
    {
        chapterText[3].SetActive(true);
    }

    public void OpenChapter5()
    {
        chapterText[4].SetActive(true);
    }


    //pilot menu
    public void OpenPilots()
    {
        pilots.SetActive(true);
    }

    public void OpenCharacterSelect()
    {
        characterSelect.SetActive(true);
    }

    public void OpenProfiles()
    {
        profiles.SetActive(true);
    }

    //Open Credits
    public void OpenCredits()
    {
        credits.SetActive(true);
    }


    public void CloseIndex()
    {
        gallery.SetActive(false);
    }

    //close ArchiveMenu
    public void CloseArchives()
    {
        archives.SetActive(false);
    }

    public void CloseRelicInfo()
    {
        relicInfo.SetActive(false);
    }

    public void CloseLibrary()
    {
        library.SetActive(false);
    }



    //close Library chapter
    public void CloseChapters()
    {
        for(int i = 0; i < chapterText.Count; i++)
        {
            chapterText[i].SetActive(false);

        }

    }

    //close pilot menu
    public void ClosePilots()
    {
        pilots.SetActive(false);
    }

    public void CloseCharacterSelect()
    {
        characterSelect.SetActive(false);
    }

    public void CloseProfiles()
    {
        profiles.SetActive(false);
    }

    public void CloseCredits()
    {
        credits.SetActive(false);
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
