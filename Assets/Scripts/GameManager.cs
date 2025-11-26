using UnityEngine;
using System.Collections;

using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Allows us to use Lists. 

public class GameManager : MonoBehaviour
{

    public enum upgradeName
    {
        healthUp,
        dash,
        damageUp,
        dashCDdown,
        speedUp,
        shotgun
    }

    public bool hasShotgun = false;
    public bool hasdash = false;
    public bool hasdashcd = false;
    public bool hasdamageUp = false;
    public int numberOfHealthUps = 0;
    public int numberOfSpeedUps = 0;

    PlayerHealthManager playerHealthManager;
    PlayerMovement playerMovement;
    Combat combat;
    public GameObject player;

    public int bulletDamage = 1;

    public static GameManager
        instance = null; //Static instance of GameManager which allows it to be accessed by any other script.

    private int level = 0; //Current level number, expressed in game as "Day 1".
    public int currency = 0;

    public int enemyCount = 0;
    
    public List<Door> doorList;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        // linking GameManager with the player
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealthManager = player.GetComponent<PlayerHealthManager>();
        playerMovement = player.GetComponent<PlayerMovement>();
        combat = player.GetComponent<Combat>();

        //creates a doorlist for using doors
        doorList = new List<Door>();

        //Call the InitGame function to initialize the first level 
        InitGame();
    }

    private void Update()
    {
        


        //Opens all doors when fucking all chunguses are banishes

        if (enemyCount <= 0)
        {
            foreach (Door d in doorList)
            {
                d.GetComponent<Animator>().Play("doorAnim");
                Debug.Log("Opening door" + d.ToString());
            }
        }
        else
        {
            foreach (Door d in doorList)
            {
                d.GetComponent<Animator>().Play("doorClose");
                Debug.Log("Opening door" + d.ToString());
            }
        }
    }

    //Initializes the game for each level.
    void InitGame()
    {
        //initialize level

    }


    //Update is called every frame.
    public void Upgrade(upgradeName upgradeName, int price)
    {
        switch (upgradeName)
        {
            case upgradeName.damageUp:
                hasdamageUp = true;
                bulletDamage += 1;
                currency -= price;
                break;
            case upgradeName.dash:
                hasdash = true;
                playerMovement.hasDash = true;
                currency -= price;
                break;
            case upgradeName.dashCDdown:
                hasdashcd = true;
                playerMovement.dashCDMod = 0.5f;
                currency -= price;
                break;
            case upgradeName.healthUp:
                numberOfHealthUps++;
                playerHealthManager.playerMaxHealth += 2;
                playerHealthManager.playerCurrentHealth += 2;
                currency -= price;
                break;
            case upgradeName.shotgun:
                hasShotgun = true;
                player.GetComponent<Combat>().GiveConeShot();
                currency -= price;
                break;
            case upgradeName.speedUp:
                numberOfSpeedUps++;
                playerMovement.movementModifier = 1.3f;
                currency -= price;
                break;
            default:
                break;
            
            
        }
    }

    public void AddDoor(Door d)
    {
        doorList.Add(d);
    }

    public void EnterDungeon()
    {
        SceneManager.LoadScene("MapGeneratorTesting", LoadSceneMode.Single);
        enemyCount = 0;
    }

    public void EnterBossRoom()
    {
        doorList.Clear();
        SceneManager.LoadScene("BossRoom", LoadSceneMode.Single);
        player.transform.position = new Vector2(13f, 0f);
        
    }

    public void EnterHome()
    {
        doorList.Clear();
        

        playerHealthManager.playerCurrentHealth = playerHealthManager.playerMaxHealth;
        SceneManager.LoadScene("House", LoadSceneMode.Single);
        player.transform.position = new Vector2(77f, 40f);
    }

    public void ExitGame()
    {

    }


}