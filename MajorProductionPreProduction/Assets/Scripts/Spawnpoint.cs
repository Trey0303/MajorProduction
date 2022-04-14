using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawnpoint : MonoBehaviour
{
    public Transform spawnpoint;
    public Transform player;
    private Rigidbody playerRB;

    public PlayerControllerIsometric playerControllerScript;
    public Dialogue dialogueScript;

    public float waitTime = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript.enabled = false;
        dialogueScript.enabled = false;

        playerRB = player.GetComponent<Rigidbody>();
        playerRB.position = spawnpoint.position;
        StartCoroutine(LateStart(waitTime));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        playerControllerScript.enabled = true;
        dialogueScript.enabled = true;
    }
}
