using System.Collections.Generic;
using UnityEngine;

public class EnemyShipSpawner : MonoBehaviour
{
    public GameObject enemyShipPrefab;
    public GameObject piratePrefab;
    public GameObject playerShip; // reference to the player's ship to avoid spawning near it
    public int numberOfShipsToSpawn;
    public int numberOfPirates;
    public float spawnRadius;
    public float minDistanceFromPlayer = 50f; // minimum distance from the player ship
    public float minDistanceFromOtherShips = 30f; // minimum distance from other enemy ships

    private List<Vector3> spawnedShipPositions = new List<Vector3>(); // to keep track of spawned ship positions

    void Start()
    {
        SpawnEnemyShips();
    }

    void SpawnEnemyShips()
    {
        for (int i = 0; i < numberOfShipsToSpawn; i++)
        {
            Vector3 randomPosition = Vector3.zero;
            bool positionFound = false;
            int attemptCounter = 0;

            while (!positionFound && attemptCounter < 100) // prevent infinite loop, attempt up to 100 times
            {
                attemptCounter++;
                randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;
                randomPosition.y = 0f; // ensure it's on the ocean surface

                if (Vector3.Distance(randomPosition, playerShip.transform.position) >= minDistanceFromPlayer && IsFarFromOtherShips(randomPosition))
                {
                    positionFound = true;
                }
            }

            if (positionFound)
            {
                Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
                GameObject enemyShip = Instantiate(enemyShipPrefab, randomPosition, randomRotation);
                spawnedShipPositions.Add(randomPosition); // keep track of spawned ship position

                SpawnPiratesOnShip(enemyShip);
            }
            else
            {
                Debug.LogWarning("Could not find a suitable position for enemy ship #" + (i + 1));
            }
        }
    }

    bool IsFarFromOtherShips(Vector3 position)
    {
        foreach (Vector3 otherPosition in spawnedShipPositions)
        {
            if (Vector3.Distance(position, otherPosition) < minDistanceFromOtherShips)
            {
                return false;
            }
        }
        return true;
    }

    void SpawnPiratesOnShip(GameObject ship)
    {
        for (int i = 0; i < numberOfPirates; i++)
        {
            Vector3 piratePosition = ship.transform.position + Random.insideUnitSphere * 5f;
            piratePosition.y = ship.transform.position.y + 6f;
            GameObject pirate = Instantiate(piratePrefab, piratePosition, Quaternion.identity);
            pirate.transform.parent = ship.transform.Find("Ship").Find("ship_main");
        }
    }
}
