using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnUpgradeInRoom : MonoBehaviour
{
    private Transform UpgradeSpawnLocation;
    public GameManager.upgradeName upgradeTypes;
    public GameObject[] UpgradeGO;
    private GameManager gameManager;
    // Start is called before the first frame update
    void Start()
    {
        UpgradeSpawnLocation = gameObject.transform;
        gameManager = GameManager.instance;
        upgradeTypes = (GameManager.upgradeName)Mathf.Floor(Random.Range(0f,6f));
        switch (upgradeTypes)
        {
            case GameManager.upgradeName.dashCDdown:
                if (!gameManager.hasdashcd)
                {
                    Instantiate(UpgradeGO[0], UpgradeSpawnLocation);
                }
                break;
            case GameManager.upgradeName.dash:
                if (!gameManager.hasdash)
                {
                    Instantiate(UpgradeGO[1], UpgradeSpawnLocation);
                }
                break;
            case GameManager.upgradeName.damageUp:
                if (!gameManager.hasdamageUp)
                {
                    Instantiate(UpgradeGO[2], UpgradeSpawnLocation);
                }
                break;
            case GameManager.upgradeName.shotgun:
                if (!gameManager.hasShotgun)
                {
                    Instantiate(UpgradeGO[3], UpgradeSpawnLocation);
                }
                break;

            case GameManager.upgradeName.healthUp:
                if (gameManager.numberOfHealthUps <= 3)
                {
                    Instantiate(UpgradeGO[4], UpgradeSpawnLocation);
                }
                break;
            
            case GameManager.upgradeName.speedUp:
                if (gameManager.numberOfSpeedUps <= 2)
                {
                    Instantiate(UpgradeGO[5], UpgradeSpawnLocation);
                }
                break;
            default:
                break;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
