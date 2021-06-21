using UnityEngine;
using System.Collections;

// MapDisplay class
// Turn the noisemap into a texture
public class MapPreview : MonoBehaviour
{
    // Reference to the renderer
    public Renderer textureRender;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    // Determine which map is going to be generated
    public enum DrawMode { NoiseMap, Mesh, FalloffMap };
    public DrawMode drawMode;

    public MeshSettings meshSettings;
    public HeightMapSettings heightMapSettings;
    public TextureData textureData;

    public Material terrainMaterial;

    [Range(0, MeshSettings.numSupportedLODs - 1)]                          // multiply the LOD by 2
    public int editorPreviewLOD;           // LOD

    public bool autoUpdate;

    // DrawMapInEditor method
    // Preview the map in the editor
    public void DrawMapInEditor()
    {
        textureData.ApplyToMaterial(terrainMaterial);
        textureData.UpdateMeshHeights(terrainMaterial, heightMapSettings.minHeight, heightMapSettings.maxHeight);
        HeightMap heightMap = HeightMapGenerator.GenerateHeightMap(meshSettings.numVertsPerLine, meshSettings.numVertsPerLine, heightMapSettings, Vector2.zero);

        // draw noise map
        if (drawMode == DrawMode.NoiseMap)
        {
            DrawTexture(TextureGenerator.TextureFromHeightMap(heightMap));
        }
        // draw mesh
        else if (drawMode == DrawMode.Mesh)
        {
            DrawMesh(MeshGenerator.GenerateTerrainMesh(heightMap.values, meshSettings, editorPreviewLOD));
        }
        else if (drawMode == DrawMode.FalloffMap)
        {
            DrawTexture(TextureGenerator.TextureFromHeightMap(new HeightMap(FalloffGenerator.GenerateFalloffMap(meshSettings.numVertsPerLine), 0, 1)));
        }
    }


    // DrawTexture method
    // Draw a texture to the screen
    public void DrawTexture(Texture2D texture)
    {
        // apply the texture to the texture renderer
        textureRender.sharedMaterial.mainTexture = texture;
        textureRender.transform.localScale = new Vector3(texture.width, 1, texture.height)/10f;
        textureRender.gameObject.SetActive(true);
        meshFilter.gameObject.SetActive(false);
    }

    // DrawMesh method
    // Draw a mesh to the screen
    public void DrawMesh(MeshData meshData)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        textureRender.gameObject.SetActive(false);
        meshFilter.gameObject.SetActive(true);
    }

    // OnValuesUpdated method
    void OnValuesUpdated()
    {
        if (!Application.isPlaying)
        {
            DrawMapInEditor();
        }
    }

    // OnTextureValuesUpdated method
    void OnTextureValuesUpdated()
    {
        textureData.ApplyToMaterial(terrainMaterial);
    }

    // OnValidate method
    // Called automatically whenever one of the scripts variables is changed in the inspector
    void OnValidate()
    {
        // if something in the inspector changes, call the On Values Updated event
        if (meshSettings != null)
        {
            meshSettings.OnValuesUpdated -= OnValuesUpdated;
            meshSettings.OnValuesUpdated += OnValuesUpdated;
        }
        if (heightMapSettings != null)
        {
            heightMapSettings.OnValuesUpdated -= OnValuesUpdated;
            heightMapSettings.OnValuesUpdated += OnValuesUpdated;
        }
        if (textureData != null)
        {
            textureData.OnValuesUpdated -= OnTextureValuesUpdated;
            textureData.OnValuesUpdated += OnTextureValuesUpdated;
        }
    }
}