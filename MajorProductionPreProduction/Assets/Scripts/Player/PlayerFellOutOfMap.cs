using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFellOutOfMap : MonoBehaviour
{
    public Rigidbody player;
    private PlayerControllerIsometric playerControllerScript;

    public Transform respawnpoint;
    //public Vector3 movePlayer;

    //public float movePlayerY = 2;
    //public float movePlayerX = 2;
    //public float movePlayerZ = 2;

    // Start is called before the first frame update
    void Start()
    {
        playerControllerScript = player.gameObject.GetComponent<PlayerControllerIsometric>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            playerControllerScript.enabled = false;

            player = player.GetComponent<Rigidbody>();
            player.position = respawnpoint.position;

            playerControllerScript.enabled = true;
        }
    }
}
