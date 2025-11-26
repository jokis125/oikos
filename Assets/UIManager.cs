using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public int heartAmount = 3;
    public Transform heart1;
    public Transform heart2;
    public Transform heart3;
    public Transform heart4;
    public Transform heart5;
    public Transform heart6;

    public Sprite heartSprite1;
    public Sprite heartSprite2;

    public Text text;

    GameObject player;
    PlayerHealthManager playerHealthManager;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.instance;
        player = GameObject.FindGameObjectWithTag("Player");
        playerHealthManager = player.GetComponent<PlayerHealthManager>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = gameManager.currency.ToString();


        if (playerHealthManager.playerCurrentHealth == 12)
        {
            heart6.GetComponent<Image>().enabled = true;
            heart6.GetComponent<Image>().sprite = heartSprite1;
            heart5.GetComponent<Image>().enabled = true;
            heart5.GetComponent<Image>().sprite = heartSprite1;
            heart4.GetComponent<Image>().enabled = true;
            heart4.GetComponent<Image>().sprite = heartSprite1;
        }
        else if (playerHealthManager.playerCurrentHealth == 11)
        {
            heart6.GetComponent<Image>().enabled = true;
            heart6.GetComponent<Image>().sprite = heartSprite2;
        }
        else if (playerHealthManager.playerCurrentHealth == 10)
        {
            heart6.GetComponent<Image>().enabled = false;
            heart5.GetComponent<Image>().enabled = true;
            heart5.GetComponent<Image>().sprite = heartSprite1;
            heart4.GetComponent<Image>().enabled = true;
            heart4.GetComponent<Image>().sprite = heartSprite1;
        }
        else if (playerHealthManager.playerCurrentHealth == 9)
        {
            heart5.GetComponent<Image>().enabled = true;
            heart5.GetComponent<Image>().sprite = heartSprite2;
        }
        else if (playerHealthManager.playerCurrentHealth == 8)
        {
            heart5.GetComponent<Image>().enabled = false;
            heart4.GetComponent<Image>().enabled = true;
            heart4.GetComponent<Image>().sprite = heartSprite1;
            heart4.GetComponent<Image>().enabled = true;
            heart3.GetComponent<Image>().sprite = heartSprite1;
        }
        else if (playerHealthManager.playerCurrentHealth == 7)
        {
            heart4.GetComponent<Image>().enabled = true;
            heart4.GetComponent<Image>().sprite = heartSprite2;
        }
        else if (playerHealthManager.playerCurrentHealth == 6)
        {
            heart4.GetComponent<Image>().enabled = false;
            heart3.GetComponent<Image>().enabled = true;
            heart3.GetComponent<Image>().sprite = heartSprite1;
            heart3.GetComponent<Image>().enabled = true;
            heart2.GetComponent<Image>().sprite = heartSprite1;
        }
        else if (playerHealthManager.playerCurrentHealth == 5)
        {
            heart3.GetComponent<Image>().enabled = true;
            heart3.GetComponent<Image>().sprite = heartSprite2;
        }
        else if (playerHealthManager.playerCurrentHealth == 4)
        {
            heart3.GetComponent<Image>().enabled = false;
            heart2.GetComponent<Image>().enabled = true;
            heart2.GetComponent<Image>().sprite = heartSprite1;
            heart2.GetComponent<Image>().enabled = true;
            heart1.GetComponent<Image>().sprite = heartSprite1;
        }
        else if (playerHealthManager.playerCurrentHealth == 3)
        {
            heart2.GetComponent<Image>().enabled = true;
            heart2.GetComponent<Image>().sprite = heartSprite2;
        }
        else if (playerHealthManager.playerCurrentHealth == 2)
        {
            heart2.GetComponent<Image>().enabled = false;
            heart1.GetComponent<Image>().enabled = true;
            heart1.GetComponent<Image>().sprite = heartSprite1;
        }
        else if (playerHealthManager.playerCurrentHealth == 1)
        {
            heart1.GetComponent<Image>().enabled = true;
            heart1.GetComponent<Image>().sprite = heartSprite2;
        }
        else if (playerHealthManager.playerCurrentHealth == 0) heart1.GetComponent<Image>().enabled = false;
    }
}
