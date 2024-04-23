using UnityEngine;

public class OceanGenerator : MonoBehaviour
{
    public GameObject waterTilePrefab; // Assign your water tile prefab with the Shader Graph material
    public int tileCount = 3;
    public float height = 3; // The number of tiles in each direction from the center

    private float tileSize;

    void Start()
    {
        tileSize = waterTilePrefab.GetComponent<Renderer>().bounds.size.x; // Assuming a square tile
        GenerateTiles();
    }

    void GenerateTiles()
    {
        for (int i = -tileCount; i <= tileCount; i++)
        {
            for (int j = -tileCount; j <= tileCount; j++)
            {
                Vector3 position = new Vector3(i * tileSize, height, j * tileSize);
                Instantiate(waterTilePrefab, position, Quaternion.identity, transform);
            }
        }
    }
}
