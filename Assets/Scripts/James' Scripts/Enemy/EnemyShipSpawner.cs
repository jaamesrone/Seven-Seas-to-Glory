using UnityEngine;

public class EnemyShipSpawner : MonoBehaviour
{
    public GameObject enemyShipPrefab; //enemyship model
    public int numberOfShipsToSpawn = 8; // number of enemy ships to spawn
    public float spawnRadius = 20f; // radius which enemy ships will be spawned

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

            // setting the enemyships at y = 0 so it's on the ocean surface.
            randomPosition.y = 0f;

            // setting a random rotation for the enemyships each ship runs in a random direction
            Quaternion randomRotation = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);

            // Instantiate the enemy ship prefab at the random position with the random rotation
            GameObject enemyShip = Instantiate(enemyShipPrefab, randomPosition, randomRotation);
        }
    }
}
