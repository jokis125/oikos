using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public enum LastKey
    {
        left,
        right,
        up,
        down
    }
    private Combat combatScript;

    public float movementModifier = 1.0f;
    public float playerMaxSpeed;
    public bool isDashing = false;
    public float dashCDMod = 1.0f;  // dont make this lower than 0.1f 
    public bool hasDash = false;

    Animator animator;
    SpriteRenderer spriteRenderer;
    private Vector2 AxisInput;   
    private Rigidbody2D _rigidbody2D;
    private int activeKeys = 0;
    [SerializeField]
    private float dashspeed = 10.0f;
    private bool isDashOnCD = false;
    private float dashTime = 0.1f;
    private float dashCooldown = 1.0f;
    private LastKey lastKey = LastKey.down;
    [SerializeField]
    private LastKey activeSide = LastKey.down;
    IEnumerator coroutine;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        combatScript = gameObject.GetComponent<Combat>();
    }

    

    // Update is called once per frame
    void Update()
    {
        activeSide =  GetLastKey();

        AxisInput = (new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));

        if (hasDash && !isDashOnCD && Input.GetKeyDown(KeyCode.Space))
            Dash(AxisInput);

#region movement if not dashing
        if (!isDashing)
        {
            if (Input.GetAxis("Horizontal") != 0.0f || Input.GetAxis("Vertical") != 0.0f)
            {
                if (AxisInput.magnitude > 1.0f)
                    _rigidbody2D.velocity = AxisInput.normalized * playerMaxSpeed * movementModifier;
                else
                    _rigidbody2D.velocity = AxisInput * playerMaxSpeed * movementModifier;
            }
            else
            {
                _rigidbody2D.velocity = new Vector2(0f, 0f);
            }
        }
#endregion
#region Animation control
        if(AxisInput.magnitude != 0 && !combatScript.isShooting)
        {
            animator.SetBool("IsWalking", true);
        }
        else
        {
            animator.SetBool("IsWalking", false);
        }

        switch (activeSide)
        {
            case LastKey.down:
                spriteRenderer.flipX = false;
                animator.SetInteger("Side", 0);
                break;
            case LastKey.up:
                spriteRenderer.flipX = false;
                animator.SetInteger("Side", 1);
                break;
            case LastKey.left:
                spriteRenderer.flipX = true;
                animator.SetInteger("Side", 2);
                break;
            case LastKey.right:
                spriteRenderer.flipX = false;
                animator.SetInteger("Side", 3);
                break;
            default:
                break;
        }

#endregion
    }


    public Vector2 GetPlayerDirection()
    {
        Vector2 direcrtion = _rigidbody2D.velocity.normalized;

        if (direcrtion.x == direcrtion.y && direcrtion.x != 0.0f)
        {
            //conflict


        }
        else if(direcrtion.x == direcrtion.y && direcrtion.x == 0.0f)
        {
            direcrtion = new Vector2(0f, 0f);
        }
        else if (Mathf.Abs(direcrtion.x) > Mathf.Abs(direcrtion.y))     // noconflict
        {
            if (direcrtion.x > 0)
                direcrtion = new Vector2(1f, 0f);
            else
                direcrtion = new Vector2(-1f, 0f);
        }
        else
        {
            if (direcrtion.y > 0)
                direcrtion = new Vector2(0f, 1f);
            else
                direcrtion = new Vector2(0f, -1f);
        }



        return direcrtion;
    }

    public LastKey GetLastKey()
    {
        LastKey Temp_lastKey = activeSide;   // default value

        #region key deregistation
        if (Input.GetKeyUp(KeyCode.W))
        {
            if (activeKeys == 1)
                Temp_lastKey = LastKey.up;
            if (activeKeys > 1)
                Temp_lastKey = lastKey;
            activeKeys--;       // save last active key
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            if (activeKeys == 1)
                Temp_lastKey = LastKey.down;
            if (activeKeys > 1)
                Temp_lastKey = lastKey;
            activeKeys--;       // save last active key
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            if (activeKeys == 1)
                Temp_lastKey = LastKey.left;
            if (activeKeys > 1)
                Temp_lastKey = lastKey;
            activeKeys--;       // save last active key
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            if (activeKeys == 1)
                Temp_lastKey = LastKey.right;
            if (activeKeys > 1)
                Temp_lastKey = lastKey;
            activeKeys--;       // save last active key
        }
        #endregion

        #region last key registration
        if (Input.GetKeyDown(KeyCode.W))
        {
            lastKey = LastKey.up;
            if (activeKeys == 0)
                Temp_lastKey = LastKey.up;      // if you are the first one save it 
            activeKeys++;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            lastKey = LastKey.down;
            if (activeKeys == 0)
                Temp_lastKey = LastKey.down;      // if you are the first one save it 
            activeKeys++;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            lastKey = LastKey.left;
            if (activeKeys == 0)
                Temp_lastKey = LastKey.left;      // if you are the first one save it 
            activeKeys++;
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            lastKey = LastKey.right;
            if (activeKeys == 0)
                Temp_lastKey = LastKey.right;      // if you are the first one save it 
            activeKeys++;
        }

        if (activeKeys == 0)        // is nothing is pressed look down
            Temp_lastKey = LastKey.down;
        #endregion

        #region active key correction
        if (Input.GetKey(KeyCode.W))
        {
            if (activeKeys == 1) 
            Temp_lastKey = LastKey.up;
        }
        if (Input.GetKey(KeyCode.S))
        {
            if (activeKeys == 1) 
            Temp_lastKey = LastKey.down;
        }
        if (Input.GetKey(KeyCode.A))
        {
            if (activeKeys == 1) 
            Temp_lastKey = LastKey.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            if (activeKeys == 1) 
            Temp_lastKey = LastKey.right;
        }
        #endregion

        return Temp_lastKey;
    }

    private void Dash(Vector2 direction)
    {
        isDashing = true;
        isDashOnCD = true;
        _rigidbody2D.velocity = _rigidbody2D.velocity * dashspeed;
        coroutine = DoDash((dashCooldown * dashCDMod)- dashTime);
        StartCoroutine(coroutine);
        
    }

    public void Push(Vector2 direction)
    {
        isDashing = true;
        isDashOnCD = true;
        GetComponent<Rigidbody2D>().AddForce(direction);
        coroutine = DoDash((dashCooldown * dashCDMod) - dashTime);
        StartCoroutine(coroutine);
    }

    private IEnumerator DoDash(float waitTime)
    {
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        yield return new WaitForSeconds(waitTime);
        isDashOnCD = false;
    }

}
