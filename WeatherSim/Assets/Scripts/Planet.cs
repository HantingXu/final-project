using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;

    public PlanetSettings planetSettings;
    public MapGeneratorStats mapSettings;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    private float timer;
    private int shouldUpdate;
    private CloudMapGenerator cloudMapGenerator;
    private BiomeMapGenerator biomeMapGenerator;
    private WeatherMapGenerator weatherMapGenerator;

    public RenderTexture renderTexture;

    private void Start()
    {
        timer = 0.0f;
        shouldUpdate = 0;

        //renderTexture = mapSettings.WeatherMapTexture;
        //ExportRenderTexture("Assets/ExportedImage.png");
    }

    private void Update()
    {
        if (shouldUpdate == mapSettings.weatherUpdateRate) {
            timer += Time.deltaTime;
            cloudMapGenerator.UpdateTime(timer);
            weatherMapGenerator.GenerateMap();
            shouldUpdate = 0;
        } else
        {
            shouldUpdate += 1;
        }
    }

    public void OnValidate()
    {
        Initialize();
        GenerateMesh();
    }
    void Initialize()
    {
        cloudMapGenerator = new CloudMapGenerator(mapSettings);
        biomeMapGenerator = new BiomeMapGenerator(mapSettings);
        weatherMapGenerator = new WeatherMapGenerator(mapSettings);

        GenerateTexture();
        GenerateMap();

        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = {Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back};

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = planetSettings.planetMaterial;
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }
            terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh, planetSettings.planetRadius, resolution, directions[i]);
        }
    }

    void GenerateTexture()
    {
        biomeMapGenerator.GenerateBlueNosie();
    }
    void GenerateMap()
    {
        HeightMapGenerator heightMapGenerator = new HeightMapGenerator(mapSettings);
        heightMapGenerator.GenerateMap();
        WaterMapGenerator waterMapGenerator = new WaterMapGenerator(mapSettings);
        waterMapGenerator.GenerateMap();
        TemperatureMapGenerator temperatureMapGenerator = new TemperatureMapGenerator(mapSettings);
        temperatureMapGenerator.GenerateMap();
        BiomeMapGenerator biomeMapGenerator = new BiomeMapGenerator(mapSettings);
        biomeMapGenerator.GenerateMap();
        cloudMapGenerator.GenerateMap();
        weatherMapGenerator.GenerateMap();
    }

    public void OnPlanetSettingsUpdated()
    {
        Initialize();
        GenerateMesh();
    }

    public void OnMapSettingsUpdated()
    {
        GenerateMap();
    }

    public void ExportRenderTexture(string filePath)
    {
        // Set the active RenderTexture
        RenderTexture activeRenderTexture = RenderTexture.active;
        RenderTexture.active = renderTexture;

        // Create a new Texture2D with the same dimensions as the RenderTexture
        Texture2D texture2D = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RG32, false);

        // Read the RenderTexture into the Texture2D
        texture2D.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        texture2D.Apply();

        // Reset the active RenderTexture
        RenderTexture.active = activeRenderTexture;

        // Encode the Texture2D to PNG format
        byte[] bytes = texture2D.EncodeToPNG();

        // Save the encoded image to disk
        File.WriteAllBytes(filePath, bytes);

        // Clean up
        Destroy(texture2D);
    }

    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }
}
