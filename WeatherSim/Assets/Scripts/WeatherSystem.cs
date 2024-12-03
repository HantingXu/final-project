using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class WeatherSystem : MonoBehaviour
{
    public static WeatherSystem Instance { get; private set; }

    private int currentWeatherType = 0;
    private float currentWeatherIntensity = 0.0f;
    private int currentTargetWeatherType = 0;
    private float currentTargetWeatherIntensity = 0.0f;
    public ComputeShader weatherComputeShader;
    public RenderTexture WeatherMapTexture;
    private ComputeBuffer outputBuffer;

    private float transitionFactor = 0.0f;
    const float transitionSpeed = 0.00025f;
    private Vector3 playerPosition = Vector3.zero;

    private float timer;
    private int kernel;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        kernel = weatherComputeShader.FindKernel("CSMain");
        outputBuffer = new ComputeBuffer(2, sizeof(float));
        timer = 11.0f;
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > 10.0f)
        {
            weatherComputeShader.SetVector("PlayerPos", playerPosition);

            weatherComputeShader.SetBuffer(kernel, "OutputBuffer", outputBuffer);
            weatherComputeShader.SetTexture(kernel, "WeatherMap", WeatherMapTexture);

            weatherComputeShader.Dispatch(kernel, 1, 1, 1);

            AsyncGPUReadback.Request(outputBuffer, OnCompleteReadback);

            timer = 0.0f;
        }

        UpdateCurrentWeather(Time.deltaTime);
    }

    void OnCompleteReadback(AsyncGPUReadbackRequest request)
    {
        if (request.hasError)
        {
            Debug.LogError("GPU readback error detected.");
            return;
        }

        float[] data = request.GetData<float>().ToArray();
        currentTargetWeatherType = (int)data[0];
        currentTargetWeatherIntensity = data[1];
    }

    void OnDestroy()
    {
        if (outputBuffer != null)
        {
            outputBuffer.Release();
        }
    }

    void UpdateCurrentWeather(float deltaTime)
    {
        if (currentWeatherType != currentTargetWeatherType)
        {
            if (currentWeatherIntensity < 0.0001f)
            {
                currentWeatherType = currentTargetWeatherType;
                transitionFactor = 0.0f; // start intensity transition
            }
            else
            {
                transitionFactor += deltaTime * transitionSpeed;
                currentWeatherIntensity = Mathf.Lerp(currentWeatherIntensity, 0.0f, transitionFactor);
            }
        }
        else
        {
            if (currentWeatherIntensity != currentTargetWeatherIntensity)
            {
                transitionFactor += deltaTime * transitionSpeed;
                currentWeatherIntensity = Mathf.Lerp(currentWeatherIntensity, currentTargetWeatherIntensity, transitionFactor);
            }
            else
            {
                transitionFactor = 0.0f;
            }
        }
        //Debug.Log("Current Type: " + currentWeatherType + "Current Intensity: " + currentWeatherIntensity);
        //Debug.Log("Current Target Type: " + currentTargetWeatherType + "Current Target Intensity: " + currentTargetWeatherIntensity);
    }

    public void setPlayerPosition(Vector3 pos)
    {
        playerPosition = pos;
    }

    public float getCurrentWeatherType()
    {
        return (float)currentWeatherType;
    }

    public float getCurrentWeatherIntensity()
    {
        return currentWeatherIntensity;
    }
}
