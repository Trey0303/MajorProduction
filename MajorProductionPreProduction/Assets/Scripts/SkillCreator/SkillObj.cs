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
    public float activeHitBoxTimer;

    //use skill
    //public string characterName;
    protected GameObject wielder;
    private Rigidbody playerRb;
    protected float knockbackStrength;
    //protected bool hitboxSpawned = false;

    public GameObject hurtboxPrefab;


    public virtual void Use(float skillProgDamage, GameObject curWielder) {
        wielder = curWielder;

        //create hitbox
        DisplayHitBox(skillProgDamage);

    }

    public virtual void Use(float skillProgDamage, GameObject curWielder, float knockbackAmount)
    {
        wielder = curWielder;

        knockbackStrength = knockbackAmount;

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
            else if (targetCollider.gameObject.tag == "Player")//if target is the player
            {
                if (!PlayerControllerIsometric.invincibility)
                {
                    PlayerHealth.curHealth = PlayerHealth.curHealth - skillProgDamage;//apply damage to player
                    PlayerControllerIsometric.staggerTimer = wielder.gameObject.GetComponent<EnemyAi>().meleeStaggerTime;//add hit stun to player
                    //apply knockback
                    playerRb = targetCollider.gameObject.GetComponent<Rigidbody>();
                    //targetCollider.transform.position = targetCollider.transform.position + new Vector3(wielder.gameObject.GetComponent<EnemyAi>().meleeKnockback, 0, wielder.gameObject.GetComponent<EnemyAi>().meleeStaggerTime);

                    //playerRb.AddForce(knockbackDirection.normalized * 500);
                    Vector3 knockbackDirection = playerRb.position - wielder.transform.position;

                    knockbackDirection.y = 0;

                    playerRb.gameObject.GetComponent<PlayerControllerIsometric>().KnockBack(knockbackStrength, knockbackDirection);

                }

            }
            else
            {
                DebugEx.Log("target does NOT have health script attached");
            }
        }
    }

}