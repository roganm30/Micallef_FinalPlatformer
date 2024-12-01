using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum FacingDirection
    {
        left, right
    }

    public float speed = 5f;
    public float move;

    private Rigidbody2D rb;

    public float jump;


    public float apexHeight;
    public float apexTime;

    public float gravity;
    public float initialJumpVelocity;


    public float posTerminalSpeed = 10f;
    public float negTerminalSpeed = -10f;


    public float coyoteTime = 0.5f;

    public float dashSpeed;

    public float jumpDelay = 1;
    float timer;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime; // timer variable adding over time passing
    }

    private void FixedUpdate()
    {
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.
        Vector2 playerInput = new Vector2();
        MovementUpdate(playerInput);

        Debug.Log(timer);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        move = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(move * speed * Time.fixedDeltaTime, rb.velocity.y);

        gravity = -2 * apexHeight / Mathf.Pow(apexTime, 2);
        initialJumpVelocity = 2 * (apexHeight / apexTime);

        rb.AddForce(new Vector2(0, gravity));

        if (rb.velocity.y >= posTerminalSpeed || rb.velocity.y <= negTerminalSpeed)
        {
            gravity = -9.8f;
        }

        //Debug.Log("positive terminal speed is : " + posTerminalSpeed);

        //Debug.Log("negative terminal speed is : " + negTerminalSpeed);

        //Debug.Log(rb.velocity.y);

        //Debug.Log(gravity);

        // task 1: dash mechanic
        if (Input.GetKey(KeyCode.LeftShift) && move != 0) // if the left shift key is pressed AND the move value does not equal 0
        {
            rb.velocity = new Vector2(move * speed * dashSpeed * Time.fixedDeltaTime, rb.velocity.y); // takes the movement code i used previously and
                                                                                                      // added the multiplication of dashSpeed
                                                                                                      // this is flawed right now because the player can
                                                                                                      // hold it indefinitely :,D

            //Debug.Log("shift key was hit");
        }
    }

    public bool IsWalking()
    {
        if (move != 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsGrounded()
    {
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(1, 1), 0f, -transform.up, 0.67f);

        // RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 0.67f);
        if (hit)
        {
            Debug.Log("Hit Something : " + hit.collider.name);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(new Vector2(0, initialJumpVelocity * 7f));
            }

            timer = 0; // if the player/boxcast hits the ground, timer is reset to 0

            return true;
        }
        else
        {
            // task 2: quick descent
            if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)) // this if statement is placed within the else of the raycast being hit
                                                                            // this is so that it will only occur if IsGrounded is being marked as false
                                                                            // it is basically saying that as long as the DownArrow or S keys are held
                                                                            // the if statement will occur
            {
                rb.AddForce(new Vector2(0, -1)); // the if statement is giving the player character a bit of a push by adding force
                                                 // the force is (0, -1) because it is adjusting the y position (x, y) and it is pushing down (negative)
                                                 // if the force were to be (0, 1) then the player would start to fly away upward
            }

            // task 3 ver.2: rocket boots
            if (Input.GetKeyDown(KeyCode.Space) && timer < jumpDelay) // the jump delay is set to 1, this code will only run if both conditions are met
                                                                      // the space bar is pressed, and the timer (which counts real time) is under 1
                                                                      // because the timer resets when IsGrounded is true, this forces it to be a quick
                                                                      // double jump mechanic rather than something that can be spammed
            {
                rb.AddForce(new Vector2(0, initialJumpVelocity * 5f)); // when these conditions are met, force is added to the player
                                                                       // much like the initial jump mechanic, but a bit weaker to feel more natural
            }

            return false;
        }

    }

    public FacingDirection GetFacingDirection()
    {
        if (move < 0)
        {
            return FacingDirection.left;
        }
        else
        {
            return FacingDirection.right;
        }
        
    }
}
