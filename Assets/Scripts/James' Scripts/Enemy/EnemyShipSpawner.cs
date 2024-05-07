using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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

    public TextMeshProUGUI despawnWarning;


    private DayAndNight dayAndNight;
    private List<GameObject> spawnedShips = new List<GameObject>();

    private bool previousState;

    void Start()
    {
        dayAndNight = FindObjectOfType<DayAndNight>();
        previousState = dayAndNight.IsDaytime;
        SpawnEnemyShips();
    }

    void Update()
    {
       UpdatePirateVisibility();
       SetSpawnWarning();
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
                randomPosition.y = -0.34f;

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
            if (ship != null) //not having null checks has caused a lot of errors to pop up
            {
                if (Vector3.Distance(position, ship.transform.position) < minDistanceFromOtherShips)
                    return false;
            }
        }
        return true;
    }

    void UpdatePirateVisibility()
    {
        bool isDaytime = dayAndNight.IsDaytime;
        foreach (GameObject ship in spawnedShips)
        {
            bool shouldBeVisible = true;
            if (ship != null) //not having null checks has caused a lot of errors to pop up
            {
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

    private void SetSpawnWarning()
    {
        if (previousState == true && !dayAndNight.IsDaytime)
        {
            StartCoroutine(ActivateWarning("The Emperials are returning to the barracks."));
        }
        if (previousState == false && dayAndNight.IsDaytime)
        {
            StartCoroutine(ActivateWarning("The Undead are returning to the depths."));
        }
        previousState = dayAndNight.IsDaytime;
    }

    private IEnumerator ActivateWarning(string warning)
    {
        despawnWarning.text = warning;
        despawnWarning.enabled = true;
        yield return new WaitForSeconds(3.5f);
        despawnWarning.enabled = false;
    }
}
