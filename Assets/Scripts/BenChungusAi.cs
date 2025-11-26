using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BenChungusAi : MonoBehaviour
{
    [SerializeField] private GameObject attackProjectile = null;
    private float _rotationMultiplier = 0.5f;
    bool canShoot = false;
    private bool offCooldown = false;
    public float shootDelay = 0.7f;
    public float spawnCooldown = 10f;
    public float shotDistance = 5f;
    IEnumerator coroutine;
    public int damage = 1;
    [SerializeField] private int benChungusArmyCount = 3;

    [SerializeField] private GameObject[] _chungusArmy = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(!canShoot)
        {
            Collider2D[] hitCollider = Physics2D.OverlapCircleAll(gameObject.transform.position, shotDistance);
            foreach (var collision in hitCollider)
            {
                if (collision.transform.CompareTag("Player"))
                {
                    coroutine = ShootDelay(shootDelay);
                    StartCoroutine(coroutine);
                    Shoot(collision.transform);
                }
            }
        }
        
        if(!offCooldown)
        {

            StartCoroutine(SpawnArmy(spawnCooldown));
            var army = chooseArmy();
            for (int i = 0; i < benChungusArmyCount; i++)
            {
                Instantiate(army[i], transform.position + new Vector3(5, i * 2), Quaternion.identity);
            }
        }
        transform.Rotate(Vector3.forward * -100 * Time.deltaTime);
    }

    void Shoot(Transform target)
    {
        //StartCoroutine(coroutine);
        var heading = target.position - gameObject.transform.position;
        var distance = heading.magnitude;
        Vector2 direction = heading / distance; // This is now the normalized direction.

        var obj = (GameObject)Instantiate(attackProjectile, (Vector2)transform.position + direction * _rotationMultiplier, Quaternion.identity);
        obj.GetComponent<EnemyBullet>().ChangeBulletVelocity(direction);
    }

    private IEnumerator ShootDelay(float waitTime)
    {
        canShoot = true;
        yield return new WaitForSeconds(waitTime);
        canShoot = false;
    }
    private IEnumerator SpawnArmy(float waitTime)
    {
        offCooldown = true;
        yield return new WaitForSeconds(waitTime);
        offCooldown = false;
    }

    List<GameObject> chooseArmy()
    {
        List<GameObject> temp = new List<GameObject>();
        for (int i = 0; i < benChungusArmyCount; i++)
        {
            temp.Add(_chungusArmy[Random.Range(0,3)]);
        }
        
        return temp;
    }



}
