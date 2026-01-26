#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

public class CreateAndSaveTerrainEditor : MonoBehaviour
{
    public int heightmapResolution = 513;   // 513, 1025, etc.
    public float terrainSize = 2000f;
    public float noiseScale = 30f;
    public float heightMultiplier = 600f;

    [ContextMenu("Create and Save Random Terrain")]
    public void CreateAndSaveRandomTerrain()
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

        // 3. Save TerrainData as an asset
        string folder = "Assets/SavedTerrains";
        if (!AssetDatabase.IsValidFolder(folder))
        {
            string parent = "Assets";
            AssetDatabase.CreateFolder(parent, "SavedTerrains");
        }

        string terrainDataPath = AssetDatabase.GenerateUniqueAssetPath($"{folder}/ProceduralTerrainData.asset");
        AssetDatabase.CreateAsset(data, terrainDataPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Saved TerrainData to: {terrainDataPath}");

        // 4. Create Terrain GameObject
        GameObject terrainGO = Terrain.CreateTerrainGameObject(data);
        terrainGO.name = "Procedural Terrain";

        // 5. Save Terrain GameObject as a prefab
        string prefabPath = AssetDatabase.GenerateUniqueAssetPath($"{folder}/ProceduralTerrain.prefab");
        PrefabUtility.SaveAsPrefabAsset(terrainGO, prefabPath, out bool success);
        if (success)
        {
            Debug.Log($"Saved Terrain prefab to: {prefabPath}");
        }
        else
        {
            Debug.LogWarning("Failed to save Terrain prefab.");
        }

        // 6. (Optional) Mark scene dirty so Unity asks you to save it
        UnityEditor.EditorUtility.SetDirty(terrainGO);
    }
}
#endif