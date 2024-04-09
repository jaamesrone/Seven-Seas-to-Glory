using System.Collections.Generic;
using UnityEngine;

public class EnemyShipSpawner : MonoBehaviour
{
    public GameObject sharkShipPrefab, royalShipPrefab, pirateShipPrefab;
    public GameObject sharkPiratePrefab, piratePrefab, royalPiratePrefab;
    public GameObject playerShip;
    public int numberOfShipsToSpawn;
    public float spawnRadius;
    public float minDistanceFromPlayer = 50f;
    public float minDistanceFromOtherShips = 30f;

    private DayAndNight dayAndNight;
    private List<GameObject> spawnedShips = new List<GameObject>(); // Store all spawned ships

    void Start()
    {
        dayAndNight = FindObjectOfType<DayAndNight>();
        SpawnEnemyShips();
    }

    void Update()
    {
        UpdatePirateVisibility();
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
                randomPosition.y = 0f;

                if (Vector3.Distance(randomPosition, playerShip.transform.position) >= minDistanceFromPlayer && IsFarFromOtherShips(randomPosition))
                {
                    positionFound = true;
                }
            }

            if (positionFound)
            {
                Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                GameObject enemyShip = null;
                GameObject selectedPiratePrefab = null; // Use a different variable to select the prefab

                // Randomly select the type of ship and its corresponding pirate
                int shipType = Random.Range(0, 3); // Random number between 0 and 2
                switch (shipType)
                {
                    case 0: // Shark Ship
                        enemyShip = Instantiate(sharkShipPrefab, randomPosition, randomRotation);
                        selectedPiratePrefab = sharkPiratePrefab;
                        break;
                    case 1: // Imperial Ship
                        enemyShip = Instantiate(royalShipPrefab, randomPosition, randomRotation);
                        selectedPiratePrefab = royalPiratePrefab;
                        break;
                    case 2: // Pirate Ship
                        enemyShip = Instantiate(pirateShipPrefab, randomPosition, randomRotation);
                        selectedPiratePrefab = piratePrefab; // Directly use class-level piratePrefab
                        break;
                }

                if (enemyShip != null && selectedPiratePrefab != null)
                {
                    Vector3 piratePosition = enemyShip.transform.position;
                    piratePosition.y += 6f; // Adjust the y-position for the pirate to spawn above the ship
                    Instantiate(selectedPiratePrefab, piratePosition, Quaternion.identity, enemyShip.transform);
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
            // Check the tag of the ship to determine its visibility based on the time of day
            bool shouldBeVisible = true; // Default visibility

            if (ship.CompareTag("RoyalShip"))
            {
                shouldBeVisible = isDaytime; // Imperial ships are visible during the day
            }
            else if (ship.CompareTag("SharkShip") || ship.CompareTag("PirateShip"))
            {
                shouldBeVisible = !isDaytime; // Shark and Pirate ships are visible during the night
            }

            // Set the visibility of the entire ship
            ship.SetActive(shouldBeVisible);
        }
    }
}
