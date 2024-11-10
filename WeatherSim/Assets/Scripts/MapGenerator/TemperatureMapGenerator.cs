using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemperatureMapGenerator : MonoBehaviour
{
    private MapGeneratorStats parameter;

    public TemperatureMapGenerator(MapSetup mapSettings)
    {
        parameter = mapSettings.parameter;
    }

    public void GenerateMap()
    {
        if (parameter.TemperatureMapTexture == null)
        {
            Debug.LogWarning("Please assign a RenderTexture to the WaterMapGenerator.");
            return;
        }
        int width = parameter.TemperatureMapTexture.width;
        int height = parameter.TemperatureMapTexture.height;

        parameter.TemperatureMapShader.SetTexture(0, "HeightMap", parameter.HeightMapTexture);
        parameter.TemperatureMapShader.SetFloat("heightMultiplier", parameter.HeightMultiplier);
        parameter.TemperatureMapShader.SetFloat("heightWeight", parameter.HeightWeight);
        parameter.TemperatureMapShader.SetFloat("latitudeWeight", parameter.LatitudeWeight);
        parameter.TemperatureMapShader.SetTexture(0, "Result", parameter.TemperatureMapTexture);
        parameter.TemperatureMapShader.SetInt("width", width);
        parameter.TemperatureMapShader.SetInt("height", height);

        // Dispatch the compute shader
        int threadGroupsX = Mathf.CeilToInt(width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(height / 8.0f);

        parameter.TemperatureMapShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
    }
}
