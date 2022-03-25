using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FindTarget : MonoBehaviour
{

    public SkillObj skill;

    public List<GameObject> targets;

    //public int targetLayer = 6;
    internal int damage;

    private void Update()
    {
        if (targets != null)
        {
            for(int i = 0; i < targets.Count; i++)
            {
                skill.HitTarget(targets[i], damage);
                targets[i] = null;
            }
        }
    }


    //enemy and player will be unable to hurt themselves because of how their attack hitbox is parented to them???
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log("triggered");
        if (other.gameObject.tag == "Enemy")//enemy
        {
            //Debug.Log("enemy in range");
            //Debug.Log(other.gameObject.name);
            targets.Add(other.gameObject);
        }
        if (other.gameObject.tag == "Player")
        {
            targets.Add(other.gameObject);
        }
        if(other.gameObject.tag == "Boss")
        {
            targets.Add(other.gameObject);
        }


    }
}
 