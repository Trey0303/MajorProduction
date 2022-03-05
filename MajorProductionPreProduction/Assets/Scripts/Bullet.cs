using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Rigidbody rb;

    public float speed = 10.0f;
    public int damage = 1;
    public float bulletDespawnTimer = 8;

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
        Debug.Log("rotation: " + transform.rotation);
        Debug.Log("rb rotation: " + rb.rotation);
        rb.MovePosition(rb.position + (Vector3.forward * speed) * Time.deltaTime);
        //transform.Translate(Vector3.forward * speed * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.layer != LayerMask.NameToLayer("Enemy"))
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
