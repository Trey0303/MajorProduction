using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerIsometric : MonoBehaviour
{
    public Camera cam;

    //player movement Speed
    public float moveSpeed = 10;

    public float dashTime = 1.45f;//how long evade takes
    //public float dashDistance = 10;//how far player with evade
    public static bool canMove { get; set; }
    private Vector3 lastDirection;
    private Vector3 newDirection;
    private float degree = 20;
    private float radian;
    private bool currentlyDashing;
    private Quaternion startRotation;
    private float timer = 0;
    public float dashSpeed = 44f;

    //collider variables
    private BoxCollider thisCollider;
    private Vector3 velocity;
    public Rigidbody rb;
    private bool isGrounded = false;
    private Vector3 skinWidthSize;
    private float skinWidth = .001f;
    LayerMask mask;
    private float maxGroundAngle = 60f;
    Vector3 originalSize;
    public bool gravity = true;

    Vector3 projectedPosition;

    private enum movementType
    {
        walk,
        dash,
        idle
    }

    private movementType curMovement;

    private void Start()
    {
        startRotation = rb.rotation;
        timer = 0;
        canMove = true;

        //Collider defaults
        rb = GetComponent<Rigidbody>();
        velocity = rb.velocity;
        mask = LayerMask.GetMask("Default", "Enemy");
        thisCollider = this.GetComponent<BoxCollider>();
        originalSize = thisCollider.size;
        skinWidthSize = new Vector3(thisCollider.size.x + skinWidth, thisCollider.size.y + skinWidth, thisCollider.size.z + skinWidth);
        isGrounded = false;
        StartCoroutine(LateStart(.1f));
    }

    IEnumerator LateStart(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (!canMove)
        {
            rb.rotation = startRotation;
        }
        //Debug.Log(canMove);
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        if (!currentlyDashing)
    //        {
    //            //Dash(input);
    //            canMove = false;
    //            currentlyDashing = true;

    //            // while dashing (we haven't dashed long enough)
    //            //Debug.Log("start");

    //        }
    //    }
        
    //}

    // Update is called once per frame
    void FixedUpdate()
    {
        //player input
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

        //gravity
        if (gravity)
        {
            //                        gravity * fall speed * time.deltaTIme
            velocity += Physics.gravity * 2 * Time.deltaTime;
        }

        // switch
        // walking => projectedPosition
        // dashing => projectedPosition
        // other => projectedPosition

        if (Input.GetKey(KeyCode.Space))
        {
            curMovement = movementType.dash;
            timer = 0;
        }

        switch (curMovement)
        {
            case movementType.walk:
                if (canMove)
                {
                    Movement(input);   

                }

                break;
            case movementType.dash:
                Dash();   
                break;
            case movementType.idle:
                projectedPosition = rb.position + (velocity) * Time.deltaTime;
                break;
        }


        //projected position


        // ground movement logic

        // normal velocity
        //else//other/idle
        // {

        //}


        //Camera/PlayerRotation
        if (canMove)
        {
            MouseRotation();

        }

        //Collider Logic

        thisCollider.size = skinWidthSize;
        //Debug.Log("thiscollider.size: " + thisCollider.size);

        if (thisCollider != null)
        {

            //collide/overlap Detection                                              Skin Width: Add this skin width when performing your collision tests. For example, if my KinematicCharacter has a BoxCollider that has a size of 1,1,1, then I should be testing with a box whose full-extents are 1.001, 1.001, 1.001 when calling Physics.OverlapBox.
            Collider[] hitColliders = Physics.OverlapBox(projectedPosition, skinWidthSize / 2, Quaternion.identity, mask);//Remember that Physics.OverlapBox asks for the half-extents so you'll need to divide the box's size by two before passing it along.



            //Check when there is a new collider coming into contact with the box
            for (int i = 0; i < hitColliders.Length; i++)
            {
                if (hitColliders[i] == thisCollider)
                {
                    continue;
                }

                //Output all of the collider names
                //Debug.Log("Hit : " + hitColliders[i].name + i);

                Vector3 otherPosition = hitColliders[i].transform.position;//gets objects position
                Quaternion otherRotation = hitColliders[i].transform.rotation;//gets objects rotation

                float distance;
                Vector3 direction;

                //ComputePenetration works as a bool to know when the player is or isnt inside an object. Also give you the distance and direction between player and object
                //                                           object A collider, object A position, object A rotation, object b collider, object b position, object b rotation, MTV(minimum translation vector)
                bool overlapped = Physics.ComputePenetration(thisCollider, projectedPosition, transform.rotation, hitColliders[i], otherPosition, otherRotation, out direction, out distance);

                //if player is gonna overlap with object
                if (overlapped)
                {

                    //                               (vector, planeNormal)
                    velocity = Vector3.ProjectOnPlane(velocity, direction * 2);

                    float angle = Vector3.Angle(direction, Vector3.up);
                    //Debug.Log("angle: " + angle);

                    if (angle < maxGroundAngle)
                    {
                        projectedPosition.y += direction.y * distance;
                        velocity = new Vector3(0f, velocity.y, 0f);

                        //Debug.Log("walkable slope");
                        isGrounded = true;



                    }
                    else//if slope too steep
                    {
                        projectedPosition += direction * distance;//pushes back player by direction times distance and adding to players current position
                                                                  //Debug.Log("too steep a slope");
                        isGrounded = false;
                    }

                }



            }

            thisCollider.size = originalSize;
            //Debug.Log("thiscollider.size: " + thisCollider.size);
           rb.MovePosition(projectedPosition);
        }
        

        Debug.DrawRay(transform.position, newDirection * 5);
    }

    private void Dash()
    {
        timer = timer + Time.deltaTime;
        // apply the dash to player
        projectedPosition = rb.position + (velocity + newDirection * dashSpeed) * Time.deltaTime;
        //Debug.Log("timer:" + timer);

        if (timer >= dashTime)
        {
            curMovement = movementType.walk;
            //enable player movement control
            //Debug.Log("stop");

        }
    }

    private void Movement(Vector3 input)
    {
        projectedPosition = rb.position + (velocity + input * moveSpeed) * Time.deltaTime;
        //Movement(projectedPosition);

        if (input.magnitude != 0)
        {
            lastDirection = input.normalized;
            radian = degree * Mathf.Deg2Rad;
            newDirection = Vector3.RotateTowards(lastDirection, input.normalized * Time.deltaTime, radian, 0.0f);

        }
    }

    void MouseRotation()
    {
        ////Get the Screen positions of the object
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        ////Get the Screen position of the mouse
        if (Physics.Raycast(ray, out hit))
        {
            Vector3 targetPoint = hit.point;
            targetPoint.y = transform.position.y;

            
            ///Get the angle between the points
            Vector3 direction = (targetPoint - transform.position).normalized;
            ///change player rotation
            rb.MoveRotation(Quaternion.LookRotation(direction, Vector3.up));

        }
    }
}
