using UnityEngine;

public class CreateTerrainFromScratch : MonoBehaviour
{
    public int heightmapResolution = 513;  // typical power-of-two + 1: 513, 1025, etc.
    public float terrainSize = 2000f;
    public float noiseScale = 30f;
    public float heightMultiplier = 600f;

    private void Start()
    {
        CreateRandomTerrain();
    }

    [ContextMenu("Create Random Terrain")]
    public void CreateRandomTerrain()
    {
        // 1. Create TerrainData
        TerrainData data = new TerrainData();
        data.heightmapResolution = heightmapResolution;
        data.size = new Vector3(terrainSize, heightMultiplier, terrainSize);

        // 2. Generate heightmap
        int w = heightmapResolution;
        int h = heightmapResolution;
        float[,] heights = new float[w, h];

        float offsetX = Random.Range(0f, 1000f);
        float offsetY = Random.Range(0f, 1000f);

        for (int x = 0; x < w; x++)
        {
            for (int y = 0; y < h; y++)
            {
                float xCoord = (float)x / w * noiseScale + offsetX;
                float yCoord = (float)y / h * noiseScale + offsetY;

                float noiseValue = Mathf.PerlinNoise(xCoord, yCoord);
                heights[x, y] = noiseValue;
            }
        }

        data.SetHeights(0, 0, heights);

        // 3. Create Terrain GameObject
        GameObject terrainGO = Terrain.CreateTerrainGameObject(data);
        terrainGO.name = "Procedural Terrain";
    }
}