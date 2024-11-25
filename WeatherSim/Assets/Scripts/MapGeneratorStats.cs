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
    public int weatherUpdateRate = 1000;

    [Header("Height Map Parameters")]
    public NoiseLayer[] noiseLayers;
    public RenderTexture HeightMapTexture;
    public ComputeShader HeightMapShader;

    [Header("Water Map Parameters")]
    [Range(0f, 1f)] public float WaterRatio = 0.4f;
    public RenderTexture WaterMapTexture;
    public ComputeShader WaterMapShader;

    [Header("Tempurature Map Parameters")]
    [Range(0f, 1f)] public float LatitudeWeight = 0.5f;
    [Range(0f, 1f)] public float HeightWeight = 0.5f;
    public RenderTexture TemperatureMapTexture;
    public ComputeShader TemperatureMapShader;

    [Header("Cloud Map Parameters")]
    [Range(0, 30)] public int CenterNumber = 18;
    public RenderTexture CloudMapTexture;
    public ComputeShader CloudMapShader;

    [Header("Biome Map Parameters")]
    public Color ShoreColor;
    public Color WaterColor;
    public RenderTexture BlueNoiseTexture;
    public RenderTexture BiomeMapTexture;
    public ComputeShader BiomeMapShader;

    [Header("Weather Map Parameters")]       
    public float nearThreshold = 0.5f;            
    public float veryHighThreshold = 0.95f;
    public float highThreshold = 0.8f;
    public float mediumThreshold = 0.5f;     
    public float lowThreshold = 0.2f;        
    public float hotThreshold = 0.8f;
    public float coldThreshold = 0.2f;
    public RenderTexture WeatherMapTexture;
    public ComputeShader WeatherMapShader;
}
