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
    float u = atan2(unitPos.z, unitPos.x) / (2 * PI) + 0.5f;
    float v = asin(unitPos.y) / PI + 0.5f;

    float offsetU = 1.0f / 1024.0f;
    float offsetV = 1.0f / 512.0f;

    float heightCenter = _HeightMap.SampleLevel(sampler_point_clamp, float2(u, v), 0).r;
    float heightUPos = _HeightMap.SampleLevel(sampler_point_clamp, float2(u + offsetU, v), 0).r;
    float heightUNeg = _HeightMap.SampleLevel(sampler_point_clamp, float2(u - offsetU, v), 0).r;
    float heightVPos = _HeightMap.SampleLevel(sampler_point_clamp, float2(u, v + offsetV), 0).r;
    float heightVNeg = _HeightMap.SampleLevel(sampler_point_clamp, float2(u, v - offsetV), 0).r;

    float3 posUPos = normalize(uvToPos(float2(u + offsetU, v))) * (radius + heightUPos);
    float3 posUNeg = normalize(uvToPos(float2(u - offsetU, v))) * (radius + heightUNeg);
    float3 posVPos = normalize(uvToPos(float2(u, v + offsetV))) * (radius + heightVPos);
    float3 posVNeg = normalize(uvToPos(float2(u, v - offsetV))) * (radius + heightVNeg);

    float3 tangentU = 2 * (posUPos - posUNeg);   // Tangent in the U direction
    float3 tangentV = 2 * (posVPos - posVNeg);   // Tangent in the V direction

    // Step 6: Compute normal as average of cross-products
    normal = normalize(float3(length(tangentU), length(tangentV), sqrt(1 - dot(tangentU, tangentU) - dot(tangentV, tangentV))));

    normal = mul(unity_ObjectToWorld, normal);
}