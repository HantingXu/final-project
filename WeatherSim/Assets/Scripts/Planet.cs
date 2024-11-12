using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour
{
    [Range(2, 256)]
    public int resolution = 10;

    public PlanetSettings planetSettings;
    public MapGeneratorStats mapSettings;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    public void OnValidate()
    {
        Initialize();
        GenerateMesh();
    }
    void Initialize()
    {
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

    void GenerateMap()
    {
        HeightMapGenerator heightMapGenerator = new HeightMapGenerator(mapSettings);
        heightMapGenerator.GenerateMap();
        WaterMapGenerator waterMapGenerator = new WaterMapGenerator(mapSettings);
        waterMapGenerator.GenerateMap();
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

    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }
}
