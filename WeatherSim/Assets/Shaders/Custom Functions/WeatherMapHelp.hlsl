#pragma once
SAMPLER(sampler_point_clamp);

float3 intensify(float3 color, float intensity) {
    float maxC = max(max(color.x, color.y), color.z);
    float3 result = color;
    if (maxC == color.x) {
        result.x = intensity;
    }
    else if (maxC == color.y){
        result.y = intensity;
    }
    else if (maxC == color.z) {
        result.z = intensity;
    }
    return result;
}

void AssignCorrectColor_float(float2 uv, float3 defaultColor, float3 rainyColor, float3 snowyColor, float3 cloudyColor, float3 sunnyColor, float3 partlyCloudyColor, out float3 color, out float intensity)
{
    float v = uv.y;
    float latitude = (v - 0.5f) * PI;
    float adjustedV = 0.5f + 0.5f * sin(latitude);
    float2 uv2 = float2(uv.x, adjustedV);
    float weatherType = _WeatherMap.SampleLevel(sampler_point_clamp, uv2, 0).r;
    intensity = _WeatherMap.SampleLevel(sampler_point_clamp, uv2, 0).g;
    if (weatherType == 1.0) {
        color = rainyColor * intensity;
    }
    else if (weatherType == 2.0) {
        color = snowyColor * intensity;
    }
    else if (weatherType == 3.0) {
        color = cloudyColor * intensity;
    }
    else if (weatherType == 4.0) {
        color = sunnyColor * intensity;
    }
    else if (weatherType == 5.0) {
        color = partlyCloudyColor * intensity;
    }
    else {
        color = defaultColor;
    }

    //color = float3(uv2.x, uv2.y, 0.f);
}