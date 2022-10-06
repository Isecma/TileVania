using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float enemySpeed;
    [SerializeField] GameObject coin;
    public AudioClip enemyDeathSFX;

    public bool isDead;
    public bool hasDroppedCoin;

    public Rigidbody2D myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
    }


    void Update()
    {
        if (isDead)
        {
            if (!hasDroppedCoin)
            {
                DropCoin();
                hasDroppedCoin = true;
            }
            
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
        transform.localScale = new Vector2(-(myRigidbody.velocity.x), 1f);
    }

    void DropCoin()
    {
        Vector2 coinDropPosition = new Vector2(transform.position.x, transform.position.y);
        Instantiate(coin, coinDropPosition, transform.rotation);
    }
}
