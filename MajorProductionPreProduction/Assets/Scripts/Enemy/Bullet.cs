using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;

    public float speed = 10.0f;
    public int damage = 1;
    public float bulletDespawnTimer = 8;
    private Vector3 AngularVel;
    private Quaternion deltaRotation;

    //public bool isFriendly = true;

    // Start is called before the first frame update
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        Destroy(gameObject, bulletDespawnTimer);
    }

    // Update is called once per frame
    private void Update()
    {
        Movement();

    }

    void Movement()
    {
        //Debug.Log("rotation: " + transform.rotation);
        //Debug.Log("rb rotation: " + rb.rotation);

        //AngularVel = Vector3.forward * speed;

        //deltaRotation = Quaternion.Euler(AngularVel* Time.deltaTime);

        //rb.rotation = deltaRotation * rb.rotation;



        rb.MovePosition(rb.position + (transform.forward * speed) * Time.deltaTime);
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.layer != LayerMask.NameToLayer("Enemy") && collider.gameObject.layer != LayerMask.NameToLayer("hitbox"))
        {
            //ShipController ship = collider.GetComponent<ShipController>();
            Debug.Log(collider.gameObject.name);
            //if (ship)
            //{
                //ship.TakeDamage(damage);

            //}   
            Destroy(gameObject);
        }

    }
}
