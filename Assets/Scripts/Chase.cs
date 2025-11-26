using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chase : MonoBehaviour
{

    Transform player;
    public float speed = 2f;
    public float minDistance = 5f;
    public float chasingDistance = 10f;
    private float idleDistance;
    private float range;
    public int damage = 1;


    // Start is called before the first frame update
    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject.transform;
        idleDistance = minDistance; // setting idle distance
    }

    // Update is called once per frame
    void Update()
    {
        range = Vector2.Distance(transform.position, player.position); // check distance between player and enemy
        //Debug.Log(range);

        if (range < minDistance)
        {
            if(minDistance != chasingDistance) minDistance = chasingDistance;  // set new longer chasing distance

            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime); // move enemy
            //if (range < 1) DealDamage(); //DAMAGE LOGIC HERE
        }
        else minDistance = idleDistance; 
    }

    void DealDamage()
    {
        player.GetComponent<PlayerHealthManager>().DealDamage(damage);

        var heading = player.position - gameObject.transform.position;
        var distance = heading.magnitude;
        Vector2 direction = heading / distance; // This is now the normalized direction.
        GetComponent<Rigidbody2D>().AddForce(-direction * 500);
        player.GetComponent<PlayerMovement>().Push(direction * 300);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            DealDamage();
        }
    }

}
