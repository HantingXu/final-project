#pragma once

float2 RaySphereDst(float3 sphereCenter, float sphereRadius, float3 pos, float3 rayDir)
{
    float3 oc = pos - sphereCenter;
    float b = dot(rayDir, oc);
    float c = dot(oc, oc) - sphereRadius * sphereRadius;
    float t = b * b - c;//t > 0有两个交点, = 0 相切， < 0 不相交
    
    float delta = sqrt(max(t, 0));
    float dstToSphere = max(-b - delta, 0);
    float dstInSphere = max(-b + delta - dstToSphere, 0);
    return float2(dstToSphere, dstInSphere);
}

 // Returns (dstToBox, dstInsideBox). If ray misses box, dstInsideBox will be zero
 float2 RayBoxDst(float3 boundsMin, float3 boundsMax, float3 rayOrigin, float3 invRaydir) 
 {
    float3 t0 = (boundsMin - rayOrigin) * invRaydir;
    float3 t1 = (boundsMax - rayOrigin) * invRaydir;
    float3 tmin = min(t0, t1);
    float3 tmax = max(t0, t1);
    
    float dstA = max(max(tmin.x, tmin.y), tmin.z);
    float dstB = min(tmax.x, min(tmax.y, tmax.z));

    float dstToBox = max(0, dstA);
    float dstInsideBox = max(0, dstB - dstToBox);
    return float2(dstToBox, dstInsideBox);
}

float Remap(float originalVal, float originalMin, float originalMax, float newMin, float newMax)
{
    return newMin + ((originalVal - originalMin) / (originalMax - originalMin)) * (newMax - newMin);
}

float sampleDensity(float3 rayPos) 
{
    /*
    float3 shapeSamplePos = rayPos * 0.5f * 0.001; //+ shapeOffset * offsetSpeed;
    float4 shapeNoise = _NoiseTex.SampleLevel(SamplerState_Trilinear_Repeat, shapeSamplePos, 0);
    float density = max(0, shapeNoise.r - 0.3f) *  1.0f * 0.005f;*/

    float3 shapeSamplePos = rayPos * 0.5f * 0.001 + float3(_Timer * 0.1f, _Timer * 0.2f, _Timer * 0.3f) * _MoveSpeed; //+ shapeOffset * offsetSpeed;
    float4 shapeNoise = _NoiseTex.SampleLevel(SamplerState_Trilinear_Repeat, shapeSamplePos, 0)  - _BaseThreshold;
    float4 detailNoise = _DetailTex.SampleLevel(SamplerState_Trilinear_Repeat, shapeSamplePos, 0) - _DetailThreshold;
    float baseDensity = max(0, shapeNoise.r - _BaseThreshold) *  0.01f;
    float baseFBM = dot(shapeNoise.gba, float3(0.56, 0.25, 0.125));
    baseDensity = Remap(shapeNoise.r, saturate((1.0f - baseFBM) * 0.44f), 1.0, 0, 1.0);

    float detailDensity = max(0, detailNoise.r - _DetailThreshold) *  0.01f;
    float detailFBM = dot(detailNoise.gba, float3(0.5, 0.25, 0.125));
    detailDensity = Remap(detailNoise.r, saturate((1.0f - detailFBM) * 0.44f), 1.0, 0, 1.0);
    return baseDensity * (1.0f - _DetailRatio) + detailDensity * _DetailRatio;//step(baseDensity, 0.01) * detailDensity;
    //return 0.01;//density;
}

void ComputeRayDirection_float(float3 cameraPos, float3 targetPos, out float3 rayDirection)
{
    rayDirection = normalize(targetPos - cameraPos);
}

void Test_float(float3 cameraPos, float3 viewDirection, float3 boundMin, float3 boundMax, float depth, out float4 col)
{
    float3 rayOrigin = cameraPos;
    float3 rayDirection = viewDirection;

    float2 rayToContainerInfo = RayBoxDst(boundMin, boundMax, rayOrigin, 1/rayDirection);
    float dstToBox = rayToContainerInfo.x;
    float dstInsideBox = rayToContainerInfo.y;

    // point of intersection with the cloud container
    //float3 entryPoint = rayPos + rayDir * dstToBox;
    if(dstInsideBox == 0.0f || depth < dstToBox)
        col = float4(0,0,0,0);
    else
        col = float4(1,0.4,0.4,1);
}

