using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public SkillProgress attack;

    public Agent agent;
    public Transform target;

    public float speed = 3.0f;

    public float range = 5;
    public float shootRange = 3.5f;

    public GameObject bulletPrefab;
    public float firingInterval = 1.0f;
    private float firingTimer;
    private Rigidbody rb;
    
    public float staggerTimer;
    public float setStaggerTime = 1;

    public NavMeshAgent navAgent;

    public bool staggered = false;

    private enum movementType
    {
        move,
        hit,
        shoot,
        idle
    }

    private movementType curMovement;
    public bool canShoot;
    public bool canHit;

    private void Start()
    {
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
                navAgent.enabled = false;

                break;
            case movementType.move:
                //DebugEx.Log("MOVE");
                navAgent.enabled = true;
                if (!staggered)
                {
                    navAgent.SetDestination(new Vector3(target.position.x, 0/*prevents enemy from looking up*/, target.position.z));

                }
                break;
            case movementType.hit:
                if (canHit)
                {
                    //update rotation
                    Vector3 direction = (target.position - transform.position).normalized;
                    direction.y = 0;//prevents enemy from looking up
                    rb.MoveRotation(Quaternion.LookRotation(direction, Vector3.up));

                    if (!staggered)
                    {
                        Attack();

                        firingTimer = firingInterval;

                    }
                }
                break;
            case movementType.shoot:
                //DebugEx.Log("SHOOT");
                //stop navMesh movement and rotation
                if (canShoot)
                {
                    navAgent.enabled = false;

                    //update rotation
                    Vector3 direction = (target.position - transform.position).normalized;
                    direction.y = 0;//prevents enemy from looking up
                    rb.MoveRotation(Quaternion.LookRotation(direction, Vector3.up));

                    //shoot
                    if (firingTimer <= 0.0f)
                    {
                    
                        if(!staggered)
                        {
                            Fire();

                            firingTimer = firingInterval;

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

            if (canHit)//TODO: add switch for melee attacks 
            {
                MeleeLogic();
            }
            if (canShoot)
            {
                ShootLogic();
            }

            

        }
        else
        {
            curMovement = movementType.idle;
            //DebugEx.Log("target out of sight");
        }
    }

    private void Attack()
    {
        attack.Use(this.gameObject);
    }

    void ShootLogic()
    {
        //if in shooting range
        if (target.position.x < transform.position.x + shootRange && target.position.z < transform.position.z + shootRange && target.position.x + shootRange > transform.position.x && target.position.z + shootRange > transform.position.z)
        {
            //DebugEx.Log("target in attack range");
            curMovement = movementType.shoot;

        }
        else
        {
            //Movement(distance);
            curMovement = movementType.move;
            //DebugEx.Log("moving towards target");
        }
    }


    void Fire()
    {
        if (!staggered)
        {
            GameObject newBullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z), transform.rotation);
            Vector3 playerPositionCopy = target.position;
            playerPositionCopy.y = newBullet.transform.position.y;
            newBullet.transform.forward = (playerPositionCopy - newBullet.transform.position).normalized;

        }

    }

    //void Movement(Vector3 distance)
    //{
    //    agent.velocity = distance.normalized * speed;
    //    agent.UpdateMovement();
    //}
    //void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, range);

    //    Gizmos.color = Color.blue;
    //    Gizmos.DrawSphere(transform.position, shootRange);
    //}
}

