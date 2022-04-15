using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AirEnemieAi : MonoBehaviour
{
    //public Agent agent;

    //public float speed = 3.0f;

    //enemy aggro/move towards player
    [Header("Aggro Range")]
    public float range = 5;
    //public float moveSpeed = 2f;
    //public float angleSpeed = 2f;


    [Header("Shoot Settings")]
    //shoot
    public GameObject bulletPrefab;//projectile prefab
    public float shootRange = 11.12f;//max shoot state range
    public float firingInterval = 1f;
    public bool canShoot;

    [Header("Melee Settings")]
    //melee
    public SkillProgress attack;//enemy attack
    public float meleeRange = 3.15f;//melee state range
    public float meleeStartup = 1.5f;//melee interval
    public float playerMeleeStaggerTime = .7f;//give stagger time to player on hit
    public float meleeKnockback = 4.5f;//amount of knockback player will receive
    public float playerKnockedbackTimeSet = 1;//length of time that player will receive 'meleeKnockback'
    public bool canHit;
    public float attackEndLag = .7f;

    [Header("Staggered timer")]
    //staggered/hurt state
    public float setStaggerTime = 1;//set stagger time

    //Programmer Only
    [Header("other/ programmer variables")]
    //navMesh
    public NavMeshAgent navAgent;
    public Transform target;

    //rigidbody
    private Rigidbody rb;


    //hurt state
    public float staggerTimer;
    public bool staggered = false;


    //shoot
    private float firingTimer;//fire interval
    public float bulletSpawnY = 3;

    //melee
    private float hitTimer;//hit interval
    public bool midAttack = false;

    private float attackEndLagTimer;

    private enum movementType
    {
        move,
        hit,
        shoot,
        idle
    }

    private movementType curMovement;
    public static float playerKnockedBackTime { get; set; }

    private void Start()
    {

        //get information for all attached skills
        if (attack.skillData != null)
        {
            attack.AddSkill();
            attackEndLagTimer = attackEndLag + attack.skillData.activeHitBoxTimer;
        }
        hitTimer = meleeStartup;
        staggered = false;
        curMovement = movementType.idle;
        rb = GetComponent<Rigidbody>();
        staggerTimer = setStaggerTime;
    }

    // Update is called once per frame
    void LateUpdate()
    {

        switch (curMovement)
        {
            case movementType.idle:
                //DebugEx.Log("IDLE");
                //navAgent.enabled = false;
                navAgent.updateRotation = false;
                navAgent.isStopped = true;
                //navAgent.speed = 0;
                //navAgent.angularSpeed = 0;

                break;
            case movementType.move:
                //DebugEx.Log("MOVE");
                if (!midAttack)
                {
                    //navAgent.enabled = true;
                    navAgent.updateRotation = true;
                    navAgent.isStopped = false;
                    //navAgent.speed = moveSpeed;
                    //navAgent.angularSpeed = angleSpeed;

                    if (!staggered)
                    {
                        navAgent.SetDestination(new Vector3(target.position.x, 7, target.position.z));

                    }

                }

                break;
            case movementType.hit:
                if (canHit && target.position.y >= transform.position.y)
                {
                    //navAgent.enabled = false;
                    navAgent.updateRotation = false;
                    navAgent.isStopped = true;
                    //navAgent.speed = 0;
                    //navAgent.angularSpeed = 0;

                    //update rotation
                    if (!staggered)
                    {
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
                                Attack();
                                hitTimer = meleeStartup;

                            }
                        }

                    }
                }
                break;
            case movementType.shoot:
                //DebugEx.Log("SHOOT");
                //stop navMesh movement and rotation
                if (canShoot && target.position.y >= transform.position.y)
                {
                    //navAgent.enabled = false;
                    navAgent.updateRotation = false;
                    navAgent.isStopped = true;
                    //navAgent.speed = 0;
                    //navAgent.angularSpeed = 0;

                    if (!midAttack)
                    {
                        //update rotation
                        Vector3 direction = (target.position - transform.position).normalized;
                        direction.y = 0;//prevents enemy from looking up

                        var targetRot = Quaternion.LookRotation(direction, Vector3.up);
                        //rb.MoveRotation(Quaternion.LookRotation(direction, Vector3.up));
                        rb.rotation = targetRot;
                        transform.rotation = targetRot;

                        Debug.Assert(!navAgent.updateRotation, "Agent is updating its rotation when it should be aiming!", this);
                        Debug.DrawRay(transform.position, direction * 5.0f, Color.red);
                        //shoot
                        if (firingTimer <= 0.0f)
                        {

                            if (!staggered)
                            {
                                Fire();

                                firingTimer = firingInterval;

                            }
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
            //shoot timer
            firingTimer -= Time.deltaTime;

            //if enemy is in the middle of its attack
            if (midAttack)
            {
                attackEndLagTimer = attackEndLagTimer - Time.deltaTime;
                //attack.skillData.activeHitBoxTimer = attack.skillData.activeHitBoxTimer - Time.deltaTime;
            }
            if (attackEndLagTimer <= 0)
            {
                midAttack = false;
                attackEndLagTimer = attack.skillData.activeHitBoxTimer + attackEndLag;

            }

            //stagger logic
            if (staggered)
            {
                staggerTimer -= Time.deltaTime;
            }
            if (staggered && staggerTimer <= 0)
            {
                staggerTimer = setStaggerTime;
                staggered = false;
            }
            //DebugEx.Log(staggerTimer);

            if (canHit || canShoot)
            {
                MeleeShootLogic();
            }

        }
        else
        {
            curMovement = movementType.idle;
            //DebugEx.Log("target out of sight");
        }
    }

    void MeleeShootLogic()
    {
        //if in shooting range
        if (canHit && target.position.x < transform.position.x + meleeRange && target.position.z < transform.position.z + meleeRange && target.position.x + meleeRange > transform.position.x && target.position.z + meleeRange > transform.position.z)
        {
            //DebugEx.Log("target in attack range");
            if (target.position.y >= transform.position.y)
            {
                curMovement = movementType.hit;

            }

        }
        else if (canShoot && target.position.x < transform.position.x + shootRange && target.position.z < transform.position.z + shootRange && target.position.x + shootRange > transform.position.x && target.position.z + shootRange > transform.position.z)
        {
            //if (canShoot)
            //{
            //Movement(distance);

            if(target.position.y >= transform.position.y)
            {
                curMovement = movementType.shoot;

            }
            //DebugEx.Log("moving towards target");

            // }
        }
        else
        {
            //Movement(distance);
            curMovement = movementType.move;
            //DebugEx.Log("moving towards target");
        }
    }

    private void Attack()
    {
        if (attack.skillData != null)
        {
            midAttack = true;
            playerKnockedBackTime = playerKnockedbackTimeSet;
            attack.Use(this.gameObject, meleeKnockback, playerKnockedBackTime);
            //midAttack = false;
        }
    }

    void Fire()
    {
        if (!staggered)
        {
            DebugEx.Log("Fire!");
            GameObject newBullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y + bulletSpawnY, transform.position.z), transform.rotation);
            Vector3 playerPositionCopy = target.position;
            playerPositionCopy.y = newBullet.transform.position.y;
            newBullet.transform.forward = (playerPositionCopy - newBullet.transform.position).normalized;

        }

    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shootRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, meleeRange);
    }
}
