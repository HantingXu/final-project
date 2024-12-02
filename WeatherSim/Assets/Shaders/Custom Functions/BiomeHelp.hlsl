#pragma once

float Remap(float originalVal, float originalMin, float originalMax, float newMin, float newMax)
{
    return newMin + ((originalVal - originalMin) / (originalMax - originalMin)) * (newMax - newMin);
}

float2 CalculateUV(float2 uv)
{
    float v = uv.y;
    float latitude = (v - 0.5f) * PI;
    float adjustedV = 0.5f + 0.5f * sin(latitude);
    return float2(uv.x, adjustedV);
}

float InFoamRange(float2 uv, float threshold)
{
    float2 newUV = CalculateUV(uv);
    //float InFoamRange = _HeightMap.SampleLevel(SamplerState_Linear_Repeat, newUV, 0);

    //ds
    float2 uvSamples[5];
    
    uvSamples[0] = uv - float2(_FoamRange, 0);
    uvSamples[1] = uv + float2(_FoamRange, 0);
    uvSamples[2] = uv + float2(0, _FoamRange);
    uvSamples[3] = uv - float2(0, _FoamRange);
    uvSamples[4] = uv;
    /*
    uvSamples[0] = newUV - float2(_FoamRange, 0);
    uvSamples[1] = newUV + float2(_FoamRange, 0);
    uvSamples[2] = newUV + float2(0, _FoamRange);
    uvSamples[3] = newUV - float2(0, _FoamRange);
    uvSamples[4] = newUV;*/

    float inRange = 1.0f;

    for(int i = 0; i < 4 ; i++)
    {
        float2 UV = CalculateUV(uvSamples[i]);
        //inRange = inRange * step(_HeightMap.SampleLevel(SamplerState_Linear_Repeat, uvSamples[i], 0), threshold);
        inRange = inRange * step(_HeightMap.SampleLevel(SamplerState_Linear_Repeat, UV, 0), threshold);
    }

    return inRange;//step(inRange, 0.00001);
}

void ComputeBiomeColor_float(float2 mapUV, float2 uv, float height, out float3 col)
{
    float timer = _Time;
    float heightChange = sin(timer * 2.0f) * 0.001f - 0.001f;
    float3 biomeColor = _BiomeTex.SampleLevel(SamplerState_Linear_Repeat, uv, 0);

    float2 UV = CalculateUV(mapUV);
    float height1 = _HeightMap.SampleLevel(SamplerState_Linear_Repeat, UV, 0) + heightChange;
    float heightThresh = 0.002f - heightChange * 0.6f;
    if  ( height1 < heightThresh)
    {
        float normalizeHeight = Remap(height1, 0.0, heightThresh, 0.0f, 1.0f);//float3(0,0,1);
        col = lerp(_WaterColor, biomeColor, normalizeHeight);
        float foam =  step(_FoamTex.SampleLevel(SamplerState_Linear_Repeat, UV * 100.0f * float2(2.0f, 1.0f) + float2(2.0f * timer, timer) * 0.4f, 0), 0.08 + normalizeHeight * 0.4f);
        float inFoam = (1.0f - InFoamRange(mapUV, heightThresh)) * foam;
        col = (1.0f - inFoam) * col + inFoam * float3(1,1,1);
    }
    else
    {
        col = biomeColor;
    }
}