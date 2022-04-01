using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class textItem : MonoBehaviour
{


    [SerializeField]
    Text item;
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

    // Start is called before the first frame update
    void Start()
    {
        itemTransform = transform;
        textTransform = item.transform;

        player = GameObject.Find("Player");

        item.enabled = false;

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

        if (player != null)
        {

            if (Input.GetKeyUp(KeyCode.E) && item.enabled)
            {
                item.enabled = false;
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
                        Time.timeScale = 1;
                        //Debug.Log(PlayerControllerIsometric.canMove);
                        Destroy(itemInfoBox.gameObject);

                    }
                }

                //PlayerControllerIsometric.canMove = true;
                //Destroy(this.gameObject);
                //Destroy(itemInfoBox.gameObject);
            }

            

            RangeCheck();
        }

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
                    item.enabled = true;

                    Vector3 screenPos = Camera.main.WorldToScreenPoint(itemTransform.position);
                    screenPos.y += 30;
                    screenPos.x += 50;
                    textTransform.transform.position = screenPos;


                }

            }
            else if (item.enabled)
            {
                item.enabled = false;
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

