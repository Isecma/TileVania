using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowSpeed;
    [SerializeField] AudioClip arrowSound;

    float playerDirection;
    bool hasStopped;
    PlayerMovement playerMovement;
    EnemyMovement enemyMovement;
    Animator animator;
    Animator enemyAnimator;
    Rigidbody2D myRigidbody;
    Rigidbody2D enemyRigidbody;

    void Start()
    {
        AudioSource.PlayClipAtPoint(arrowSound, transform.position);
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
        Destroy(gameObject, 5);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemies")
        {
            enemyMovement = collision.gameObject.GetComponent<EnemyMovement>();
            enemyMovement.isDead = true;
            AudioSource.PlayClipAtPoint(enemyMovement.enemyDeathSound, transform.position);
            enemyAnimator = collision.gameObject.GetComponent<Animator>();
            enemyAnimator.SetTrigger("OnDeath");
            enemyRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
            enemyRigidbody.velocity = new Vector2(0, 0);       
            Destroy(collision.gameObject, 0.3f);
            Destroy(gameObject);
        }
        hasStopped = true;
        animator.StartPlayback();
        myRigidbody.velocity = new Vector2(0f, 0f);
        Destroy(gameObject, 3);
    }
}
