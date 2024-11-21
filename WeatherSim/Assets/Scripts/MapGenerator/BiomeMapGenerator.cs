using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BiomeMapGenerator : IMapGenerator
{
    private MapGeneratorStats parameter;

    public BiomeMapGenerator(MapGeneratorStats mapSettings)
    {
        parameter = mapSettings;
    }

    public void GenerateBlueNosie()
    {
        if (parameter.BlueNoiseTexture == null)
        {
            Debug.LogWarning("Please assign a RenderTexture to the BlueNoise.");
            return;
        }

        int width = parameter.BlueNoiseTexture.width;
        int height = parameter.BlueNoiseTexture.height;
        int blueNoiseKernel = parameter.BiomeMapShader.FindKernel("GenerateBlueNoiseCS");
        parameter.BiomeMapShader.SetInt("width", width);
        parameter.BiomeMapShader.SetInt("height", height);
        parameter.BiomeMapShader.SetTexture(blueNoiseKernel, "BlueNoise", parameter.BlueNoiseTexture);
        // Dispatch the compute shader
        int threadGroupsX = Mathf.CeilToInt(width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(height / 8.0f);
        parameter.BiomeMapShader.Dispatch(blueNoiseKernel, threadGroupsX, threadGroupsY, 1);
    }

    public void GenerateMap()
    {
        
        if (parameter.BiomeMapTexture == null)
        {
            Debug.LogWarning("Please assign a RenderTexture to the CloudMapGenerator.");
            return;
        }

        int width = parameter.BiomeMapTexture.width;
        int height = parameter.BiomeMapTexture.height;
        int biomeKernel = parameter.BiomeMapShader.FindKernel("GenerateBiomeMapCS");
        parameter.BiomeMapShader.SetInt("width", width);
        parameter.BiomeMapShader.SetInt("height", height);
        parameter.BiomeMapShader.SetVector("waterColor", parameter.WaterColor);
        parameter.BiomeMapShader.SetTexture(biomeKernel, "BlueNoise", parameter.BlueNoiseTexture);
        parameter.BiomeMapShader.SetTexture(biomeKernel, "Result", parameter.BiomeMapTexture);
        parameter.BiomeMapShader.SetTexture(biomeKernel, "TemperatureMap", parameter.TemperatureMapTexture);
        parameter.BiomeMapShader.SetTexture(biomeKernel, "WaterMap", parameter.WaterMapTexture);
        parameter.BiomeMapShader.SetTexture(biomeKernel, "HeightMap", parameter.HeightMapTexture);
        // Dispatch the compute shader
        int threadGroupsX = Mathf.CeilToInt(width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(height / 8.0f);

        parameter.BiomeMapShader.Dispatch(biomeKernel, threadGroupsX, threadGroupsY, 1);
    }
}
