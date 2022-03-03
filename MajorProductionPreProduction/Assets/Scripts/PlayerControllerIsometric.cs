using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerIsometric : MonoBehaviour
{
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

    private void Start()
    {
        timer = 0;
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        //movement

        if (canMove)
        {
            Movement();

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

    void Movement()
    {
        Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0.0f, Input.GetAxisRaw("Vertical"));
        //deltaTime makes player move by 5 seconds instead of 5 frames
        transform.Translate(input * moveSpeed * Time.deltaTime, Space.World);
        if(input.magnitude != 0)
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
        //Get the Screen positions of the object
        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(transform.position);

        //Get the Screen position of the mouse
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);

        //Get the angle between the points
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        //change player rotation
        transform.rotation = Quaternion.Euler(new Vector3(0f, -angle, 0f));
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
