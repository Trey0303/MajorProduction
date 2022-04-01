using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public GameObject dialogueBox;

    public List<GameObject> text;

    int curText;

    private bool textDisplayed;

    // Start is called before the first frame update
    void Start()
    {
        dialogueBox.SetActive(false);
        textDisplayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (textDisplayed)
        {
            if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.E))
            {
                curText++;
            }
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
                dialogueBox.active = false;
                if (PlayerControllerIsometric.canMove && !dialogueBox.active)
                {
                    Time.timeScale = 1;
                    //Debug.Log(PlayerControllerIsometric.canMove);
                    Destroy(this.gameObject);

                }
            }

        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            //DebugEx.Log("trigger");
            dialogueBox.SetActive(true);
            PlayerControllerIsometric.canMove = false;
            textDisplayed = true;
            Time.timeScale = 0;
        }
    }
}
