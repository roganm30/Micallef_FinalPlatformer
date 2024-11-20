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

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //The input from the player needs to be determined and then passed in the to the MovementUpdate which should
        //manage the actual movement of the character.
        Vector2 playerInput = new Vector2();
        MovementUpdate(playerInput);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        move = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(move * speed, rb.velocity.y);
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
        return true;
    }

    public FacingDirection GetFacingDirection()
    {
        return FacingDirection.left;
    }
}
