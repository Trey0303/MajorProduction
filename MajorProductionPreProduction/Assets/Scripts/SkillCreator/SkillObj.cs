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
    protected float knockbackTime;
    //protected bool hitboxSpawned = false;

    public GameObject hurtboxPrefab;


    public virtual void Use(float skillProgDamage, GameObject curWielder) {
        wielder = curWielder;

        //create hitbox
        DisplayHitBox(skillProgDamage);

    }

    public virtual void Use(float skillProgDamage, GameObject curWielder, float knockbackAmount, float knockbackTimeSet)
    {
        wielder = curWielder;

        knockbackStrength = knockbackAmount;

        knockbackTime = knockbackTimeSet;

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

        Destroy(box, activeHitBoxTimer);
    }

    //apply damage
    public virtual void HitTarget(GameObject targetCollider , float skillProgDamage)
    {
        if (targetCollider != null)
        {
            if (targetCollider.gameObject.GetComponent<Health>() != null && targetCollider.gameObject.tag != wielder.gameObject.tag)
            {
                Health targetHealth = targetCollider.gameObject.GetComponent<Health>();

                //damage = 

                targetHealth.health = targetHealth.health - (int)skillProgDamage;
                //Debug.Log("Enemy Health: " + targetHealth.health);
                if(wielder.tag == "Enemy")
                {
                    targetCollider.gameObject.GetComponent<EnemyAi>().staggered = true;
                    targetCollider.gameObject.GetComponent<EnemyAi>().staggerTimer = targetCollider.gameObject.GetComponent<EnemyAi>().setStaggerTime;

                }
                if(wielder.tag == "FlyingEnemies")
                {
                    targetCollider.gameObject.GetComponent<EnemyAi>().staggered = true;
                    targetCollider.gameObject.GetComponent<AirEnemieAi>().staggerTimer = targetCollider.gameObject.GetComponent<AirEnemieAi>().setStaggerTime;
                }

            }
            else if (targetCollider.gameObject.tag == "Player" && targetCollider.gameObject.tag != wielder.gameObject.tag)//if target is the player
            {
                //DebugEx.Log("player hit");
                if (!PlayerControllerIsometric.invincibility)
                {
                    PlayerHealth.curHealth = PlayerHealth.curHealth - skillProgDamage;//apply damage to player

                    //apply HITSTUN to the player
                    if (wielder.tag == "Enemy")
                    {
                        PlayerControllerIsometric.staggerTimer = wielder.gameObject.GetComponent<EnemyAi>().playerMeleeStaggerTime;//add hit stun to player

                    }
                    if(wielder.tag == "FlyingEnemies")
                    {
                        PlayerControllerIsometric.staggerTimer = wielder.gameObject.GetComponent<AirEnemieAi>().playerMeleeStaggerTime;//add hit stun to player
                    }
                    else if(wielder.tag == "Boss") { 
                        if(wielder.gameObject.GetComponent<BossAI>().curAction == 3)
                        {
                            PlayerControllerIsometric.staggerTimer = wielder.gameObject.GetComponent<BossAI>().lightKnockbackStagger;//add hit stun to player
                        }
                        if (wielder.gameObject.GetComponent<BossAI>().curAction == 4)
                        {
                            PlayerControllerIsometric.staggerTimer = wielder.gameObject.GetComponent<BossAI>().heavyKnockbackTime;
                        }
                        if (wielder.gameObject.GetComponent<BossAI>().curAction == 5)
                        {
                            PlayerControllerIsometric.staggerTimer = wielder.gameObject.GetComponent<BossAI>().criticalKnockbackTime;
                        }
                    }
                    playerRb = targetCollider.gameObject.GetComponent<Rigidbody>();
                    //playerRb.AddForce(knockbackDirection.normalized * 500);
                    Vector3 knockbackDirection = playerRb.position - wielder.transform.position;

                    knockbackDirection.y = 0;

                    playerRb.gameObject.GetComponent<PlayerControllerIsometric>().Knockback(knockbackStrength, knockbackTime, knockbackDirection);

                }

            }
            else if (targetCollider.gameObject.tag == wielder.gameObject.tag)
            {
                DebugEx.Log("cant damage ally");
            }
            else
            {
                DebugEx.Log("target does NOT have health script attached");
            }
        }
    }

}