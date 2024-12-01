#pragma once

void ComputeSun_float(float4 uv, float3 sunDirection, out float sunDisc)
{
    float sun = distance(uv.xyz, -sunDirection);
    sunDisc = 1 - smoothstep(0.75, 1, (sun / _SunRadius));
}