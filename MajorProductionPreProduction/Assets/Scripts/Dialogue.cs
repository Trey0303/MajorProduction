using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public GameObject dialogueBox;

    public List<GameObject> text;

    int curText;

    public static bool canClick { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        canClick = false;
        curText = 0;
        StartCoroutine(LateStart(.1f));
    }
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
#pragma warning disable 0618
        dialogueBox.active = true;
        PlayerControllerIsometric.canMove = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (canClick)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.E))
            {
                curText++;
            }
            //still text left
            if (curText < text.Count)
            {
                for(int i = 0; i < text.Count; i++)
                {
                    if(i == curText)
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
                dialogueBox.active = false;
                if (PlayerControllerIsometric.canMove && !dialogueBox.active)
                {
                    //Debug.Log(PlayerControllerIsometric.canMove);
                    this.enabled = false;

                }
            }

        }

        
    }
}