float ComputeCloudMapDensity(float2 xzPosition, float3 boundMin, float3 boundMax)
{
    float2 xzTemp = xzPosition - boundMin.xz;
    float2 boundTotal = boundMax.xz - boundMin.xz;
    float2 uv = xzTemp / boundTotal; //_WeatherMap.SampleLevel(SamplerState_Linear_Repeat)
    return SAMPLE_TEXTURE2D_LOD(_WeatherMap, SamplerState_Linear_Repeat, uv, 0.0f).r;
    //return 0.0f;
}

float LightMarch(float3 boundMin, float3 boundMax, float3 position) {
    float3 dirToLight = normalize(_SunDirection);
    float dstInsideBox = RayBoxDst(boundMin, boundMax, position, 1/dirToLight).y;

    int numStepsLight = 30;
    float lightAbsorptionTowardSun = _LightAbsorption;
    float darknessThreshold = _Darkness;
    float stepSize = dstInsideBox/numStepsLight;

    float totalDensity = 0;

    for (int step = 0; step < numStepsLight; step ++) {
        position += dirToLight * stepSize;
        totalDensity += max(0, sampleDensity(position) * stepSize);
    }

    float transmittance = exp(-totalDensity * lightAbsorptionTowardSun);
    return darknessThreshold + transmittance * (1-darknessThreshold);
}

// Henyey-Greenstein
float hg(float a, float g) 
{
    float g2 = g*g;
    return (1-g2) / (4*3.1415*pow(1+g2-2*g*(a), 1.5));
}

float phase(float a) 
{
    float blend = .5;
    float4 phaseParams = float4(0.5, 0.7, 0.8, 0.4);
    float hgBlend = hg(a,phaseParams.x) * (1-blend) + hg(a,-phaseParams.y) * blend;
    return phaseParams.z + hgBlend*phaseParams.w;
}           

void DrawCloud_float(float3 cameraPos, float3 viewDirection, float3 boundMin, float3 boundMax, float depth, float2 uv, out float4 col)
{
    float3 rayOrigin = cameraPos;
    float3 rayDirection = viewDirection;

    float2 rayToContainerInfo = RayBoxDst(boundMin, boundMax, rayOrigin, 1/rayDirection);
    float dstToBox = rayToContainerInfo.x;
    float dstInsideBox = rayToContainerInfo.y;

    // Phase function makes clouds brighter around sun
    float cosAngle = dot(rayDirection, normalize(_SunDirection));
    float phaseVal = phase(cosAngle);

    // point of intersection with the cloud container
    float3 entryPoint = rayOrigin + rayDirection * dstToBox;

    float dstTravelled = SAMPLE_TEXTURE2D_LOD(_BlueNoise, SamplerState_Linear_Repeat, uv, 0.0f).r * _NoiseScale;
    float dstLimit = min(depth-dstToBox, dstInsideBox);

    const float stepSize = 11;

    // March through volume:
    float transmittance = 1;
    float3 lightEnergy = 0;
    float totDensity = 0.0f;
    while (dstTravelled < dstLimit) 
    {
        rayOrigin = entryPoint + rayDirection * dstTravelled;
        float density = sampleDensity(rayOrigin);
        
        if (density > 0) {
            totDensity += density;
            float lightTransmittance = LightMarch(boundMin, boundMax, rayOrigin);
            lightEnergy += density * stepSize * transmittance * ComputeCloudMapDensity(rayOrigin.xz, boundMin, boundMax) * lightTransmittance * phaseVal;
            transmittance *= exp(-density * stepSize);
        
            // Exit early if T is close to zero as further samples won't affect the result much
            if (transmittance < 0.01) {
                break;
            }
        }
        dstTravelled += stepSize;
    }
    float3 cloudCol = lightEnergy * _CloudBaseColor + (1 - lightEnergy) * _CloudShadowColor;// + (1.0f - lightEnergy) * float3(1.0f, 1.0f, 0.0f);
    col = float4(cloudCol, lightEnergy.r);
     //           return float4(col,0);
}