using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public GameObject dialogueBox;

    public List<GameObject> text;

    int curText;

    bool startingDialogue;

    public static bool canClick { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        startingDialogue = true;
        //PlayerControllerIsometric.canMove = false;
        PlayerControllerIsometric.startingDialogueActive = true;
        canClick = false;
        curText = 0;
        Time.timeScale = 0;
        //DebugEx.Log("Start Dialogue Coroutine");
        StartCoroutine(LateStart(.1f));
    }
    IEnumerator LateStart(float waitTime)
    {
        if (dialogueBox)
        {
            //PlayerControllerIsometric.canMove = false;
            PlayerControllerIsometric.startingDialogueActive = true;

            //DebugEx.Log("yield dialogue: " + waitTime * 2);
            yield return new WaitForSeconds(waitTime);
            PlayerControllerIsometric.startingDialogueActive = true;
    #pragma warning disable 0618
            dialogueBox.active = true;
            //DebugEx.Log("dialogue active:" + dialogueBox.active);
            //DebugEx.Log("player movement: " + PlayerControllerIsometric.canMove);
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
        if(dialogueBox != null)
        {
            if (!startingDialogue)
            {
                PlayerControllerIsometric.startingDialogueActive = false;
                Destroy(dialogueBox);
                Destroy(GetComponent<Dialogue>());
                this.enabled = false;
            }

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
                        Time.timeScale = 1;
                        PlayerControllerIsometric.startingDialogueActive = false;
                        //Debug.Log(PlayerControllerIsometric.canMove);
                        startingDialogue = false;
                        //this.enabled = false;

                    }
                }

            }

        }


        
    }
}
