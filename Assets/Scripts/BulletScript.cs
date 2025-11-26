using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public Vector2 bulletVelocity;
    private Vector2 _bulletSpeed;

    private Rigidbody2D _rb;
    [SerializeField] private float bulletSpeedMultiplier = 0;
    [SerializeField] private GameObject explosionParticle = null;

    private const float DestroyAfterUnits = 20;

    private float _totalDistance = 0;
    private bool _isQuitting = false;

    public int bulletDamage = 1;
    // Start is called before the first frame update
    void Start()
    {
        //bulletVelocity = new Vector2(0,0);
        bulletVelocity = this.transform.position;
        //bulletSpeed = Vector2.right;
        _rb = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        bulletMove();
        CommitSudoku();
    }

    private void bulletMove()
    {
        _totalDistance += Mathf.Abs(_bulletSpeed.x * bulletSpeedMultiplier * Time.deltaTime);
        _totalDistance += Mathf.Abs(_bulletSpeed.y * bulletSpeedMultiplier * Time.deltaTime);
        bulletVelocity += _bulletSpeed * bulletSpeedMultiplier * Time.deltaTime;
        _rb.MovePosition(bulletVelocity);
    }

    public void ChangeBulletVelocity(Vector2 newVel)
    {
        this._bulletSpeed = newVel;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {

        if (other.gameObject.CompareTag("Wall"))  //wall or Enemy
        {
            Destroy(gameObject);
        }
    }

    private void CommitSudoku()
    {
        if(_totalDistance >= DestroyAfterUnits)
            Destroy(gameObject);
    }

    private void OnDestroy()
    {
        if(!_isQuitting)
            Instantiate(explosionParticle, transform.position, Quaternion.identity);
    }
    
    void OnApplicationQuit()
    {
        _isQuitting = true;
    }
}
