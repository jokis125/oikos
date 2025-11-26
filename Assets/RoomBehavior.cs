using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehavior : MonoBehaviour
{
    public GameObject[] spawnPoints;

    public GameObject[] enemies;

    public bool spawned = false;

    
   

    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            if(!spawned)
            {
                SpawnEnemies();
                spawned = true;

            }
            
        }
    }

    private void SpawnEnemies()
    {
        for(int i = 0; i < spawnPoints.Length; i++)
        {
            int index = Random.Range(0, enemies.Length);
            Instantiate(enemies[index], spawnPoints[i].transform.position, Quaternion.identity);
            GameManager.instance.enemyCount++;
        }
    }
}
