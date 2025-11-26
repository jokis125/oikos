using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour
{
    [SerializeField] private GameObject attackProjectile = null;
    private float _rotationMultiplier = 0.5f;
    bool canShoot = false;
    public float shootDelay = 5f;
    public float shotDistance = 5f;
    IEnumerator coroutine;
    public int damage = 1;

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

}
