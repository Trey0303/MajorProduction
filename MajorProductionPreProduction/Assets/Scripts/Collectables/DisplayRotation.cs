using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayRotation : MonoBehaviour
{
    public float speed = 1;
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(0, 0, -90) * Time.deltaTime * speed);
    }
}
