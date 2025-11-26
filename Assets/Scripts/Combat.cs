using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Combat : MonoBehaviour
{
    [SerializeField] private GameObject currentPlayer;
    [SerializeField] private GameObject attackProjectile = null;
    private Vector2 _currentPlayerVelocity;// = new Vector2(0, 1);

    private Vector2 _rotation = Vector2.right;
    
    private PlayerMovement _pm;
    private CamShakeSimple _shaker;
    

    private float _rotationMultiplier = 0.5f;
    private float _reloadTimer = 0;
    [SerializeField]private float _timeToReload = 0.2f;
    //[SerializeField]private float AnimationTime = 0.3f;
    public bool isShooting = false; // do not know if i need this anymore but im tired
    IEnumerator coroutine;  // arnas need this for animation

    enum weaponType
    {
        singleShot,
        coneShot
    };
    weaponType weapon;

    [SerializeField] private weaponType selectedWeapon;

    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        //Get velocity from movement script

        //temporary velocity, replace later
        _currentPlayerVelocity = new Vector2(1, 0);
        _pm = GetComponent<PlayerMovement>();
        _shaker = GetComponent<CamShakeSimple>();
        
        //weapon = weaponType.coneShot;
    }

    // Update is called once per frame
    private void Update()
    {
        getCurrentPlayerVelocity();
        if (_reloadTimer > 0.0f)
        {
            _reloadTimer -= Time.deltaTime;
        }
        else _reloadTimer = 0.0f;
        ChangePlayerDirection();
        if (_rotation != Vector2.zero)
        {
            if(_reloadTimer <= 0)
                Shoot();
        }

    }

    private void LateUpdate()
    {
        isShooting = false;
        
    }

    private void Shoot()
    {

        coroutine = Animate(0.2f);
        StartCoroutine(coroutine);
        //ChangePlayerDirection();
        animator.SetBool("isAttacking", true);
        isShooting = true;
        switch (weapon)
        {
            case weaponType.singleShot:
            {
                Vector2 bulletVector = (Vector2)transform.position + (_rotation * _rotationMultiplier);
                var obj = (GameObject) Instantiate(attackProjectile, bulletVector, Quaternion.identity);
                obj.GetComponent<BulletScript>().ChangeBulletVelocity(_rotation + (_currentPlayerVelocity * 0.15f));
                _shaker.ShakeItBaby(8);
                break;
            }
            case weaponType.coneShot:
            {
                const int bulletCount = 3;
                _shaker.ShakeItBaby(16);
                GameObject[] bulletObjects = new GameObject[bulletCount];
                for (int i =  0; i < bulletCount; i++)
                {
                    var obj = (GameObject)Instantiate(attackProjectile,(Vector2) transform.position + _rotation * _rotationMultiplier, Quaternion.identity);
                    if (i % 2 == 0)
                    {
                        ShootCone(obj, i);
                    }
                    else
                    {
                        obj.GetComponent<BulletScript>().ChangeBulletVelocity(_rotation);
                    }
                }
                break;
            } 
        }
        _reloadTimer = _timeToReload;
    }

    private IEnumerator Animate(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        animator.SetBool("isAttacking", false);
    }

    private void ShootCone(GameObject obj, int i)
    {
        if (_rotation == Vector2.up || _rotation == Vector2.down)
        {
            obj.GetComponent<BulletScript>().ChangeBulletVelocity(new Vector2(((-1 + i) * 0.1f), _rotation.y));
        }
        else if(_rotation == Vector2.left || _rotation == Vector2.right)
        {
            obj.GetComponent<BulletScript>().ChangeBulletVelocity(new Vector2(_rotation.x, ((-1 + i) * 0.1f)));
        }
    }

    private void ChangePlayerDirection()
    {
        _rotation = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            animator.SetInteger("ShootSide", 1);
            _rotation = Vector2.up;   
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetInteger("ShootSide", 0);
            _rotation = Vector2.down;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            animator.SetInteger("ShootSide", 3);
            _rotation = Vector2.right;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            animator.SetInteger("ShootSide", 2);
            _rotation = Vector2.left;
        }
    }

    private void getCurrentPlayerVelocity()
    {
        _currentPlayerVelocity = _pm.GetPlayerDirection();
    }

    private Vector2 SwapXforY(Vector2 swappedVector)
    {
        return new Vector2(swappedVector.y, swappedVector.x);
    }

    public void GiveConeShot()
    {
        weapon = weaponType.coneShot;
    }
}