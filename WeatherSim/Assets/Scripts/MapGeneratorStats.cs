using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

[System.Serializable]
public struct NoiseLayer
{
    public Vector3 center;
    [Range(1, 8)] public float numLayers;
    public float baseRoughness;
    public float roughness;
    public float persistence;
    public float strength;
    public float minValue;
    [Range(0, 1)] public int useFirstLayerAsMask;
    [Range(0, 1)] public int enabled;
};

[CreateAssetMenu(menuName = "GAME STATS/Map Generator Stats")]
public class MapGeneratorStats : ScriptableObject
{
    [Header("Height Map Parameters")]
    public NoiseLayer[] noiseLayers;
    public RenderTexture HeightMapTexture;
    public ComputeShader HeightMapShader;

    [Header("Water Map Parameters")]
    [Range(0f, 1f)] public float WaterRatio = 0.4f;
    public RenderTexture WaterMapTexture;
    public ComputeShader WaterMapShader;

    [Header("Tempurature Map Parameters")]
    [Range(0f, 10f)] public float LatitudeWeight = 1f;
    [Range(0f, 10f)] public float HeightWeight = 1f;
    public RenderTexture TemperatureMapTexture;
    public ComputeShader TemperatureMapShader;

    [Header("Cloud Map Parameters")]
    [Range(0, 30)] public int CenterNumber = 18;
    public RenderTexture CloudMapTexture;
    public ComputeShader CloudMapShader;
}
