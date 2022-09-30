using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 1;
    [SerializeField] float jumpSpeed = 1;
    
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator animator;
    int groundMask;

    int numberOfJumps = 2;
    

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        groundMask = LayerMask.GetMask("Ground");
    }


    void Update()
    {
        Run();
        FlipSprite();
    }

    void Run()
    {
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
}
