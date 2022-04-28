using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapCam : MonoBehaviour
{
    private GameObject cam;
    private Camera cameraComponent;
    public GameObject player;
    public float distanceFromPlayerX;
    public float distanceFromPlayerY;
    public float distanceFromPlayerZ;

    private void Start()
    {
        cam = this.gameObject;
        cameraComponent = cam.GetComponent<Camera>();

        cameraComponent.cullingMask &= ~(1 << 11);
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null && cam != null)
        {
            if (cam.transform.position.z != player.transform.position.z || cam.transform.position.x != player.transform.position.x)
            {
                float tempX = player.transform.position.x;
                cam.transform.position = new Vector3(tempX - distanceFromPlayerX, player.transform.position.y + distanceFromPlayerY, player.transform.position.z + distanceFromPlayerZ);

            }

        }
        if (player == null)
        {
            DebugEx.Log("camera is missing reference to player");
        }


    }
}
