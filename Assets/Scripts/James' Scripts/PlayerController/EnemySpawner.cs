using System.Collections;
using UnityEngine;
public class EnemySpawner : MonoBehaviour
{
    public float minimumX;
    public float max_X;
    public float minimumZ;
    public float max_Z;

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
            float randomX = Random.Range(minimumX, max_X);
            float randomY = Random.Range(minimumZ, max_Z);

            Vector3 pos = new Vector3(randomX, 15, randomY);

            Instantiate(enemySpawner,pos,Quaternion.identity);
        }
    }
}
