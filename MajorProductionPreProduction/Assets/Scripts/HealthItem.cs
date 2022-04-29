using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public float healAmount = 1;

    private Vector3 velocity;

    //public Rigidbody rb;

    //void Start()
    //{
    //    rb = this.gameObject.GetComponent<Rigidbody>();
    //    velocity = rb.velocity;

    //}



    //private void FixedUpdate()
    //{
    //    velocity += Physics.gravity * 1 * Time.deltaTime;//gravity
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealth.curHealth = PlayerHealth.curHealth + healAmount;

            Destroy(this.gameObject);
        }
    }
}
