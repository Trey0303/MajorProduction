using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFellOutOfMap : MonoBehaviour
{
    public Rigidbody player;
    private PlayerControllerIsometric playerControllerScript;

    public Vector3 movePlayer;

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
            player.position = new Vector3(player.position.x + movePlayer.x, player.position.y + movePlayer.y, player.position.z + movePlayer.z);

            playerControllerScript.enabled = true;
        }
    }
}
