using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //public Enemy enemySpawner;

    public int health;
    public float dropRate = .50f;
    public GameObject healthItemPrefab;

    private void Update()
    {
        if(health <= 0)
        {
            float dropChance = Random.Range(0.0f, 1.0f);
            //DebugEx.Log(dropChance);
            if(dropChance <= dropRate)
            {
                GameObject healthDrop = Instantiate(healthItemPrefab, new Vector3(transform.position.x, GameObject.Find("Player").transform.position.y, transform.position.z), transform.rotation);
            }
            PlayerControllerIsometric.killcount = PlayerControllerIsometric.killcount + 1;//increase kill count by 1
            Destroy(this.gameObject);
        }
    }
}
