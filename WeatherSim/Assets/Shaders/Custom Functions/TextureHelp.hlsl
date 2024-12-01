#pragma once

SAMPLER(sampler_point_clamp);

void CalculateNewPos_float(float3 pos, out float3 newPos)
{
    float radius = length(pos);
    float3 unitPos = normalize(pos);
    float u = atan2(unitPos.z, unitPos.x) / (2 * PI) + 0.5f;
    float v = asin(unitPos.y) / PI + 0.5f;
    float heightValue = _HeightMap.SampleLevel(sampler_point_clamp, float2(u, v), 0).r;
    newPos = unitPos * (radius + heightValue);
}

void CalculateUV_float(float3 pos, out float2 uv)
{
    float3 unitPos = normalize(pos);
    float u = atan2(unitPos.z, unitPos.x) / (2 * PI) + 0.5f;
    float v = asin(unitPos.y) / PI + 0.5f;
    uv = float2(u,v);
}

float3 uvToPos(float2 uv) {
    float theta = uv.x * 2.f * PI;
    float phi = (uv.y - 0.5f) * PI;

    float x = cos(phi) * cos(theta);
    float y = sin(phi);
    float z = cos(phi) * sin(theta);
    
    return float3(x, y, z);
}

void CalculateSmoothNormal_float(float3 pos, out float3 normal)
{
    float radius = length(pos);
    float3 unitPos = normalize(pos);

    float u = atan2(unitPos.z, unitPos.x) / (2.0f * PI) + 0.5f;
    float v = asin(unitPos.y) / PI + 0.5f;

    float offsetU = 1.0f / 1024.0f;
    float offsetV = 1.0f / 512.0f; 

    float heightCenter = _HeightMap.SampleLevel(sampler_point_clamp, float2(u, v), 0).r;
    float heightUPos = _HeightMap.SampleLevel(sampler_point_clamp, float2(u + offsetU, v), 0).r;
    float heightUNeg = _HeightMap.SampleLevel(sampler_point_clamp, float2(u - offsetU, v), 0).r;
    float heightVPos = _HeightMap.SampleLevel(sampler_point_clamp, float2(u, v + offsetV), 0).r;
    float heightVNeg = _HeightMap.SampleLevel(sampler_point_clamp, float2(u, v - offsetV), 0).r;

    // Convert UV coordinates back to 3D positions on the sphere
    float3 posCenter = uvToPos(float2(u, v)) * (radius + heightCenter);
    float3 posUPos = uvToPos(float2(u + offsetU, v)) * (radius + heightUPos);
    float3 posUNeg = uvToPos(float2(u - offsetU, v)) * (radius + heightUNeg);
    float3 posVPos = uvToPos(float2(u, v + offsetV)) * (radius + heightVPos);
    float3 posVNeg = uvToPos(float2(u, v - offsetV)) * (radius + heightVNeg);

    // Calculate tangent vectors
    float3 tangentU = posUPos - posUNeg; // Tangent in U direction
    float3 tangentV = posVPos - posVNeg; // Tangent in V direction

    normal = normalize(cross(tangentU, tangentV));

    normal = normalize(mul((float3x3)unity_ObjectToWorld, normal));
}