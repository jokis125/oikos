using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBaseScript : MonoBehaviour
{
    private int _currencyReward;
    public int enemyHealth;
    private GameManager _gameManager;
    [SerializeField]private int minReward =0, maxReward=2;

/*    enum enemyTypes
    {
        Chaser,
        Exploder,
        Shooter,
        BenChungus
    }

    [SerializeField] private enemyTypes enemyType;*/

    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.instance;
        _currencyReward = Random.Range(minReward, maxReward);
        //typeSelect();
    }

    // Update is called once per frame
    private void OnDestroy()
    {
        _gameManager.enemyCount--;
  
    }

/*    private void typeSelect()
    {
        if(GetComponent<Chase>() !=null)
            enemyType = enemyTypes.Chaser;
        else if (GetComponent<BrainlessMove>() != null)
            enemyType = enemyTypes.Exploder;
        else if (GetComponent<Shooter>() != null)
            enemyType = enemyTypes.Shooter;
        else if (GetComponent<BenChungusAi>() != null)
            enemyType = enemyTypes.BenChungus;
    }*/
    
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            int bulletDamage = other.gameObject.GetComponent<BulletScript>().bulletDamage;
            enemyHealth -= bulletDamage;
            Destroy(other.gameObject); //destroy bullet
            if (enemyHealth <= 0)
            {
                OnKill();
            }
        }
    }

    private void OnKill()
    {

        _gameManager.currency += _currencyReward;
        if (gameObject.GetComponent<BrainlessMove>() != null) 
            gameObject.GetComponent<BrainlessMove>().Explode();
        Destroy(gameObject);
        
    }
    
    
}
