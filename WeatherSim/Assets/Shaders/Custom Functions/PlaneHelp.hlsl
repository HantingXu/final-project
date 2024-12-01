#pragma once

SAMPLER(sampler_point_clamp);

float2 getAdjustedUV(float2 uv)
{
    float v = uv.y;
    float latitude = (v - 0.5f) * PI; 
    float adjustedV = 0.5f + 0.5f * sin(latitude);
    return float2(uv.x, adjustedV);
}

void CalculateUV_float(float2 uv, out float2 newUV)
{
    float v = uv.y;
    float latitude = (v - 0.5f) * PI;
    float adjustedV = 0.5f + 0.5f * sin(latitude);
    newUV =  float2(uv.x, adjustedV);
}

void CalculateHeight_float(float2 uv, out float height)
{
    float2 adjustedUV = getAdjustedUV(uv);
    height = _HeightMap.SampleLevel(sampler_point_clamp, adjustedUV, 0).r;
}

void CalculateSmoothNormal_float(float2 uv, float planeSize, out float3 normal)
{
    float u = uv.x;
    float v = uv.y;

    float offsetU = 1.0f / 1024.0f;
    float offsetV = 1.0f / 512.0f;

    float2 uvCenter = getAdjustedUV(float2(u, v));
    float2 uvUPos = getAdjustedUV(float2(u + offsetU, v));
    float2 uvUNeg = getAdjustedUV(float2(u - offsetU, v));
    float2 uvVPos = getAdjustedUV(float2(u, v + offsetV));
    float2 uvVNeg = getAdjustedUV(float2(u, v - offsetV));

    float heightCenter = _HeightMap.SampleLevel(sampler_point_clamp, uvCenter, 0).r;
    float heightUPos = _HeightMap.SampleLevel(sampler_point_clamp, uvUPos, 0).r;
    float heightUNeg = _HeightMap.SampleLevel(sampler_point_clamp, uvUNeg, 0).r;
    float heightVPos = _HeightMap.SampleLevel(sampler_point_clamp, uvVPos, 0).r;
    float heightVNeg = _HeightMap.SampleLevel(sampler_point_clamp, uvVNeg, 0).r;

    
    float HalfSize = planeSize / 2.f;
    float xPos = -HalfSize + uvCenter.x * 2.f * planeSize;
    float xPosUPos = -HalfSize + uvUPos.x * 2.f * planeSize;
    float xPosUNeg = -HalfSize + uvUNeg.x * 2.f * planeSize;
    float zPos = -HalfSize + uvCenter.y * planeSize;
    float zPosVPos = -HalfSize + uvVPos.y * planeSize;
    float zPosVNeg = -HalfSize + uvVNeg.y * planeSize;

    // Positions with heights
    float3 posCenter = float3(xPos, heightCenter, zPos);
    float3 posUPos = float3(xPosUPos, heightUPos, zPos);
    float3 posUNeg = float3(xPosUNeg, heightUNeg, zPos);
    float3 posVPos = float3(xPos, heightVPos, zPosVPos);
    float3 posVNeg = float3(xPos, heightVNeg, zPosVNeg);

    // Calculate tangent vectors
    float3 tangentU = posUPos - posUNeg; // Tangent in U direction
    float3 tangentV = posVPos - posVNeg; // Tangent in V direction

    normal = normalize(cross(tangentU, tangentV));

    normal = normalize(mul((float3x3)unity_ObjectToWorld, normal));
}