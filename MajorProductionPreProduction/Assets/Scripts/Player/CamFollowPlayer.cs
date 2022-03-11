using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    private GameObject cam;
    public GameObject player;
    public float distanceFromPlayerX;
    public float distanceFromPlayerY;
    public float distanceFromPlayerZ;

    private void Start()
    {
        cam = this.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null && cam != null)
        {
            if(cam.transform.position.z != player.transform.position.z)
            {
                float tempX = player.transform.position.x;
                cam.transform.position = new Vector3(tempX - distanceFromPlayerX, player.transform.position.y + distanceFromPlayerY, player.transform.position.z + distanceFromPlayerZ);

            }

        }
        if(player == null)
        {
            DebugEx.Log("camera is missing reference to player");
        }


    }
}
