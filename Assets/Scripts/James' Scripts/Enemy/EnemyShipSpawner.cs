using System.Collections.Generic;
using UnityEngine;

public class EnemyShipSpawner : MonoBehaviour
{
    public GameObject sharkShipPrefab, royalShipPrefab, pirateShipPrefab, zombieShip;
    public GameObject sharkPiratePrefab, piratePrefab, royalPiratePrefab, zombiePrefab;
    public GameObject playerShip;
    public int numberOfShipsToSpawn;
    public int numberOfSharkPiratesPerShip;
    public int numberOfRoyalPiratesPerShip;
    public int numberOfNormalPiratesPerShip;
    public int numberOfZombiePiratesPerShip;
    public float spawnRadius;
    public float minDistanceFromPlayer = 50f;
    public float minDistanceFromOtherShips = 30f;


    private DayAndNight dayAndNight;
    private List<GameObject> spawnedShips = new List<GameObject>(); 

    void Start()
    {
        dayAndNight = FindObjectOfType<DayAndNight>();
        SpawnEnemyShips();
    }

    void Update()
    {
       // UpdatePirateVisibility();
    }

    void SpawnEnemyShips()
    {
        for (int i = 0; i < numberOfShipsToSpawn; i++)
        {
            Vector3 randomPosition = Vector3.zero;
            bool positionFound = false;
            int attemptCounter = 0;

            while (!positionFound && attemptCounter < 100)
            {
                attemptCounter++;
                randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
                randomPosition.y = 1.1f;

                if (Vector3.Distance(randomPosition, playerShip.transform.position) >= minDistanceFromPlayer && IsFarFromOtherShips(randomPosition))
                {
                    positionFound = true;
                }
            }

            if (positionFound)
            {
                Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                GameObject enemyShip = null;
                GameObject selectedPiratePrefab = null;
                int numberOfPiratesToSpawn = 0;

                int shipType = Random.Range(0, 4); 
                switch (shipType)
                {
                    case 0: // Shark Ship
                        enemyShip = Instantiate(sharkShipPrefab, randomPosition, randomRotation);
                        selectedPiratePrefab = sharkPiratePrefab;
                        numberOfPiratesToSpawn = numberOfSharkPiratesPerShip;
                        break;
                    case 1: // Imperial Ship
                        enemyShip = Instantiate(royalShipPrefab, randomPosition, randomRotation);
                        selectedPiratePrefab = royalPiratePrefab;
                        numberOfPiratesToSpawn = numberOfRoyalPiratesPerShip;
                        break;
                    case 2: // Pirate Ship
                        enemyShip = Instantiate(pirateShipPrefab, randomPosition, randomRotation);
                        selectedPiratePrefab = piratePrefab;
                        numberOfPiratesToSpawn = numberOfNormalPiratesPerShip;
                        break;
                    case 3: // Zombies Ship
                        enemyShip = Instantiate(zombieShip, randomPosition, randomRotation);
                        selectedPiratePrefab = zombiePrefab;
                        numberOfPiratesToSpawn = numberOfZombiePiratesPerShip;
                        break;
                }

                if (enemyShip != null && selectedPiratePrefab != null)
                {
                    for (int j = 0; j < numberOfPiratesToSpawn; j++)
                    {
                        Vector3 piratePosition = enemyShip.transform.position;
                        piratePosition.y += 6f; 
                        Instantiate(selectedPiratePrefab, piratePosition, Quaternion.identity, enemyShip.transform);
                    }
                    spawnedShips.Add(enemyShip);
                }
            }
            else
            {
                Debug.LogWarning($"Could not find a suitable position for enemy ship #{i + 1}");
            }
        }
    }

    bool IsFarFromOtherShips(Vector3 position)
    {
        foreach (GameObject ship in spawnedShips)
        {
            if (Vector3.Distance(position, ship.transform.position) < minDistanceFromOtherShips)
                return false;
        }
        return true;
    }

    void UpdatePirateVisibility()
    {
        bool isDaytime = dayAndNight.IsDaytime;
        foreach (GameObject ship in spawnedShips)
        {
            bool shouldBeVisible = true; 

            if (ship.CompareTag("RoyalShip"))
            {
                shouldBeVisible = isDaytime; 
            }
            else if (ship.CompareTag("SharkShip") || ship.CompareTag("PirateShip") || ship.CompareTag("zombieShip")) 
            {
                shouldBeVisible = !isDaytime;
            }

            ship.SetActive(shouldBeVisible);
        }
    }
}
