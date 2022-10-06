using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed;
    [SerializeField] float jumpSpeed;
    [SerializeField] float climbSpeed;
    [SerializeField] float flingSpeed;
    [SerializeField] GameObject bow;
    [SerializeField] GameObject arrow;
    [SerializeField] AudioClip hitSFX;
    [SerializeField] AudioClip bounceSFX;

    Vector2 moveInput;
    CinemachineVirtualCamera deathCamera;
    Rigidbody2D myRigidbody;
    Animator animator;
    CapsuleCollider2D bodyCollider;
    BoxCollider2D feetCollider;

    int groundMask;
    int bouncingMask;
    int ladderMask;
    int enemyMask;
    int hazardMask;
    float defaultGravity;

    bool isAlive = true;
    int numberOfJumps = 2;
    

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        bow = GameObject.Find("Bow");

        deathCamera = GameObject.Find("Death Camera").GetComponent<CinemachineVirtualCamera>();
        groundMask = LayerMask.GetMask("Ground");
        bouncingMask = LayerMask.GetMask("Bouncing");
        ladderMask = LayerMask.GetMask("Ladder");
        enemyMask = LayerMask.GetMask("Enemies");
        hazardMask = LayerMask.GetMask("Hazards");
        defaultGravity = myRigidbody.gravityScale;
    }


    void Update()
    {
        if (!isAlive) { return;}
        Move();
        FlipSprite();
        ClimbLadder();
        Die();
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
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (value.isPressed && numberOfJumps > 0)
        {
            myRigidbody.velocity += new Vector2(0f, jumpSpeed);
            numberOfJumps--;
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }

        animator.SetTrigger("OnAttack");
        Vector2 bowPosition = new Vector2 (bow.transform.position.x, bow.transform.position.y);
        Instantiate(arrow, bowPosition, transform.rotation);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.otherCollider.GetComponent<BoxCollider2D>().IsTouchingLayers(groundMask) || collision.otherCollider.GetComponent<BoxCollider2D>().IsTouchingLayers(bouncingMask))
        {
            numberOfJumps = 2;
        }
        if (collision.otherCollider.GetComponent<BoxCollider2D>().IsTouchingLayers(bouncingMask))
        {
            AudioSource.PlayClipAtPoint(bounceSFX, transform.position);
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
            animator.SetBool("IsRunning", false);
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

    void Die()
    {
        if (!isAlive) { return; }

        if (bodyCollider.IsTouchingLayers(enemyMask) || myRigidbody.IsTouchingLayers(hazardMask))
        {
            AudioSource.PlayClipAtPoint(hitSFX, transform.position);
            isAlive = false;
            animator.SetTrigger("OnDeath");
            bodyCollider.enabled = false;
            feetCollider.enabled = false;
            deathCamera.transform.position = new Vector3(myRigidbody.position.x, myRigidbody.position.y, -1);
            Vector2 playerFling = new Vector2(-(Mathf.Sign(moveInput.x)), flingSpeed);
            myRigidbody.velocity = playerFling;
            FindObjectOfType<GameSession>().ProcessPlayerDeath();
        } 
    }

}
