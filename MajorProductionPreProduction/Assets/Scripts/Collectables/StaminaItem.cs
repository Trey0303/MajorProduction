using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaminaItem : MonoBehaviour
{
    public float amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            PlayerControllerIsometric.stamina = PlayerControllerIsometric.stamina + amount;

            Destroy(this.gameObject);
        }
    }
}
