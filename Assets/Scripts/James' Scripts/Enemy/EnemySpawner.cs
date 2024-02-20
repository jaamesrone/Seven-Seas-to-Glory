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
            // Calculate random offset within spawnRadius
            Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;

            // Use the position of the parent game object for spawning
            Vector3 spawnPosition = transform.position + new Vector3(randomOffset.x, 0, randomOffset.y);

            // Set the y-coordinate to 0
            spawnPosition.y = 5;

            // Instantiate enemy at the spawn position
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
