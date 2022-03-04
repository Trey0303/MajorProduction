using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 10.0f;
    public int damage = 1;
    public float bulletDespawnTimer = 8;

    //public bool isFriendly = true;

    // Start is called before the first frame update
    private void Start()
    {
        Destroy(gameObject, bulletDespawnTimer);
    }

    // Update is called once per frame
    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);

    }

    private void OnTriggerEnter(Collider collider)
    {

        if(collider.tag == "Player")
        {
            //ShipController ship = collider.GetComponent<ShipController>();

            //if (ship)
            //{
                //ship.TakeDamage(damage);

                Destroy(gameObject);
            //}   
        }

    }
}
