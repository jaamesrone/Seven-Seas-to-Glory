using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public int numberOfEnemies = 6;
    public float spawnRadius = 5f; // Adjust this radius as needed

    void Start()
    {
        InvokeRepeating("SpawnEnemies", 1, 300);
    }

    void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // random offset between a circle
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;

            // a vector3 for the position of the parent game object for spawning
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, 0, randomOffset.y);

            // setting the y cordinate to 5 so pirates can spawn above the ship.
            spawnPosition.y = 5;

            // instantiate the pirates at the spawn position
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
