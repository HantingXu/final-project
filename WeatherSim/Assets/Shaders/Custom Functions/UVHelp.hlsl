#pragma once

void CalculatePos_float(float2 uv, out float3 pos)
{
    float theta = uv.x * 2.f * PI;
    float phi = (uv.y - 0.5f) * PI;

    float x = cos(phi) * cos(theta);
    float y = sin(phi);
    float z = cos(phi) * sin(theta);

    pos = float3(x, y, z);
}

void CalculateUV_float(float3 pos, out float2 uv)
{
    // Normalize the input to ensure it's on the unit sphere
    float3 unitPos = normalize(pos);

    // Compute longitude (theta) and latitude (phi)
    float theta = atan2(unitPos.z, unitPos.x); // Longitude
    float phi = asin(unitPos.y);              // Latitude

    // Convert to UV coordinates
    float u = theta / (2.0f * PI) + 0.5f;
    float v = phi / PI + 0.5f;

    uv = float2(u, v);
}