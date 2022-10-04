using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] float arrowSpeed;

    PlayerMovement playerMovement;
    float playerDirection;
    Rigidbody2D myRigidbody;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        playerDirection = playerMovement.transform.localScale.x;
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2(playerDirection * arrowSpeed, 0f);   
        myRigidbody.transform.localScale = playerMovement.transform.localScale;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemies")
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }
}
