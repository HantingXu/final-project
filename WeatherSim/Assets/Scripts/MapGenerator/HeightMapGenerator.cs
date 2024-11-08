using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class HeightMapGenerator : IMapGenerator
{
    private MapGeneratorStats parameter;

    public HeightMapGenerator(MapSetup mapSettings)
    {
        parameter = mapSettings.parameter;
    }

    public void GenerateMap()
    {
        if (parameter.HeightMapTexture == null)
        {
            Debug.LogWarning("Please assign a RenderTexture to the HeightMapGenerator.");
            return;
        }
        int width = parameter.HeightMapTexture.width;
        int height = parameter.HeightMapTexture.height;

        parameter.HeightMapShader.SetFloat("scale", parameter.Scale);
        parameter.HeightMapShader.SetFloat("heightMultiplier", parameter.HeightMultiplier);
        parameter.HeightMapShader.SetInt("width", width);
        parameter.HeightMapShader.SetInt("height", height);
        parameter.HeightMapShader.SetTexture(0, "Result", parameter.HeightMapTexture);
        // Dispatch the compute shader
        int threadGroupsX = Mathf.CeilToInt(width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(height / 8.0f);

        parameter.HeightMapShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
    }
}
