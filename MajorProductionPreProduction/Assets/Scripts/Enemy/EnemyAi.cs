using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAi : MonoBehaviour
{
    public Agent agent;
    public Transform target;

    public float speed = 3.0f;

    public float range = 5;
    public float shootRange = 3.5f;

    public GameObject bulletPrefab;
    public float firingInterval = 1.0f;
    private float firingTimer;
    private Rigidbody rb;

    public NavMeshAgent navAgent;

    private enum movementType
    {
        move,
        shoot,
        idle
    }

    private movementType curMovement;

    private void Start()
    {
        curMovement = movementType.idle;
        rb = GetComponent<Rigidbody>();
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

                navAgent.SetDestination(target.position);
                break;
            case movementType.shoot:
                //DebugEx.Log("SHOOT");
                //stop navMesh movement and rotation
                navAgent.enabled = false;



                //update rotation
                Vector3 direction = (target.position - transform.position).normalized;
                rb.MoveRotation(Quaternion.LookRotation(direction, Vector3.up));

                //shoot
                if (firingTimer <= 0.0f)
                {
                    Attack();

                    firingTimer = firingInterval;
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
        else
        {
            curMovement = movementType.idle;
            //DebugEx.Log("target out of sight");
        }
    }

    void Movement(Vector3 distance)
    {
        agent.velocity = distance.normalized * speed;
        agent.UpdateMovement();
    }

    void Attack()
    {
        GameObject newBullet = Instantiate(bulletPrefab, new Vector3(transform.position.x, transform.position.y + .3f, transform.position.z), transform.rotation);
        Vector3 playerPositionCopy = target.position;
        playerPositionCopy.y = newBullet.transform.position.y;
        newBullet.transform.forward = (playerPositionCopy - newBullet.transform.position).normalized;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, range);

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, shootRange);
    }
}

