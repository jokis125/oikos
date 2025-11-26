using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enterDungion : MonoBehaviour
{
    GameManager gameManager;

    void Start()
    {
        gameManager = GameManager.instance;
    }
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("someone entered this trigger");
        if (other.CompareTag("Player"))
        {
            gameManager.EnterDungeon();
        }
    }
}
