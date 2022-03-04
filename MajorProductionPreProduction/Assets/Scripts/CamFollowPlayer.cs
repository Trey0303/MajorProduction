using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollowPlayer : MonoBehaviour
{
    public GameObject cam;
    public GameObject player;
    public float distanceFromPlayerX;
    public float distanceFromPlayerY;
    public float distanceFromPlayerZ;

    // Update is called once per frame
    void Update()
    {
        if(cam.transform.position.z != player.transform.position.z)
        {
            float tempX = player.transform.position.x;
            cam.transform.position = new Vector3(tempX - distanceFromPlayerX, player.transform.position.y + distanceFromPlayerY, player.transform.position.z + distanceFromPlayerZ);

        }

    }
}
