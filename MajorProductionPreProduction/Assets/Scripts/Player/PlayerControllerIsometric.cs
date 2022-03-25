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
    public static bool invincibility { get; set;}
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
#pragma warning disable 0414
    private bool isGrounded = false;
    private Vector3 skinWidthSize;
    private float skinWidth = .001f;
    LayerMask mask;
    private float maxGroundAngle = 60f;
    Vector3 originalSize;
    public bool gravity;
    private float knockbackTimer;
    Vector3 projectedPosition;
    public bool staggered;
    public static float staggerTimer { get; set; }

    private enum movementType
    {
        walk,
        dash,
        fly,
        knockback,
        idle
    }

    private movementType lastmovementType;
    private float knockedbackAmount;
    private Vector3 directionKnockedback;
    private movementType curMovement;
    public float targetFlyPosY;
    //private bool flying;

    private void Start()
    {
        
        //gravity
        gravity = true;
        
        //initial rotation
        startRotation = rb.rotation;
        
        //dash timer
        timer = 0;

        //control over player
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
        yield return new WaitForSeconds(waitTime * 2);
        if (!canMove)
        {
            rb.rotation = startRotation;
        }
        yield return new WaitForSeconds(waitTime);
        Dialogue.canClick = true;
        //DebugEx.Log(canMove);
    }

    private void Update()
    {
        switch (curMovement)
        {
            case movementType.knockback:
                
                knockbackLogic(knockedbackAmount, directionKnockedback);
            break;

        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //stagger logic
        if (staggerTimer > 0)
        {
            staggered = true;
            canMove = false;
            staggerTimer -= Time.deltaTime;
            //DebugEx.Log(staggerTimer);
        }
        if (staggered && staggerTimer <= 0)
        {
            //staggerTimer = setStaggerTime;
            staggered = false;
            canMove = true;
        }


        //player input
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

        //gravity
        if (gravity)
        {
            if (canMove)
            {
                //                        gravity * fall speed * time.deltaTIme
                velocity += Physics.gravity * 2 * Time.deltaTime;

            }
        }

        

        if (Input.GetKey(KeyCode.Space))
        {
            curMovement = movementType.dash;
            timer = 0;
        }

        // switch
        // walking => projectedPosition
        // dashing => projectedPosition
        // other => projectedPosition

        switch (curMovement)
        {
            case movementType.walk:
                if (canMove)
                {
                    Movement(input);   
                }
                break;
            case movementType.dash:
                if (canMove)
                {
                    Dash();   

                }
                break;
            case movementType.fly:


                if(rb.position.y < targetFlyPosY)
                {
                    Vector3 targetPos = rb.position;
                    targetPos.y = targetFlyPosY;
                    Vector3 direction = (targetPos - rb.position).normalized;
                    projectedPosition.y = rb.position.y + (velocity.y + direction.y * moveSpeed) * Time.deltaTime;
                    Debug.DrawRay(transform.position, direction * 5);
                    //rb.position = targetPos;

                }
                else
                {
                    curMovement = movementType.walk;
                }
                break;
            
            case movementType.idle:
                projectedPosition = rb.position + (velocity) * Time.deltaTime;
                break;
        }


        //Camera/PlayerRotation
        if (canMove)
        {
            MouseRotation();

        }

        //Collider Logic

        thisCollider.size = skinWidthSize;
        //DebugEx.Log("thiscollider.size: " + thisCollider.size);

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
                //DebugEx.Log("Hit : " + hitColliders[i].name + i);

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
                    //DebugEx.Log("angle: " + angle);

                    if (angle < maxGroundAngle)
                    {
                        projectedPosition.y += direction.y * distance;
                        velocity = new Vector3(0f, velocity.y, 0f);

                        //DebugEx.Log("walkable slope");
                        isGrounded = true;



                    }
                    else//if slope too steep
                    {
                        projectedPosition += direction * distance;//pushes back player by direction times distance and adding to players current position
                                                                  //DebugEx.Log("too steep a slope");
                        isGrounded = false;
                    }

                }



            }

            thisCollider.size = originalSize;
            //DebugEx.Log("thiscollider.size: " + thisCollider.size);
           rb.MovePosition(projectedPosition);
        }
        

        Debug.DrawRay(transform.position, newDirection * 5);
        Debug.DrawRay(transform.position, directionKnockedback * 5);
    }

    private void knockbackLogic(float strength, Vector3 direction)
    {
        knockbackTimer = knockbackTimer + Time.deltaTime;
        projectedPosition = rb.position + (velocity + direction * strength) * Time.deltaTime;
        DebugEx.Log(knockbackTimer);
        DebugEx.Log("Target Time: " + EnemyAi.playerKnockedBackTime);
        if (knockbackTimer >= EnemyAi.playerKnockedBackTime)
        {
            
            knockbackTimer = 0;
            curMovement = lastmovementType;

        }
        
    }

    private void LateUpdate()
    {
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            gravity = !gravity;

            if (!gravity)
            {
                curMovement = movementType.fly;
            }

            //if (gravity)
            //{
            //    DebugEx.Log("walk");
            //    curMovement = movementType.walk;
            //}
            //else
            //{
            //    DebugEx.Log("fly");
            //    curMovement = movementType.fly;
            //}
        }
    }

    public void KnockBack(float strength, Vector3 direction)
    {
        lastmovementType = curMovement;

        knockedbackAmount = strength;
        directionKnockedback = direction;

        directionKnockedback.y = 0;

        curMovement = movementType.knockback;

        

    }

    private void Dash()
    {
        invincibility = true;
        timer = timer + Time.deltaTime;
        // apply the dash to player
        projectedPosition = rb.position + (velocity + newDirection * dashSpeed) * Time.deltaTime;
        //DebugEx.Log("timer:" + timer);

        if (timer >= dashTime)
        {
            invincibility = false;
            curMovement = movementType.walk;
            //enable player movement control
            //DebugEx.Log("stop");

        }
    }

    private void Movement(Vector3 input)
    {
        projectedPosition = rb.position + (velocity + input.normalized * moveSpeed) * Time.deltaTime;
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
        if (cam != null)
        {
            //get player elevation
            float height = transform.position.y;

            //create a plane at the players height on the XZ plane
            Plane playerPlane = new Plane(Vector3.up,// normal vector
                                            -height);//disatance from origin

            //create a ray from the players cursor shooting out from the camera
            Ray mouseRay = cam.ScreenPointToRay(Input.mousePosition);

            //raycast against that plane formed by the player
            playerPlane.Raycast(mouseRay, out float distance);

            //get intersection point between RAY and PLANE
            Vector3 hitPoint = mouseRay.origin + mouseRay.direction * distance;
            Debug.DrawRay(hitPoint, Vector3.up);

            //allign the hitpoint with the players height
            hitPoint.y = height;

            //look at it
            Vector3 direction = (hitPoint - transform.position).normalized;
            rb.MoveRotation(Quaternion.LookRotation(direction, Vector3.up));
        }
        else
        {
            DebugEx.Log("player is missing reference to camera");
        }

        

    }
}

//