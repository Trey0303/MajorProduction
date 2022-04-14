using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerIsometric : MonoBehaviour
{
    [Header("Movement")]
    //player movement Speed
    public float moveSpeed = 10;

    public float dashTime = 1.45f;//how long evade takes
    //public float dashDistance = 10;//how far player with evade
    public float dashSpeed = 44f;
    public float dashCost = 10;
    public float flightCost;
    
    public float timeToWaitBeforeStaminaRegen = 1f;

    [Header("height in flight mode")]
    public float targetFlyPosY;//target y position in flight mode
    

    //programmer variables
    [Header("other/programmmer variables")]
    public Rigidbody rb;
    public Camera cam;
    private BoxCollider thisCollider;
    private Vector3 velocity;
    //collider variables
#pragma warning disable 0414
    private bool isGrounded = false;
    private bool dashing = false;
    public bool gravity;
    Vector3 projectedPosition;
    private Vector3 skinWidthSize;
    private float skinWidth = .001f;
    LayerMask mask;
    private float maxGroundAngle = 60f;
    Vector3 originalSize;
    //other
    public static bool canMove { get; set; }
    public static bool invincibility { get; set;}
    private Vector3 lastDirection;
    private Vector3 newDirection;
    private float degree = 20;
    private float radian;
    private bool currentlyDashing;
    private Quaternion startRotation;
    private float timer = 0;
    
    private float knockbackTimer;
    
    public bool staggered;//staggered state
    public static float staggerTimer { get; set; }

    public static bool startingDialogueActive { get; set; }

    public static int killcount { get; set; }

    public static float stamina { get; set; }

    private enum movementType
    {
        walk,
        dash,
        fly,
        boost,
        knockback,
        idle
    }

    private movementType lastmovementType;
    private float knockedbackAmount;
    private Vector3 directionKnockedback;
    private movementType curMovement;
    public bool canToggleFlight;
    private float lastwalkableTerrainPosY;
    public bool flightMode;
    public bool canDash;
    public bool isAtFlightHeight;

    //private bool flying;

    private void Start()
    {
        canDash = true;

        killcount = 0;

        startingDialogueActive = false;
        
        //gravity
        gravity = true;
        
        //initial rotation
        startRotation = rb.rotation;
        
        //dash timer
        timer = 0;

        //control over player
        canMove = true;

        //stagger state
        staggered = false;

        //Collider defaults
        rb = GetComponent<Rigidbody>();
        velocity = rb.velocity;
        mask = LayerMask.GetMask("Default", "Enemy");
        thisCollider = this.GetComponent<BoxCollider>();
        originalSize = thisCollider.size;
        skinWidthSize = new Vector3(thisCollider.size.x + skinWidth, thisCollider.size.y + skinWidth, thisCollider.size.z + skinWidth);
        isGrounded = false;
        projectedPosition = rb.position;//prevents player from teleporting to position 0,0,0
        //DebugEx.Log("Start player Coroutine");
        StartCoroutine(LateStart(.1f));
    }

    IEnumerator LateStart(float waitTime)
    {
        //DebugEx.Log("yield player: " + waitTime * 2);
        yield return new WaitForSeconds(waitTime * 2);
        //DebugEx.Log("adjust rotation");
        if (!canMove || startingDialogueActive)
        {
            rb.rotation = startRotation;
        }
        //DebugEx.Log("yield player: " + waitTime);
        yield return new WaitForSeconds(waitTime);
        //DebugEx.Log("can click");
        Dialogue.canClick = true;
        canMove = false;
        
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


        //if (Input.GetKeyUp(KeyCode.R))
        //{
        //    int randomNum = UnityEngine.Random.Range(3, 6);
        //    DebugEx.Log(randomNum);
        //}


        if (Input.GetKeyUp(KeyCode.K)){
            PlayerHealth.curHealth = 0;
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && stamina >= flightCost)
        {
            if (canMove && canToggleFlight)
            {
                flightMode = !flightMode;
                isAtFlightHeight = false;
                canDash = false;
                canToggleFlight = false;
                gravity = !gravity;

                if (!gravity)
                {

                    curMovement = movementType.fly;
                }
                else
                {
                    curMovement = movementType.walk;
                }

            }
        }

        if (Input.GetKey(KeyCode.Space) && stamina >= flightCost && rb.position.y == targetFlyPosY)
        {
            if (canDash)
            {
                curMovement = movementType.boost;
                timer = 0;
            }
        }

        //if(Input.GetKeyUp(KeyCode.LeftShift) && /*is in flight mode*/)
        //{
        //    dashing = false;
        //}

        if (Input.GetKeyUp(KeyCode.Space) && stamina >= dashCost && isGrounded)
        {
            if (canDash)//TODO: ADD another condition for checking if grounded
            {
                curMovement = movementType.dash;
                timer = 0;
                //stamina = stamina - dashCost;

            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //flight mode toggle conditions
        if (isGrounded && !canToggleFlight && !flightMode)//Currently in ground mode
        {
            //FlightMode = false;
            canToggleFlight = true;
            canDash = true;
        }
        else if (rb.position.y == targetFlyPosY && !canToggleFlight && flightMode)//currently in Flight mode
        {
            //DebugEx.Log(isGrounded);
            isGrounded = false;
            //FlightMode = true;
            canToggleFlight = true;
            canDash = true;
        }
        //else if(!isGrounded || !isAtFlightHeight)
        //{
        //    //DebugEx.Log("player currently switching mode");
        //    //canToggleFlight = false;
        //}

        //starting dialogue check
        if (startingDialogueActive && canMove)
        {
            canMove = false;
            rb.rotation = startRotation;

        }
        //stagger logic
        if (staggerTimer > 0)
        {
            staggered = true;
            canMove = false;
            staggerTimer -= Time.deltaTime;
            //DebugEx.Log(staggerTimer);
            //DebugEx.Log(staggered);
        }
        if (staggered && staggerTimer <= 0)
        {
            //staggerTimer = setStaggerTime;
            staggered = false;
            canMove = true;
            //DebugEx.Log(staggered);
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

        // switch
        // walking => projectedPosition
        // dashing => projectedPosition
        // other => projectedPosition

        switch (curMovement)
        {
            case movementType.walk:
                if (canMove)
                {
                    if (isAtFlightHeight)
                    {
                        PlayerHealth.RegenWaitTimer = timeToWaitBeforeStaminaRegen;
                        stamina = stamina - flightCost;
                    }
                    if(flightCost > stamina)
                    {
                        isAtFlightHeight = false;
                        flightMode = false;
                        gravity = true;
                    }

                    Movement(input);   
                }
                break;
            case movementType.dash:
                if (canMove && canDash)
                {
                    PlayerHealth.RegenWaitTimer = timeToWaitBeforeStaminaRegen;
                    stamina = stamina - dashCost;
                    Dash();   

                }
                break;
            case movementType.fly:

                if (canMove)
                {
                    
                    //canToggleFlight = false;
                    if(rb.position.y > targetFlyPosY)
                    {
                        projectedPosition.y = targetFlyPosY;
                    }
                    if (rb.position.y < targetFlyPosY)
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
                        if (canMove)
                        {
                            isAtFlightHeight = true;
                            curMovement = movementType.walk;

                        }
                    }
                }
                break;
            case movementType.boost:
                if (canMove && canDash)
                {
                    Boost();

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

                    if (!dashing)
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
                            //lastwalkableTerrainPosY = rb.position.y;
                            //DebugEx.Log(lastwalkableTerrainPosY);

                        }
                        else//if slope too steep
                        {
                            projectedPosition += direction * distance;//pushes back player by direction times distance and adding to players current position
                                                                      //DebugEx.Log("too steep a slope");
                            isGrounded = false;
                        }

                    }
                    else if (dashing && hitColliders[i].gameObject.tag != "Enemy")
                    {
                        //DebugEx.Log("dashing");
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
                            //lastwalkableTerrainPosY = rb.position.y;
                            //DebugEx.Log(lastwalkableTerrainPosY);

                        }
                        else//if slope too steep
                        {
                            projectedPosition += direction * distance;//pushes back player by direction times distance and adding to players current position
                                                                      //DebugEx.Log("too steep a slope");
                            isGrounded = false;
                        }
                    }
                    else
                    {
                        DebugEx.Log("dashed through enemy");

                    }

                }
            }

            thisCollider.size = originalSize;
            //DebugEx.Log("thiscollider.size: " + thisCollider.size);
            rb.MovePosition(projectedPosition);
            //DebugEx.Log(isGrounded);
        }
        

        Debug.DrawRay(transform.position, newDirection * 5);
        Debug.DrawRay(transform.position, directionKnockedback.normalized * 5);
    }

    private void knockbackLogic(float strength, Vector3 direction)
    {
        //Old
        //knockbackTimer = knockbackTimer + Time.deltaTime;
        //projectedPosition = rb.position + (velocity + direction.normalized * strength) * Time.deltaTime;
        ////DebugEx.Log(knockbackTimer);
        ////DebugEx.Log("Target Time: " + EnemyAi.playerKnockedBackTime);
        //if (knockbackTimer >= EnemyAi.playerKnockedBackTime || knockbackTimer >= BossAI.playerKnockedBackTime/*PROBLEM HERE!!!!!*/)
        //{
            
        //    knockbackTimer = 0;
        //    curMovement = lastmovementType;

        //}

        //New
        knockbackTimer = knockbackTimer - Time.deltaTime;
        projectedPosition = rb.position + (velocity + direction.normalized * strength) * Time.deltaTime;

        if (knockbackTimer <= 0)
        {
            curMovement = lastmovementType;
        }
        
    }

    public void Knockback(float strength, float time, Vector3 direction)
    {
        lastmovementType = curMovement;

        knockedbackAmount = strength;
        directionKnockedback = direction;

        directionKnockedback.y = 0;

        curMovement = movementType.knockback;

        knockbackTimer = time;

        

    }

    private void Dash()
    {
        invincibility = true;
        dashing = true;
        timer = timer + Time.deltaTime;
        // apply the dash to player
        projectedPosition = rb.position + (velocity + newDirection * dashSpeed) * Time.deltaTime;
        //DebugEx.Log("timer:" + timer);

        if (timer >= dashTime)
        {
            invincibility = false;
            curMovement = movementType.walk;
            dashing = false;
            //enable player movement control
            //DebugEx.Log("stop");

        }
    }

    private void Boost()
    {
        //TODO: fix rotation issue

        //invincibility = true;
        dashing = true;
        timer = timer + Time.deltaTime;
        // apply the dash to player
        projectedPosition = rb.position + (velocity + newDirection * dashSpeed) * Time.deltaTime;
        //DebugEx.Log("timer:" + timer);

        if (timer >= dashTime)
        {
            invincibility = false;
            curMovement = movementType.walk;
            dashing = false;
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