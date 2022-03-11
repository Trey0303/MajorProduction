using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public GameObject dialogueBox;

    // Start is called before the first frame update
    void Start()
    {
        
        StartCoroutine(LateStart(.1f));
    }
    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        dialogueBox.active = true;
        PlayerControllerIsometric.canMove = false;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.E))
        {
            PlayerControllerIsometric.canMove = true;
            dialogueBox.active = false;
            if(PlayerControllerIsometric.canMove && !dialogueBox.active)
            {
                //Debug.Log(PlayerControllerIsometric.canMove);
                this.enabled = false;

            }
        }
    }
}
