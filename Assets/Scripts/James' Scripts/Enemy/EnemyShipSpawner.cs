using System.Collections.Generic;
using UnityEngine;

public class EnemyShipSpawner : MonoBehaviour
{
    public GameObject enemyShipPrefab;
    public List<GameObject> dayPiratePrefabs;
    public List<GameObject> nightPiratePrefabs;
    public GameObject playerShip;
    public int numberOfShipsToSpawn;
    public float spawnRadius;
    public float minDistanceFromPlayer = 50f;
    public float minDistanceFromOtherShips = 30f;

    private DayAndNight dayAndNight;
    private List<GameObject> allDayPirates = new List<GameObject>();
    private List<GameObject> allNightPirates = new List<GameObject>();
    private List<Vector3> spawnedShipPositions = new List<Vector3>(); 

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
                GameObject enemyShip = Instantiate(enemyShipPrefab, randomPosition, randomRotation);
                SpawnPiratesOnShip(enemyShip, dayPiratePrefabs, allDayPirates);
                SpawnPiratesOnShip(enemyShip, nightPiratePrefabs, allNightPirates);
            }
            else
            {
                Debug.LogWarning($"Could not find a suitable position for enemy ship #{i + 1}");
            }
        }
    }

    bool IsFarFromOtherShips(Vector3 position)
    {
        foreach (Vector3 otherPosition in spawnedShipPositions) 
        {
            if (Vector3.Distance(position, otherPosition) < minDistanceFromOtherShips)
                return false;
        }
        return true;
    }

    void SpawnPiratesOnShip(GameObject ship, List<GameObject> piratePrefabs, List<GameObject> pirateList)
    {
        foreach (GameObject piratePrefab in piratePrefabs)
        {
            Vector3 piratePosition = ship.transform.position + Random.insideUnitSphere * 5f;
            piratePosition.y = ship.transform.position.y + 6f;
            GameObject pirate = Instantiate(piratePrefab, piratePosition, Quaternion.identity, ship.transform);
            pirateList.Add(pirate);
            pirate.SetActive(false);
        }
    }

    void UpdatePirateVisibility()
    {
        bool isDaytime = dayAndNight.IsDaytime;
        foreach (GameObject pirate in allDayPirates)
        {
            pirate.SetActive(isDaytime);
        }
        foreach (GameObject pirate in allNightPirates)
        {
            pirate.SetActive(!isDaytime);
        }
    }
}
