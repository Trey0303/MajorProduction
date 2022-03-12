using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //public Enemy enemySpawner;

    public int health;
    public float dropRate = .50f;
    public GameObject healthItemPrefab;

    private void Start()
    {
        //enemySpawner = GameObject.Find("EnemySpawner").GetComponent<Enemy>();   
        //health += enemySpawner.healthIncrease;

    }

    private void Update()
    {
        if(health <= 0)
        {
            float dropChance = Random.Range(0.0f, 1.0f);
            //DebugEx.Log(dropChance);
            if(dropChance <= dropRate)
            {
                GameObject healthDrop = Instantiate(healthItemPrefab, new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);
            }

            //PlayerVariableData.money += 1 + enemySpawner.moneyEarnIncrease;
            //Debug.Log(enemySpawner.moneyEarnIncrease);
            //enemySpawner.dead = true;
            Destroy(this.gameObject);
        }
    }
}
