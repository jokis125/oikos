using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnThePlayerIn : MonoBehaviour
{
    public GameObject player;
    // Start is called before the first frame update
    void Awake()
    {
        if (GameObject.FindGameObjectsWithTag("Player") == null)
        {
            var spawnedplayer = Instantiate(player);
            spawnedplayer.transform.position = new Vector2(77, 40);
        }
    }

}
