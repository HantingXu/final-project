#pragma once
SAMPLER(sampler_point_clamp);

void Snow(float intensity, float2 uv, out float3 col, out float alpha)
{
    const int MAX_LAYERS = 100; // Fixed maximum number of layers
    const int MIN_LAYERS = 5;   // Minimum layers for the effect
    float numLayers = lerp(float(MIN_LAYERS), float(MAX_LAYERS), intensity);
    float DEPTH = lerp(0.5, 0.25, intensity);
    float WIDTH = lerp(0.5, 0.5, intensity);
    float SPEED = lerp(1.5, 1.5, intensity);

    const float3x3 p = float3x3(
        13.323122, 21.1212, 21.8112,
        23.5112, 28.7312, 14.7212,
        21.71123, 11.9312, 61.3934
    );

    float3 acc = float3(0.0, 0.0, 0.0);
    float dof = 5.0 * sin(_Time.y * 0.1);
    for (int i = 0; i < MAX_LAYERS; i++)
    {
        float fi = float(i);

        float layerWeight = saturate(numLayers - fi);
        if (layerWeight <= 0.0) {
            break;
        }

        float layerDepth = DEPTH;
        float layerWidth = WIDTH;

        float2 q = uv * (1.0 + fi * layerDepth);
        q += float2(
            q.y * (layerWidth * fmod(fi * 7.238917, 1.0) - layerWidth * 0.5),
            SPEED * _Time.y / (1.0 + fi * layerDepth * 0.03)
        );

        float3 n = float3(floor(q), 31.189 + fi);
        float3 m = floor(n) * 0.00001 + frac(n);
        float3 mp = (31415.9 + m) / frac(mul(p, m));
        float3 r = frac(mp);

        float2 s = abs(fmod(q, 1.0) - 0.5 + 0.9 * r.xy - 0.45);
        s += 0.01 * abs(2.0 * frac(10.0 * q.yx) - 1.0);

        float d = 0.6 * max(s.x - s.y, s.x + s.y) + max(s.x, s.y) - 0.01;

        float edge = 0.005 + 0.05 * min(0.5 * abs(fi - 5.0 - dof), 1.0);

        float t = saturate((d - edge) / (-2.0 * edge));
        acc += t * (r.x / (1.0 + 0.02 * fi * layerDepth)) * layerWeight;
    }

    alpha = length(acc);
    col = float3(1.f, 1.f, 1.f);
}

void Rain(float intensity, float2 uv, out float3 col, out float alpha)
{
    intensity = clamp(intensity, 0.f, 0.65f);
    const int MAX_LAYERS = 60;
    const int MIN_LAYERS = 5;
    float numLayers = lerp(float(MIN_LAYERS), float(MAX_LAYERS), intensity);

    float DEPTH = lerp(0.5, 0.1, intensity);
    float SPEED = lerp(4.0, 4.0, intensity);

    // Raindrop properties
    float dropLength = lerp(0.1, 0.3, intensity);
    float dropWidth = 0.005;

    const float3x3 p = float3x3(
        13.323122, 21.1212, 21.8112,
        23.5112, 28.7312, 14.7212,
        21.71123, 11.9312, 61.3934
    );

    float3 acc = float3(0.0, 0.0, 0.0);
    float dof = 5.0 * sin(_Time.y * 0.1);

    for (int i = 0; i < MAX_LAYERS; i++)
    {
        float fi = float(i);

        float layerWeight = saturate(numLayers - fi);
        if (layerWeight <= 0.0) {
            break;
        }

        float layerDepth = DEPTH;

        float2 q = uv * (1.0 + fi * layerDepth);

        // Vertical movement for rain
        q += float2(
            0.0,
            SPEED * _Time.y / (1.0 + fi * layerDepth * 0.03)
        );


        float3 n = float3(floor(q), 31.189 + fi);
        float3 m = floor(n) * 0.00001 + frac(n);
        float3 mp = (31415.9 + m) / frac(mul(p, m));
        float3 r = frac(mp);


        float fracY = fmod(q.y + r.y, 1.0);

        // Compute the raindrop shape
        float xOffset = fmod(q.x + r.x, 1.0) - 0.5;

        float d = abs(xOffset) / dropWidth + fracY / dropLength;
        d -= 1.0;

        float edge = 0.005 + 0.05 * min(0.5 * abs(fi - 5.0 - dof), 1.0);

        float t = saturate((edge - d) / edge);

        acc += t * layerWeight;
    }

    alpha = saturate(length(acc) * 0.5 * intensity);
    col = float3(0.6, 0.7, 1.0);
}


void ApplyWeatherEffect_float(float weatherType, float weatherIntensity, float2 UV, out float3 col, out float alpha)
{
    if (weatherType == 1.f) {
        Rain(weatherIntensity, UV, col, alpha);
    }
    else if (weatherType == 2.f) {
        Snow(weatherIntensity, UV, col, alpha);
    }
    else {
        col = float3(0.0f, 0.0f, 0.0f);
        alpha = 0.f;
    }
}