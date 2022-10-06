using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float enemySpeed;
    public AudioClip enemyDeathSound;

    public bool isDead;

    public Rigidbody2D myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (isDead)
        {
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
}
