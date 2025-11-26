using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPickUp : MonoBehaviour
{
    PlayerHealthManager playerHealthManager;
    public int amountOfHealing = 2;
    public GameObject player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealthManager = player.GetComponent<PlayerHealthManager>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("got triggered");
        if (other.CompareTag("Player"))
        {
            if (playerHealthManager.playerCurrentHealth == playerHealthManager.playerMaxHealth)
                return;
            else
            {
                playerHealthManager.playerCurrentHealth += amountOfHealing;
                if (playerHealthManager.playerCurrentHealth > playerHealthManager.playerMaxHealth)
                    playerHealthManager.playerCurrentHealth = playerHealthManager.playerMaxHealth;
                Destroy(gameObject);
            }
        }
    }
}


