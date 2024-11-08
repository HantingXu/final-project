using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GAME STATS/Map Generator Stats")]
public class MapGeneratorStats : ScriptableObject
{
    [Header("Height Map Parameters")]
    [Range(0f, 1000f)] public float HeightMultiplier = 10f;
    [Range(0f, 100f)] public float Scale = 5f;
    public RenderTexture HeightMapTexture;
    public ComputeShader HeightMapShader;

    [Header("Water Map Parameters")]
    [Range(0f, 1f)] public float WaterRatio = 0.4f;
    public RenderTexture WaterMapTexture;
    public ComputeShader WaterMapShader;
}
