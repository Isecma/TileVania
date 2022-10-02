using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float climbSpeed;
        
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator animator;
    int groundMask;
    int ladderMask;
    float defaultGravity;


    int numberOfJumps = 2;
    

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundMask = LayerMask.GetMask("Ground");
        ladderMask = LayerMask.GetMask("Ladder");
        defaultGravity = myRigidbody.gravityScale;
    }


    void Update()
    {
        Move();
        FlipSprite();
        ClimbLadder();
    }

    void Move()
    {
        animator.StopPlayback();
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;

        if (Mathf.Abs(moveInput.x) > 0)
        {
            animator.SetBool("IsRunning", true);
        }
        else if (moveInput.x == 0)
        {
            animator.SetBool("IsRunning", false);
        }

    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {

        if (value.isPressed && numberOfJumps > 0)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
            numberOfJumps--;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
     if (collision.otherCollider.IsTouchingLayers(groundMask))
        {
            numberOfJumps = 2;
        } 

    }


    void FlipSprite()
    {
        bool playerHasHorizontalSpeed = Mathf.Abs(moveInput.x) > Mathf.Epsilon;

        if (playerHasHorizontalSpeed)
        {
            transform.localScale = new Vector2(Mathf.Sign(moveInput.x), 1f);
        }
    }


    void ClimbLadder()
    {
        Vector2 playerClimbingVelocity;
        if (!myRigidbody.IsTouchingLayers(ladderMask))
        {
            animator.SetBool("IsClimbing", false);
            myRigidbody.gravityScale = defaultGravity;
            return;
        }

        if (Mathf.Abs(moveInput.y) > 0f)
        {
            animator.StopPlayback();
            animator.SetBool("IsClimbing", true);
            playerClimbingVelocity = new Vector2(moveInput.x, (moveInput.y) * climbSpeed);
            myRigidbody.velocity = playerClimbingVelocity;
        }
        else if (moveInput.y == 0f && animator.GetBool("IsClimbing"))
        {
            myRigidbody.gravityScale = 0f;
            playerClimbingVelocity = new Vector2(moveInput.x, (moveInput.y) * climbSpeed);
            myRigidbody.velocity = playerClimbingVelocity;
            animator.StartPlayback();
        }
        else if (moveInput.y == 0f && !animator.GetBool("IsClimbing"))
        {
            myRigidbody.gravityScale = defaultGravity;
            playerClimbingVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
            myRigidbody.velocity = playerClimbingVelocity;
        }
        
    }
}
