using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillData", menuName = "SkillObjects/ScriptableSkills", order = 1)]
public class SkillObj : ScriptableObject
{
    public string skillName;
    public int damage;
    public int maxLevel;
    public int cost;

    //use skill
    public string characterTag;
    protected GameObject wielder;
    //protected bool hitboxSpawned = false;

    public GameObject hurtboxPrefab;


    public virtual void Use(float skillProgDamage) {
        wielder = GameObject.FindWithTag(characterTag);

        //create hitbox
        DisplayHitBox(skillProgDamage);

    }

    //display hitBox and find target
    void DisplayHitBox(float skillProgDamage)
    {
        //GameObject hitbox = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var box = Instantiate(hurtboxPrefab, wielder.transform.position + wielder.transform.forward, wielder.transform.rotation);

        //get target from FindTarget
        var targetTemp = box.GetComponent<FindTarget>();

        //give FindTarget current skill
        targetTemp.skill = this;

        
        int tempDamage = (int)skillProgDamage;

        //created a damage var in targetTemp to prevent changes being saved to default damage
        targetTemp.damage = tempDamage;

        //hitbox follows player while active
        box.transform.parent = wielder.transform;

        Destroy(box, 0.1f);
    }

    //apply damage
    public virtual void HitTarget(GameObject targetCollider , float skillProgDamage)
    {
        if (targetCollider != null)
        {
            if (targetCollider.gameObject.GetComponent<Health>() != null)
            {
                Health targetHealth = targetCollider.gameObject.GetComponent<Health>();

                //damage = 

                targetHealth.health = targetHealth.health - (int)skillProgDamage;
                //Debug.Log("Enemy Health: " + targetHealth.health);
                targetCollider.gameObject.GetComponent<EnemyAi>().staggered = true;
                targetCollider.gameObject.GetComponent<EnemyAi>().staggerTimer = targetCollider.gameObject.GetComponent<EnemyAi>().setStaggerTime;

            }
            else
            {
                DebugEx.Log("target does NOT have health script attached");
            }
        }
    }

}