using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAI : MonoBehaviour
{
    //public Agent agent;

    //public float speed = 3.0f;

    //enemy aggro/move towards player
    [Header("Aggro Range")]
    public float range = 30;
    //public float moveSpeed = 10f;

    [Header("Melee Settings")]
    //melee
    public List<SkillProgress> attacks = new List<SkillProgress>();//enemy attack
    public float meleeRange = 3.15f;//melee state range
    //public float meleeStartup = 1.5f;//melee interval

    public float lightAttackStartup = 1.2f;
    public float lightKnockback = 4.5f;//amount of knockback player will receive

    public float heavyAttackStartup = 3f;
    public float heavyKnockback = 3f;

    public float criticalAttackStartup = 5f;
    public float criticalKnockback = 5f;

    public float meleeStaggerTime = .7f;//give stagger time to player on hit
    public float playerKnockedbackTimeSet = 1;//length of time that player will receive 'meleeKnockback'
    public bool canHit;
    public float attackEndLag = .7f;

    //Programmer Only
    [Header("other/ programmer variables")]
    //navMesh
    public NavMeshAgent navAgent;
    public Transform target;

    //rigidbody
    private Rigidbody rb;

    //melee
    private float hitTimer;//hit interval
    public bool midAttack = false;

    private float attackEndLagTimer;

    private int attackLastUsed = 0;

    //private movementType curMovement;
    private int curAction;
    public static float playerKnockedBackTime { get; set; }

    private void Start()
    {
        //get information for all attached skills
        if (attacks.Count != 0)
        {
            for (int i = 0; i < attacks.Count; i++)
            {
                attacks[i].AddSkill();
            }

        }
        hitTimer = lightAttackStartup;
        curAction = 0;
        rb = GetComponent<Rigidbody>();
        //staggerTimer = setStaggerTime;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        switch (curAction)
        {
            //IDLE
            case 0:
                //DebugEx.Log("IDLE");
                navAgent.updateRotation = false;
                navAgent.isStopped = true;

                break;
                //MOVE
            case 1:
                //DebugEx.Log("MOVE");
                if (!midAttack)
                {
                    navAgent.updateRotation = true;
                    navAgent.isStopped = false;

                    navAgent.SetDestination(new Vector3(target.position.x, 0, target.position.z));
                }
                break;
                //ATTACK SELECT
            case 2:
                if (canHit)
                {
                    //DebugEx.Log("ATTACK SELECT");
                    if (!midAttack)
                    {
                        //TODO: randomly select an attack
                        curAction = 3;
                    }
                }
                break;
                //LIGHT ATTACK
            case 3:
                //DebugEx.Log("Light Attack");
                if (canHit)
                {
                    navAgent.updateRotation = false;
                    navAgent.isStopped = true;

                    //DebugEx.Log("midAttack " + midAttack);
                    if (!midAttack)
                    {
                        Vector3 direction = (target.position - transform.position).normalized;
                        direction.y = 0;//prevents enemy from looking up

                        var targetRot = Quaternion.LookRotation(direction, Vector3.up);
                        //rb.MoveRotation(Quaternion.LookRotation(direction, Vector3.up));
                        rb.rotation = targetRot;
                        transform.rotation = targetRot;

                        Debug.DrawRay(transform.position, direction * 5.0f, Color.red);
                        Debug.Assert(!navAgent.updateRotation, "Agent is updating its rotation when it should be aiming!", this);

                        hitTimer -= Time.deltaTime;
                        //DebugEx.Log("hitTimer: " + hitTimer);
                        if (hitTimer <= 0.0f)
                        {
                            for (int i = 0; i < attacks.Count; i++)
                            {
                                if (i == attackLastUsed)
                                {
                                    attackEndLagTimer = attacks[i].skillData.activeHitBoxTimer + attackEndLag;
                                    //DebugEx.Log("Reset Timer to: " + attackEndLagTimer);
                                }
                            }
                            LightAttack();
                            hitTimer = lightAttackStartup;

                        }
                    }
                }
                break;
                //HEAVY ATTACK
            case 4:
                DebugEx.Log("Heavy Attack");
                if (canHit)
                {
                    navAgent.updateRotation = false;
                    navAgent.isStopped = true;

                    if (!midAttack)
                    {
                        Vector3 direction = (target.position - transform.position).normalized;
                        direction.y = 0;//prevents enemy from looking up

                        var targetRot = Quaternion.LookRotation(direction, Vector3.up);
                        //rb.MoveRotation(Quaternion.LookRotation(direction, Vector3.up));
                        rb.rotation = targetRot;
                        transform.rotation = targetRot;

                        Debug.DrawRay(transform.position, direction * 5.0f, Color.red);
                        Debug.Assert(!navAgent.updateRotation, "Agent is updating its rotation when it should be aiming!", this);

                        hitTimer -= Time.deltaTime;
                        //DebugEx.Log("hitTimer: " + hitTimer);
                        if (hitTimer <= 0.0f)
                        {
                            for (int i = 0; i < attacks.Count; i++)
                            {
                                if (i == attackLastUsed)
                                {
                                    attackEndLagTimer = attacks[i].skillData.activeHitBoxTimer + attackEndLag;
                                    DebugEx.Log("Reset Timer to: " + attackEndLagTimer);
                                }
                            }
                            HeavyAttack();
                            hitTimer = lightAttackStartup;

                        }
                    }
                }
                break;
            //CRITICAL ATTACK
            case 5:
                DebugEx.Log("Critical Attack");
                if (canHit)
                {
                    navAgent.updateRotation = false;
                    navAgent.isStopped = true;

                    if (!midAttack)
                    {
                        Vector3 direction = (target.position - transform.position).normalized;
                        direction.y = 0;//prevents enemy from looking up

                        var targetRot = Quaternion.LookRotation(direction, Vector3.up);
                        //rb.MoveRotation(Quaternion.LookRotation(direction, Vector3.up));
                        rb.rotation = targetRot;
                        transform.rotation = targetRot;

                        Debug.DrawRay(transform.position, direction * 5.0f, Color.red);
                        Debug.Assert(!navAgent.updateRotation, "Agent is updating its rotation when it should be aiming!", this);

                        hitTimer -= Time.deltaTime;
                        //DebugEx.Log("hitTimer: " + hitTimer);
                        if (hitTimer <= 0.0f)
                        {
                            for (int i = 0; i < attacks.Count; i++)
                            {
                                if (i == attackLastUsed)
                                {
                                    attackEndLagTimer = attacks[i].skillData.activeHitBoxTimer + attackEndLag;
                                    DebugEx.Log("Reset Timer to: " + attackEndLagTimer);
                                }
                            }
                            CriticalAttack();
                            hitTimer = lightAttackStartup;

                        }
                    }
                }
                break;
           
        }

        //calculate the difference between your target location and current location
        //(this give you an offset from your position to your target)
        //Vector3 distance = target.position - transform.position;
        //distance.y = 0.0f;

        if (target.position.x < transform.position.x + range && target.position.z < transform.position.z + range && target.position.x + range > transform.position.x && target.position.z + range > transform.position.z)
        {
            //DebugEx.Log("Player in range");
            //shoot timer
            //firingTimer -= Time.deltaTime;

            //End lag logic
            //if enemy is in the middle of its attack
             if (attackEndLagTimer > 0 && midAttack)
            {
                attackEndLagTimer = attackEndLagTimer - Time.deltaTime;
            }
            else if (attackEndLagTimer <= 0 && midAttack)
             {
                //DebugEx.Log("midAttack set back to FALSE");
                midAttack = false;

             }

            //ATTACK AND MOVEMENT LOGIC
            if (canHit)
            {
                MeleeLogic();
            }
        }
        else//idle switch
        {
            curAction = 0;
            //DebugEx.Log("target out of sight");
        }

        //DebugEx.Log("curAction " + curAction);

    }

    void MeleeLogic()
    {
        //if in shooting range
        if (canHit && target.position.x < transform.position.x + meleeRange && target.position.z < transform.position.z + meleeRange && target.position.x + meleeRange > transform.position.x && target.position.z + meleeRange > transform.position.z)
        {
            if(curAction <= 2)
            {
                //DebugEx.Log("target in attack range");
                curAction = 2;

            }


        }
        else//switch back to movement state
        {
            //Movement(distance);
            curAction = 1;
            //DebugEx.Log("moving towards target");
        }
    }

    private void LightAttack()
    {
        if (attacks.Count != 0)
        {
            if (attacks[0].skillData != null)
            {
                midAttack = true;
                playerKnockedBackTime = playerKnockedbackTimeSet;
                attackLastUsed = 0;
                attacks[0].Use(this.gameObject, lightKnockback, playerKnockedBackTime);

            }

        }
        else
        {
            DebugEx.Log("Boss attack not attached");
        }
    }

    private void HeavyAttack()
    {
        if (attacks.Count != 0)
        {
            if (attacks[1].skillData != null)
            {
                midAttack = true;
                playerKnockedBackTime = playerKnockedbackTimeSet;
                attackLastUsed = 1;
                attacks[1].Use(this.gameObject, heavyKnockback, playerKnockedBackTime);

            }

        }
        else
        {
            DebugEx.Log("Boss attack not attached");
        }
    }

    private void CriticalAttack()
    {
        if (attacks.Count != 0)
        {
            if (attacks[2].skillData != null)
            {
                midAttack = true;
                playerKnockedBackTime = playerKnockedbackTimeSet;
                attackLastUsed = 2;
                attacks[2].Use(this.gameObject, criticalKnockback);

            }

        }
        else
        {
            DebugEx.Log("Boss attack not attached");
        }
    }

    //void Fire()
    //{
    //    if (!staggered)
    //    {
    //        GameObject newBullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y + bulletSpawnY, transform.position.z), transform.rotation);
    //        Vector3 playerPositionCopy = target.position;
    //        playerPositionCopy.y = newBullet.transform.position.y;
    //        newBullet.transform.forward = (playerPositionCopy - newBullet.transform.position).normalized;

    //    }

    //}

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawWireSphere(transform.position, shootRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
