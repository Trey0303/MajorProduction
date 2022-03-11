using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    public float healAmount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerHealth.curHealth = PlayerHealth.curHealth + healAmount;

            Destroy(this.gameObject);
        }
    }
}
