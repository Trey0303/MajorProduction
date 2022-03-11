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
        rb.MovePosition(rb.position + (transform.forward * speed) * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider collider)
    {

        if (collider.gameObject.layer != LayerMask.NameToLayer("Enemy") && collider.gameObject.layer != LayerMask.NameToLayer("hitbox") && collider.gameObject.layer != LayerMask.NameToLayer("item"))
        {
            //ShipController ship = collider.GetComponent<ShipController>();
            //Debug.Log(collider.gameObject.name);
            if (collider.gameObject.tag == "Player")
            {
                PlayerHealth.curHealth = PlayerHealth.curHealth - 1;

            }
            Destroy(gameObject);
        }

    }
}
