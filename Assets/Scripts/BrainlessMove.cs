using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrainlessMove : MonoBehaviour
{
    public float moveDuration;    //the max time of a walking session (set to ten)
    float elapsedTime = 0f; //time since started walk
    float wait = 0f; //wait this much time
    float waitTime = 0f; //waited this much time

    float randomX;  //randomly go this X direction
    float randomY;  //randomly go this Z direction

    bool isMoving = true; //start moving

    public int damage = 1;
    Transform player;
    EnemyBaseScript enemyBase;

    private bool _isQuitting = false;
    private bool changeRotation = false;

    Vector2 direction;
    bool changeDirection = false;

    [SerializeField] private GameObject explosionParticle = null;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");
        player = playerObject.transform;
        enemyBase = GetComponent<EnemyBaseScript>();

        randomX = Random.Range(-3, 3);
        randomY = Random.Range(-3, 3);
        direction = new Vector2(randomX, randomY);
    }

    void Update()
    {
        


        //Debug.Log (elapsedTime);
        //MOVING SPAGHETTI
        if (elapsedTime < moveDuration && isMoving)
        {
            //if its moving and didn't move too much
            Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);
            pos.x = Mathf.Clamp(pos.x, 0.1f, 0.9f);
            pos.y = Mathf.Clamp(pos.y, 0.1f, 0.9f);
            transform.position = Camera.main.ViewportToWorldPoint(pos);
            transform.Translate(direction * Time.deltaTime);
            elapsedTime += Time.deltaTime;

        }
        else if(isMoving)
        {
            //do not move and start waiting for random time
            isMoving = false;
            wait = Random.Range(1, 3);
            waitTime = 0f;
        }

        if (waitTime < wait && !isMoving)
        {
            //you are waiting
            waitTime += Time.deltaTime;


        }
        else if (!isMoving) //done waiting. Move to these random directions
        {
            isMoving = true;
            elapsedTime = 0f;

            if (!changeDirection)
            {
                var heading = player.position - gameObject.transform.position;
                var distance = heading.magnitude;
                direction = heading / distance; // This is now the normalized direction.
                direction = direction * 2;
                changeDirection = true;
            }
            else
            {
                randomX = Random.Range(-3, 3);
                randomY = Random.Range(-3, 3);
                direction = new Vector2(randomX, randomY);
                changeDirection = false;
            }
        }
    }

    public void Explode()
    {
        Instantiate(explosionParticle, transform.position, Quaternion.identity);
        Collider2D[] hitCollider = Physics2D.OverlapCircleAll(gameObject.transform.position, 3f);
        foreach (var collision in hitCollider)
        {
            if (collision.transform.CompareTag("Player")) DealDamage();
        }
    }
    void DealDamage()
    {
        player.GetComponent<PlayerHealthManager>().DealDamage(damage);
    }
    
   

    void OnApplicationQuit()
    {
        _isQuitting = true;
    }
}
