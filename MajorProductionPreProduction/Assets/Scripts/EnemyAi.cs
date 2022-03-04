using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAi : MonoBehaviour
{
    public Agent agent;
    public Transform target;

    public float speed = 3.0f;

    public float range = 5;

    // Update is called once per frame
    void LateUpdate()
    {
        //calculate the difference between your target location and current location
        //(this give you an offset from your position to your target)
        Vector3 distance = target.position - transform.position;
        distance.y = 0.0f;

        if (target.position.x < transform.position.x + range && target.position.z < transform.position.z + range && target.position.x + range > transform.position.x && target.position.z + range > transform.position.z)
        {
            //Debug.Log("Distance to other : " + distance);

            //Normalize the difference
            //(This reduces the length of the offset to 1(aka unit length))

            //Scale the difference by the speed you want to move at
            agent.velocity = distance.normalized * speed;
            agent.UpdateMovement();

        }
    }
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawSphere(transform.position, range);
    //}
}

