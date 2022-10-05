using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float enemySpeed;

    [HideInInspector] public bool isDead;

    Animator animator;
    Rigidbody2D myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (isDead) 
        {
            StartCoroutine(DestroyEnemy());
            return;
        }

        myRigidbody.velocity = new Vector2 (enemySpeed, 0);
    }


    void OnTriggerExit2D(Collider2D collision)
    {
        enemySpeed = -enemySpeed;
        FlipEnemyFacing();
    }

    void FlipEnemyFacing()
    {
        if (isDead) { return; }
        transform.localScale = new Vector2(-(myRigidbody.velocity.x), 1f);
    }

    IEnumerator DestroyEnemy()
    {
        myRigidbody.velocity = new Vector2(0, 0);
        animator.SetTrigger("OnDeath");
        yield return new WaitForSecondsRealtime(0.3f);
        Destroy(gameObject);
    }
}
