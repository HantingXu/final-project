using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterMapGenerator : IMapGenerator
{
    private MapGeneratorStats parameter;

    public WaterMapGenerator(MapSetup mapSettings)
    {
        parameter = mapSettings.parameter;
    }

    public void GenerateMap()
    {
        if (parameter.WaterMapTexture == null)
        {
            Debug.LogWarning("Please assign a RenderTexture to the WaterMapGenerator.");
            return;
        }
        int width = parameter.WaterMapTexture.width;
        int height = parameter.WaterMapTexture.height;

        parameter.WaterMapShader.SetTexture(0, "HeightMap", parameter.HeightMapTexture);
        parameter.WaterMapShader.SetFloat("heightMultiplier", parameter.HeightMultiplier);
        parameter.WaterMapShader.SetFloat("waterRatio", parameter.WaterRatio);
        parameter.WaterMapShader.SetTexture(0, "Result", parameter.WaterMapTexture);
        // Dispatch the compute shader
        int threadGroupsX = Mathf.CeilToInt(width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(height / 8.0f);

        parameter.WaterMapShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
    }
}