#pragma once

void ComputeWaterNormal_float(float2 originalUV, float inWater, float2 timeScale, out float3 newNormal)
{
    float3 sampledNormal = _WaterNormalTex.SampleLevel(SamplerState_Linear_Repeat, originalUV * float2(80.0f, 40.0f) + _Time * timeScale * 0.3f, 0);
    newNormal = sampledNormal * inWater + (1.0f - inWater) * float3(0,1,0);

}
