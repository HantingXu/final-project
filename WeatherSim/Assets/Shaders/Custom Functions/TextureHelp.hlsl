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
    float radius = length(pos);
    float3 unitPos = normalize(pos);
    float u = atan2(unitPos.z, unitPos.x) / (2 * PI) + 0.5f;
    float v = asin(unitPos.y) / PI + 0.5f;
    uv = float2(u,v);
}