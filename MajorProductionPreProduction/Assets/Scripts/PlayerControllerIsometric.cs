using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerIsometric : MonoBehaviour
{
    //player movement Speed
    public int speed = 10;

    // Update is called once per frame
    void Update()
    {
        Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
        //deltaTime makes player move by 5 seconds instead of 5 frames
        transform.Translate(input * speed * Time.deltaTime);
    }
}
