using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip coinSFX;
    public int coinValue = 1;

    void OnTriggerEnter2D(Collider2D collision)
    {
        AudioSource.PlayClipAtPoint(coinSFX, transform.position);
        Destroy(gameObject);
        FindObjectOfType<GameSession>().IncreaseScore(coinValue);
    }
}
