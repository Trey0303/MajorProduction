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
    private bool canMove;
    private Vector3 lastDirection;
    private Vector3 newDirection;
    private float degree = 20;
    private float radian;
    private bool currentlyDashing;
    private float timer = 0;
    public float dashSpeed = 44f;

    //collider variables
    private BoxCollider thisCollider;
    private Vector3 velocity;
    public Rigidbody rb;
    private bool isGrounded;
    private Vector3 skinWidthSize;
    public float skinWidth = .001f;
    LayerMask mask;
    public float maxGroundAngle = 60f;
    Vector3 originalSize;
    public bool gravity = true;

    private void Start()
    {
        timer = 0;
        canMove = true;

        //Collider defaults
        rb = GetComponent<Rigidbody>();
        velocity = rb.velocity;
        mask = LayerMask.GetMask("Default");
        thisCollider = this.GetComponent<BoxCollider>();
        originalSize = thisCollider.size;
        skinWidthSize = new Vector3(thisCollider.size.x + skinWidth, thisCollider.size.y + skinWidth, thisCollider.size.z + skinWidth);
        isGrounded = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //player input
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));

        //gravity
        if (gravity)
        {
            velocity += Physics.gravity * Time.deltaTime;
        }

        //Collider Logic

        //projected position
        Vector3 projectedPosition = transform.position + (velocity + input * moveSpeed) * Time.deltaTime;

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
                Debug.Log("Hit : " + hitColliders[i].name + i);

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
                    velocity = Vector3.ProjectOnPlane(velocity, direction);

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


        }


        //movement

        if (canMove)
        {
            Movement(input);
            //deltaTime makes player move by 5 seconds instead of 5 frames
            
        }

        //Dash

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Dash();
        }

        //Camera/PlayerRotation
        MouseRotation();




        

        Debug.DrawRay(transform.position, newDirection * 5);
    }

    void Movement(Vector3 input)
    {
        transform.Translate(input * moveSpeed * Time.deltaTime, Space.World);
        if (input.magnitude != 0)
        {
            // current => lastDirection
            // target => input.normalized
            lastDirection = input.normalized;
            radian = degree * Mathf.Deg2Rad;
            newDirection = Vector3.RotateTowards(lastDirection, input.normalized * Time.deltaTime, radian, 0.0f);
        }

    }

    void MouseRotation()
    {
        Ray hit = cam.ScreenPointToRay(transform.position);
        transform.forward = (hit.direction - this.transform.position).normalized;

        ////Get the Screen positions of the object
        //Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        ////Get the Screen position of the mouse
        //Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        ////Get the angle between the points
        //float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        ////change player rotation
        //transform.rotation = Quaternion.Euler(new Vector3(0f, -angle, 0f));
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    void Dash()
    {
        if (!currentlyDashing)
        {
            StartCoroutine(DashRoutine());

        }
    }

    IEnumerator DashRoutine()
    {
        //Debug.Log("Target Time: " + dashTime);

        //disable player movement control
        canMove = false;
        currentlyDashing = true;

        // while dashing (we haven't dashed long enough)
        while(timer < dashTime)
        {
            // increment that timer
            timer = timer + .1f;

            // apply the dash to player
            transform.Translate(newDirection * Time.deltaTime * dashSpeed, Space.World);

            //Debug.Log("timer:" + timer);
            // wait until the next frame
            yield return new WaitForSecondsRealtime(Time.deltaTime);

        }

        yield return new WaitForSecondsRealtime(.15f);
        //enable player movement control
        canMove = true;
        currentlyDashing = false;
        timer = 0;
    }
}
