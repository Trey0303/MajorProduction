using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    //public Enemy enemySpawner;

    public int health;

    private void Start()
    {
        //enemySpawner = GameObject.Find("EnemySpawner").GetComponent<Enemy>();   
        //health += enemySpawner.healthIncrease;

    }

    private void Update()
    {
        if(health <= 0)
        {
            //PlayerVariableData.money += 1 + enemySpawner.moneyEarnIncrease;
            //Debug.Log(enemySpawner.moneyEarnIncrease);
            //enemySpawner.dead = true;
            Destroy(this.gameObject);
        }
    }
}
