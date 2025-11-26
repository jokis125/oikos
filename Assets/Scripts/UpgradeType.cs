using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeType : MonoBehaviour
{
  
    public int price;
    public GameManager.upgradeName upgradeName;

    private GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("someone entered this trigger");
        if (other.CompareTag("Player"))
        {
            Debug.Log("It was a player");
            if (gameManager.currency > price)
            {
                Debug.Log("buying upgrade");
                gameManager.Upgrade(upgradeName, price);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("not enouth money");
            }
                 
        }
    }
}
