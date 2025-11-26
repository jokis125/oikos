using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthManager : MonoBehaviour
{
    public static PlayerHealthManager
        instance = null; //Static instance of GameManager which allows it to be accessed by any other script.

    int playerStartingHealth = 6;
    public int playerMaxHealth = 6;
    public int playerCurrentHealth;
    int DamageModifier = 0;
    float invincibilityTime = 0.5f;
    bool invincible = false;
    bool alive = true;
    IEnumerator coroutine;
    IEnumerator deathCoroutine;
    UpgradeType upgradeType;
    GameManager gameManager;
    Color colorToBlink;
    public SpriteRenderer spriteRenderer;

    private List<MonoBehaviour> _scripts = new List<MonoBehaviour>();


    void Awake()
    {

        //Check if instance already exists
        if (instance == null)
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        playerCurrentHealth = playerStartingHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        _scripts.Add(GetComponent<PlayerMovement>());
        _scripts.Add(GetComponent<Combat>());
        _scripts.Add(GetComponent<CamShakeSimple>());
    }


    void Update()
    {
        if (invincible)
            spriteRenderer.color = Color.Lerp(new Vector4(1, 1, 1, 1), new Vector4(1, 1, 1, 0), Mathf.PingPong(Time.time, 0.5f));
        else
            spriteRenderer.color = new Vector4(1, 1, 1, 1);
    }

    public void DealDamage(int amountOfDamage) // GetDamage
    {
        if (!invincible)
        {
            playerCurrentHealth -= amountOfDamage + DamageModifier;
            if (playerCurrentHealth <= 0)
            {
                KillPlayer();
            }
            else
            {
                coroutine = BeInvincible(invincibilityTime);
                StartCoroutine(coroutine);
            }
        }
    }


    private IEnumerator BeInvincible(float waitTime)
    {
            invincible = true;
            yield return new WaitForSeconds(waitTime);
            invincible = false;
    }

    private IEnumerator Revive(float waitTime)
    {
        
        yield return new WaitForSeconds(waitTime);
        gameManager.EnterHome();
        RessPlayer();
        
    }

    private void KillPlayer()
    {
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        foreach (var script in _scripts)
        {
            script.enabled = false;
        }
        deathCoroutine = Revive(2f);
        StartCoroutine(deathCoroutine);
        
    }

    private void RessPlayer()
    {
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<CircleCollider2D>().enabled = true;
        foreach (var script in _scripts)
        {
            script.enabled = true;
        }
    }
}
