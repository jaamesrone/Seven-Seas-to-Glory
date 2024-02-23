using UnityEngine;

public class EnemyShipSpawner : MonoBehaviour
{
    public GameObject enemyShipPrefab; // enemy ship model
    public GameObject piratePrefab; // pirate model
   
    public int numberOfShipsToSpawn; // number of enemy ships to spawn
    public int numberOfPirates; 
    public float spawnRadius; // radius within which enemy ships will be spawned

    void Start()
    {
        SpawnEnemyShips();
    }

    void SpawnEnemyShips()
    {
        for (int i = 0; i < numberOfShipsToSpawn; i++)
        {
            // vector 3 random position within the spawn radius
            Vector3 randomPosition = transform.position + Random.insideUnitSphere * spawnRadius;

            // setting the enemy ships at y = 0 so it's on the ocean surface.
            randomPosition.y = 0f;

            // setting a random rotation for the enemy ships so each ship runs in a random direction
            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            // spawn the enemy ship prefab at the random position with the random rotation
            GameObject enemyShip = Instantiate(enemyShipPrefab, randomPosition, randomRotation);

            // instantiate pirate prefabs on the enemy ship
            SpawnPiratesOnShip(enemyShip);
        }
    }

    void SpawnPiratesOnShip(GameObject ship)
    {
        for (int i = 0; i < numberOfPirates; i++)
        {
            // a random position within the ship's bounds
            Vector3 piratePosition = ship.transform.position + Random.insideUnitSphere * 5f; // Adjust the radius as needed

            // the pirate is positioned above the ship's surface
            piratePosition.y = ship.transform.position.y + 6f; // Adjust the height as needed

            // instantiate the pirate prefab on the ship
            GameObject pirate = Instantiate(piratePrefab, piratePosition, Quaternion.identity);

            // parenting the pirate's to the ship
            pirate.transform.parent = ship.transform.Find("Ship").Find("ship_main");
        }
    }
}
