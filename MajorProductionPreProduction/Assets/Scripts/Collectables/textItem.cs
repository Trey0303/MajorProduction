using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class textItem : MonoBehaviour
{
    [SerializeField]
    Text pressInteractButtonText;
    [SerializeField]
    GameObject itemInfoBox;
    Transform itemTransform;
    Transform textTransform;


    GameObject player;

    public float radius = 5;

    Vector3 itemRadius;

    Vector3 playerRadius;

    bool itemInfoActive;
    private Vector3 lastRecordedPosition;

    private int curText;
    public List<GameObject> text;

    public ItemProgress itemScriptableObject;
    
    //Relic Upgrades
    [Header("Relic Upgrades")]

    public bool maxHealth;
    public bool attack;
    public bool stamina;
    public int increaseAmount = 1;

    // Start is called before the first frame update
    void Start()
    {
        if(itemScriptableObject != null)
        {
            itemScriptableObject.AddItem();
        }

        itemTransform = transform;
        textTransform = pressInteractButtonText.transform;

        player = GameObject.Find("Player");

        pressInteractButtonText.enabled = false;

        itemRadius = new Vector3(transform.position.x + radius, transform.position.y + radius, transform.position.z + radius);
        playerRadius = new Vector3(player.transform.position.x + radius, player.transform.position.y + radius, player.transform.position.z + radius);

        itemInfoActive = false;

        //itemInfo = item.gameObject.GetComponentInChildren<Text>();

        lastRecordedPosition = player.transform.position;

        itemInfoBox.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyUp(KeyCode.F))
        //{
        //    DebugEx.Log("Dead");
        //    PlayerHealth.curHealth = 0;
        //}

        if (player != null)
        {

            if (Input.GetKeyUp(KeyCode.E) && pressInteractButtonText.enabled)
            {
                pressInteractButtonText.enabled = false;
                Time.timeScale = 0;
                //itemInfoBox.SetActive(true);//get a direct refernce to child object instead
                //itemInfoActive = true;
                PlayerControllerIsometric.canMove = false;
                itemInfoBox.gameObject.SetActive(true);
                itemInfoActive = true;

                //tell game that this specific item has been collected and stored for future reference
            }
            else if (Input.GetMouseButtonUp(0) && itemInfoActive || Input.GetKeyUp(KeyCode.E) && itemInfoActive)
            {
                curText++;

                //still text left
                if (curText < text.Count)
                {
                    for (int i = 0; i < text.Count; i++)
                    {
                        if (i == curText)
                        {
#pragma warning disable 0618
                            text[i].active = true;
                        }
                        else
                        {
                            text[i].active = false;
                        }
                    }
                }
                //no text left
                if (curText >= text.Count)
                {
                    PlayerControllerIsometric.canMove = true;
                    itemInfoBox.gameObject.SetActive(true);
                    itemInfoActive = false;
                    if (PlayerControllerIsometric.canMove && !itemInfoActive)
                    {
                        //player.GetComponent<Inventory>().itemList.Add(itemScriptableObject);
                        for(int i = 0; i <= Inventory.itemList.Count; i++)
                        {
                            if(Inventory.itemList.Count != 0)
                            {
                                if (i == Inventory.itemList.Count)
                                {
                                    AddItem();
                                    return;
                                }
                                else if (Inventory.itemList[i].itemData.itemName.Contains(itemScriptableObject.itemData.itemName))
                                {
                                    DebugEx.Log("already have item");
                                    Time.timeScale = 1;
                                    //Debug.Log(PlayerControllerIsometric.canMove);
                                    Destroy(itemInfoBox.gameObject);
                                    Destroy(this.gameObject);
                                    return;
                                }

                            }
                            else
                            {
                                if (i == Inventory.itemList.Count)
                                {
                                    AddItem();
                                    return;
                                }
                            }
                        }

                    }
                }
            }



            RangeCheck();
        }

    }

    private void AddItem()
    {
        Inventory.itemList.Add(itemScriptableObject);
        
        //increase selected stat
        if (maxHealth)
        {
            IncreaseMaxHealth();
        }
        if (stamina)
        {
            IncreaseMaxStamina();
        }
        if (attack)
        {
            IncreaseAttack();
        }
        
        DebugEx.Log("Item Added");
        Time.timeScale = 1;
        //Debug.Log(PlayerControllerIsometric.canMove);
        Destroy(itemInfoBox.gameObject);
        Destroy(this.gameObject);
    }

    private void IncreaseMaxStamina()
    {
        PlayerHealth.maxStamina = PlayerHealth.maxStamina + increaseAmount;// increase max stamina
    }

    private void IncreaseMaxHealth()
    {
        PlayerHealth.maxHealth = PlayerHealth.maxHealth + increaseAmount;//increase max health
    }

    private void IncreaseAttack()
    {
        PlayerAttack.normalAttackDamage = PlayerAttack.normalAttackDamage + increaseAmount;
    }

    private void RangeCheck()
    {
        if (player.transform.position != lastRecordedPosition)
        {
            lastRecordedPosition = player.transform.position;
            itemRadius = new Vector3(transform.position.x + radius, transform.position.y + radius, transform.position.z + radius);
            playerRadius = new Vector3(player.transform.position.x + radius, player.transform.position.y + radius, player.transform.position.z + radius);

            if (player.transform.position.x <= itemRadius.x && player.transform.position.y <= itemRadius.y && player.transform.position.z <= itemRadius.z &&
                transform.position.x <= playerRadius.x && transform.position.y <= playerRadius.y && transform.position.z <= playerRadius.z)
            {
                if (!itemInfoActive)
                {
                    //DebugEx.Log("IN RANGE");
                    pressInteractButtonText.enabled = true;

                    Vector3 screenPos = Camera.main.WorldToScreenPoint(itemTransform.position);
                    screenPos.y += 30;
                    screenPos.x += 50;
                    textTransform.transform.position = screenPos;


                }

            }
            else if (pressInteractButtonText.enabled)
            {
                pressInteractButtonText.enabled = false;
            }
        }
    }

    //private void FixedUpdate()
    //{
    //    if (player != null)
    //    {



    //    }
    //}

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //    }
    //}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}

