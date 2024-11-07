using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class HeightMapGenerator : MonoBehaviour
{
    public ComputeShader heightMapComputeShader;
    public RenderTexture heightMapTexture;
    public float scale = 10f;
    public float heightMultiplier = 5f;

    private int width;
    private int height;

    private void Start()
    {
        if (heightMapTexture == null)
        {
            Debug.LogWarning("Please assign a RenderTexture to the HeightMapGenerator.");
            return;
        }
        width = heightMapTexture.width;
        height = heightMapTexture.height;
        GenerateHeightMap();
    }
    public void GenerateHeightMap()
    {

        // Set parameters for the compute shader
        heightMapComputeShader.SetFloat("scale", scale);
        heightMapComputeShader.SetFloat("heightMultiplier", heightMultiplier);
        heightMapComputeShader.SetInt("width", width);
        heightMapComputeShader.SetInt("height", height);
        heightMapComputeShader.SetTexture(0, "Result", heightMapTexture);

        // Dispatch the compute shader
        int threadGroupsX = Mathf.CeilToInt(width / 8.0f);
        int threadGroupsY = Mathf.CeilToInt(height / 8.0f);

        heightMapComputeShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);
    }
}
