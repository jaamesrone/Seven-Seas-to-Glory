using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Vector2 spawnRangeX;
    public Vector2 spawnRangeY;

    public GameObject enemySpawner;
    public int numberOfEnemies = 6;

    void Start()
    {
        InvokeRepeating("Spawner", 1, 300);
    }

    void Spawner()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            float randomX = Random.Range(spawnRangeX.x, spawnRangeX.y);
            float randomY = Random.Range(spawnRangeY.x, spawnRangeY.y);

            Vector3 pos = new Vector3(randomX, 15, randomY);

            Instantiate(enemySpawner, pos, Quaternion.identity, transform);
        }
    }
}
