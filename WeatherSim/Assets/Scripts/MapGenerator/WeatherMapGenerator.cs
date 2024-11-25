using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class WeatherMapGenerator : IMapGenerator
{
    private MapGeneratorStats parameter;

    public WeatherMapGenerator(MapGeneratorStats mapSettings)
    {
        parameter = mapSettings;
    }

    public void GenerateMap()
    {
        if (parameter.WeatherMapTexture == null)
        {
            Debug.LogWarning("Please assign a RenderTexture to the WeatherMapGenerator.");
            return;
        }
        int width = parameter.WeatherMapTexture.width;
        int height = parameter.WeatherMapTexture.height;

        parameter.WeatherMapShader.SetInt("width", width);
        parameter.WeatherMapShader.SetInt("height", height);
        parameter.WeatherMapShader.SetFloat("highThreshold", parameter.highThreshold);
        parameter.WeatherMapShader.SetFloat("nearThreshold", parameter.nearThreshold);
        parameter.WeatherMapShader.SetFloat("coldThreshold", parameter.coldThreshold);
        parameter.WeatherMapShader.SetFloat("veryHighThreshold", parameter.veryHighThreshold);
        parameter.WeatherMapShader.SetFloat("mediumThreshold", parameter.mediumThreshold);
        parameter.WeatherMapShader.SetFloat("lowThreshold", parameter.lowThreshold);
        parameter.WeatherMapShader.SetFloat("hotThreshold", parameter.hotThreshold);
        parameter.WeatherMapShader.SetTexture(0, "HeightMap", parameter.HeightMapTexture);
        parameter.WeatherMapShader.SetTexture(0, "CloudMap", parameter.CloudMapTexture);
        parameter.WeatherMapShader.SetTexture(0, "WaterMap", parameter.WaterMapTexture);
        parameter.WeatherMapShader.SetTexture(0, "TemperatureMap", parameter.TemperatureMapTexture);
        parameter.WeatherMapShader.SetTexture(0, "Result", parameter.WeatherMapTexture);
        // Dispatch the compute shader
        int threadGroupsX = Mathf.CeilToInt(width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(height / 8.0f);

        parameter.WeatherMapShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
    }
}
