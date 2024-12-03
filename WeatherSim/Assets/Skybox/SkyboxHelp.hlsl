#pragma once

void ComputeSun_float(float4 uv, float3 sunDirection, out float sunDisc)
{
    float sun = distance(uv.xyz, -sunDirection);
    sunDisc = 1 - smoothstep(0.75, 1, (sun / _SunRadius));
}

void Intensify_float(float weatherType, float intensity, float3 col, out float3 intensifiedCol) 
{
    // Rain
    if (weatherType == 1.f) {
        intensifiedCol = normalize(col) * clamp((length(col) - sqrt(clamp(intensity, 0.f, 0.8f))), 0.1f, 1.0f);
    }
    // Snow
    else if (weatherType == 2.f) {
        intensifiedCol = normalize(col) * clamp((length(col) + sqrt(intensity)), 0.0f, 1.0f);
    }
    else {
        intensifiedCol = col;
    }
}