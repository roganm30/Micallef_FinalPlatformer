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


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.
        Vector2 playerInput = new Vector2();
        MovementUpdate(playerInput);
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
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(2, 2), 180f, -transform.up, 0.67f);

        // RaycastHit2D hit = Physics2D.Raycast(transform.position, -transform.up, 0.67f);
        if (hit)
        {
            Debug.Log("Hit Something : " + hit.collider.name);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                rb.AddForce(new Vector2(rb.velocity.x, initialJumpVelocity * 7f));
            }

            return true;
        }
        else
        {
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
