using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMapGenerator : IMapGenerator
{
    private MapGeneratorStats parameter;
    private List<float> centers;
    private int centerNumber;

    public CloudMapGenerator(MapGeneratorStats mapSettings)
    {
        parameter = mapSettings;
        centers = new List<float>();
        centerNumber = parameter.CenterNumber;
    }

    public void GenerateMap()
    {
        if (parameter.CloudMapTexture == null)
        {
            Debug.LogWarning("Please assign a RenderTexture to the CloudMapGenerator.");
            return;
        }
        int width = parameter.CloudMapTexture.width;
        int height = parameter.CloudMapTexture.height;
        int cloudKernel = parameter.CloudMapShader.FindKernel("GenerateCloudMapCS");
        GenerateRandomPositions();

        ComputeBuffer centerBuffer = new ComputeBuffer(centerNumber * 2, sizeof(float));
        centerBuffer.SetData(centers.ToArray());

        parameter.CloudMapShader.SetInt("width", width);
        parameter.CloudMapShader.SetInt("height", height);
        parameter.CloudMapShader.SetInt("centerNumber", centerNumber);
        parameter.CloudMapShader.SetFloat("cloudCoverage", parameter.CloudCoverage);
        parameter.CloudMapShader.SetFloat("cloudShape", parameter.CloudShape);
        parameter.CloudMapShader.SetFloat("cloudIntensity", parameter.CloudIntensity);
        parameter.CloudMapShader.SetBuffer(cloudKernel, "centers", centerBuffer);
        parameter.CloudMapShader.SetTexture(0, "Result", parameter.CloudMapTexture);
        // Dispatch the compute shader
        int threadGroupsX = Mathf.CeilToInt(width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(height / 8.0f);

        parameter.CloudMapShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
    }

    public void UpdateTime(float time)
    {
        parameter.CloudMapShader.SetFloat("time", time);
        int width = parameter.CloudMapTexture.width;
        int height = parameter.CloudMapTexture.height;
        /*
        int cloudKernel = parameter.CloudMapShader.FindKernel("GenerateCloudMapCS");
        GenerateRandomPositions();

        ComputeBuffer centerBuffer = new ComputeBuffer(centerNumber * 2, sizeof(float));
        centerBuffer.SetData(centers.ToArray());

        parameter.CloudMapShader.SetInt("width", width);
        parameter.CloudMapShader.SetInt("height", height);
        parameter.CloudMapShader.SetInt("centerNumber", centerNumber);
        parameter.CloudMapShader.SetBuffer(cloudKernel, "centers", centerBuffer);
        parameter.CloudMapShader.SetTexture(0, "Result", parameter.CloudMapTexture);*/
        // Dispatch the compute shader
        int threadGroupsX = Mathf.CeilToInt(width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(height / 8.0f);
        parameter.CloudMapShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
    }

    private void GenerateRandomPositions()
    {
        for (int i = 0; i < centerNumber * 2; i++)
        {
            centers.Add(Random.Range(0.03f, 0.98f));
            //Debug.Log(centers[i]);
        }
    }

}
