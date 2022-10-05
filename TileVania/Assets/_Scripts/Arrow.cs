using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowSpeed;

    float playerDirection;
    bool hasStopped;
    PlayerMovement playerMovement;
    Animator animator;
    Rigidbody2D myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        animator = GetComponent<Animator>();
        playerDirection = playerMovement.transform.localScale.x;
    }

    void Update()
    {
        if (hasStopped) { return; }
        myRigidbody.velocity = new Vector2(playerDirection * arrowSpeed, 0f);   
        myRigidbody.transform.localScale = playerMovement.transform.localScale;
        Invoke("DestroyArrow", 5);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemies")
        {
            Destroy(collision.gameObject);
            DestroyArrow();
        }
        hasStopped = true;
        animator.StartPlayback();
        myRigidbody.velocity = new Vector2(0f, 0f);
        Invoke("DestroyArrow", 3);
    }

    void DestroyArrow()
    {
        Destroy(gameObject);
    }
}
