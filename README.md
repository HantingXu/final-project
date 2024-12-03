## Procedural Weather System
### Created by: Christine Kneer & Hanting Xu

### Contents
- [Design Doc](https://github.com/HantingXu/final-project?tab=readme-ov-file#design-doc)
- [Milestone 1](https://github.com/HantingXu/final-project?tab=readme-ov-file#milestone-1)
- [Milestone 2](https://github.com/HantingXu/final-project?tab=readme-ov-file#milestone-2)
- [Final](https://github.com/HantingXu/final-project?tab=readme-ov-file#final)

## Design Doc

#### Introduction
The project stems from a fascination with procedural weather systems discovered during a past internship. Procedurally generated weather responds to factors like terrain height, temperature, and humidity, which offers the exciting potential to create realistic, responsive environments that enhance immersion in virtual worlds, whether in games, simulations, or educational tools. By building a system that can handle weather generation at both macro and micro levels, we aim to provide an experience that not only enhances the sense of realism but also invites users to engage with the environment through exploration and experimentation.

#### Goal
The final outcome of our project is to produce a fully procedural-generated and self-evolving weather system based on some user inputs, like height maps or other distribution maps. Also, the project should provide the users with some additional tunable parameters to change the generated environment and allow user to zoom in/out to view the simulated result on both micro and marco levels.

#### Inspiration/reference:

- Geographical Adventures
  
|![image](https://github.com/user-attachments/assets/ad62093c-f6d9-4aac-b881-e506f6c42c4e)|
|:--:|
|*[Geographical Adventures by Sebastian Lagu](https://github.com/SebLague/Geographical-Adventures)*|

This little geography game resembles what we hope to achieve for our macro scopic simulation, both visually and logically. We hope to draw inspiration from how Sebastian used real world geography data to simulate weather.

- Procedural Weather Patterns
  
|![image](https://github.com/user-attachments/assets/fd084460-9d45-4817-9418-723da2cb43c0)|
|:--:|
|*[Procedural Weather Patterns by Nick](https://nickmcd.me/2018/07/10/procedural-weather-patterns/)*|

Our project was inspired by Nick's blog post, where they discussed how to procedrually generate dynamic weather. Although our project will likely be at a bigger scope, we will definitely refer to their discussion on procedurally generating weather maps (like precipitation map, wind map etc.).

#### Specification:
- Our project should be able to generate terrain and other required maps either automatically by some random seed or by reading some user provided maps.
- The system should provide some intuitive parameters for users to adjust the weather as they like.
- The modeled environment should be presented in a certain levels of details that allow user to zoom in or out for viewing.

#### Techniques:
- Noise functions will be involved in process of terrain, other related maps and cloud/rain generation.
- For the map generation part, we think this [article](https://medium.com/@henchman/adventures-in-procedural-terrain-generation-part-1-b64c29e2367a) and [this repository](https://github.com/weigert/proceduralweather) might be useful.
- For the weather transition and how different variable might act on one another, we might refer to [this paper](http://www.ignaciogarciadorado.com/p/2017_TOG/2017_TOG.pdf) as well.
- For the rendering part, we might refer to the [GDC talk](https://www.youtube.com/watch?v=mGHCOOnI5aE).

#### Design:
![](Images/flowchart.png)

#### Timeline:
- The focus of Milestone 1 would be mainly about generating terrain and other required map for weather.
- The focus of Milestone 2 would be mainly about simualting the weather and environment modeling.
- The focus of Milestone 3 would be mainly about rendering the weather and creating the special effects for the weather.

|*Timeline*|*Tasks for Christine*|*Tasks for Hanting*|
|:--:|:--:|:--:|
|Week 1 (2024/11/06 to 2024/11/13)|Procedural terrain generation|Water Map generation|
||Height map generation|Cloud map generation|
|||Temperature map generation|
|Week 2 (2024/11/13 to 2024/11/20)|Weather Map|Cloud Rendering|
|Week 3 (2024/11/20 to 2024/11/27)|Microscopic view foundation|Volumetric Cloud & Player|
|Week 4 (2024/11/27 to 2024/12/02)|Weather shaders|Skybox shader|
||Weather manager & weather transition|Water shader|


## Milestone 1

In Milestone 1, we laid the groundwork for our weather simulation project by generating both terrain and weather maps. These foundational maps allowed us to create a basic macroscopic view of our planet, incorporating a high level of user control for customization. 

|![demo](https://github.com/user-attachments/assets/bdedd8ad-642b-437b-abf7-478538965745)|
|:--:|
|*Milestone 1 Demo*|

### Completed Tasks

#### Height Map & Procedural Terrain Generation (Christine)

- **Planet Shape**

I used a cube with six faces, which is dynamically displaced into a unit sphere. The terrain is triangulated based on user input, allowing control over both tessellation levels and radius adjustments via the editor. The basic sphere is then further displaced by sampling the height map generated in the following step.

|![lod](https://github.com/user-attachments/assets/7e84d8b7-460e-4d9f-8cac-bdfa8430f4ad)|![radius](https://github.com/user-attachments/assets/a4fad612-7499-4e39-8a7d-f9835c42307f)|![control](https://github.com/user-attachments/assets/df9728d7-d0af-4bd9-b033-e76c1755d5ff)|
|:--:|:--:|:--:|
|*LOD*|*Radius*|*User Conrtol*|

- **Procedural Terrain Generation & Height Map**

Users can add custom noise layers, with each layer introducing additional surface variation to the planet. Using Perlin noise, users have control over parameters such as number of layers, frequency, base roughness, roughness, minimum value, and strength. 

|![noise](https://github.com/user-attachments/assets/5ef0fdcf-7991-40c3-9a9c-1c4aab5d9a05)|![heightMap](https://github.com/user-attachments/assets/f648395d-0bb7-4467-9ec6-ca0bf516fa07)|![heightMapControl](https://github.com/user-attachments/assets/9c06203c-3e29-4c38-9cbb-51ccca3978f1)|
|:--:|:--:|:--:|
|*Noise*|*Height Map*|*User Control*|

The first noise layer can act as a mask to define continent outlines, for example. Subsequent layers, like a second noise layer, can add high-strength noise to simulate mountainous regions. Using the first layer as a mask allows us to ensure that mountains only appear on continents rather than oceans, as demonstrated below.

|![1](https://github.com/user-attachments/assets/9f36193b-8fb3-4256-8e73-ba44df8e8c86)|![2](https://github.com/user-attachments/assets/918eee37-eeb0-40fa-94c2-f51e2eeaa3aa)|![3](https://github.com/user-attachments/assets/bc106a44-2e8d-449a-a1c4-06e87c9d3199)|
|:--:|:--:|:--:|
|*First Layer*|*First + Second Layer*|*First + Second Layer (using first layer as mask)*|

#### Other Weather Maps (Hanting)

- **Water Map**

Water map is generated based on the the height map. The user can adjust the percentage of water coverage of planet based on height with Water Ratio slider.

|![waterMap](https://github.com/user-attachments/assets/30a51c81-0ff4-453f-906b-bbd6f34eca59)|![control](https://github.com/user-attachments/assets/cf21078b-96aa-4f8d-a87b-17c0a6461bf0)|
|:--:|:--:|
|*Water Map*|*User Control*|

- **Cloud Map**

Cloud map is purely randomly generated by the perlin noise in the initialization stage. The user can change the number of the storm centers they want to have by sliding the Center Number slider. To have the swirling clouds on the atmosphere, I use the curl noise for cloud flow decribed in this [article](https://www.cs.ubc.ca/~rbridson/docs/bridson-siggraph2007-curlnoise.pdf). 

|![cloudMap](https://github.com/user-attachments/assets/ff320458-37df-440b-98ae-b0d226d1fa4d)|![control](https://github.com/user-attachments/assets/3d78e831-a7c5-48fb-a38f-7ce9053efaf5)|
|:--:|:--:|
|*Cloud Map*|*User Control*|

- **Temperature Map**

Tempurature map is generated based the latitude and height of the globe. In this Setting, the temperature tends to be lower at both poles and places of high altitude, and gets warmer as becomes closer to the equator. People can slide the Latitude Weight and Height Weight to adjust the influence of these two factors acting on temperature.

|![temperaturMap](https://github.com/user-attachments/assets/07385fea-e808-4bca-93c6-e745dd2385b5)|![control](https://github.com/user-attachments/assets/76e990d1-c791-4ccd-80fb-0256055ec563)|
|:--:|:--:|
|*Temperature Map*|*User Control*|

#### References
- [*Sebastian Lague's Procedural Planet Series*](https://www.youtube.com/watch?v=QN39W020LqU&list=PLFt_AvWsXl0cONs3T0By4puYy6GM22ko8)
- [*Curl Noise*](https://www.cs.ubc.ca/~rbridson/docs/bridson-siggraph2007-curlnoise.pdf)

## Milestone 2

### Completed Tasks

#### Biome Map Generation & Microscopic Cloud Generation (Hanting)

- **Biome Map Generation**

Biome map is generated based on two maps temperature map and humidity map. User can provide a texture map that assigns different colors that represents different biomes. The provided texture should take temperature in y-axis and humidity in x-axis, like the picture shown below.

|![biomeLookup](https://github.com/user-attachments/assets/12f9d36d-89f4-4083-8a85-4a088ad3bae4)|![biomePlanet](https://github.com/user-attachments/assets/f44623f8-050d-4ad7-9570-1fabf72836ef)|
|:--:|:--:|
|*Biome Map*|*Planet textured by Biome Map*|

- **Microscopic Cloud Generation**
  
Our volumetric clouds are generated through sampling the density of a provided 3D noise texture by raymarching, so that in the final version, the player can move around or in the cloud. To do that, this [video](https://www.youtube.com/watch?v=4QOcCGI6xOU) provide great help to me. Currently, the clouds are only rendered in a fixed box and didn't sample the cloud map distribution. I'll continue to work on that in the final version.

|![](Images/cloud.png)|
|:--:|
|*Cloud*|

#### Procedural Weather & Weather Visualization (Christine)

- **Procedural Weather**
  
I leverage our maps including temperature, water, and cloud maps to procedurally generate realistic and dynamic weather conditions. 
Each map is associated with an environmental factor that influence the weather:

|**Map**|**Environmental Factor**|
|:--:|:--:|
|Temperature Map|Temperature|
|Water Map|Water Proximity|
|Cloud Map|Cloud Density|

By integrating environmental factors, I simulate the following weather phenomena based on user-defined thresholds:

|**Weather Type**|**Relevant Environmental Factor**|
|:--:|:--:|
|Sunny|*temperature > hotThreshold && cloudDensity < lowThreshold*|
|Rainy|*temperature > coldThreshold && cloudDensity > highThreshold && waterProximity > nearThreshold*|
|Cloudy|*cloudDensity > mediumThreshold*|
|Snowy|*temperature < coldThreshold && cloudDensity > highThreshold && waterProximity > nearThreshold*|
|Partly Cloudy|*Other*|

Additionally, each weather type is assigned an intensity based on the current environmental factors and some smoothing factor.

|![](https://github.com/user-attachments/assets/d0c961c8-71f1-4a12-bf92-02bfd23db284)|![](https://github.com/user-attachments/assets/30db2f56-d7ae-4291-837e-f205bbf03143)|
|:--:|:--:|
|*Weather Map (R Channel: Weather Type, G Channel: Intensity)*|*User Control*|

- **Weather Visualization**

I also created a shader to showcase our dyanmic weather system, where each color represents a unique weather type.

|![weather](https://github.com/user-attachments/assets/25116134-6a83-4b9f-8ce2-0cdbe0fc371f)|![](https://github.com/user-attachments/assets/31961ead-4151-4f6e-a318-e9b1b057c528)|
|:--:|:--:|
|*Weather Map Visualization*|*User Control*|

#### References
- [*Sebastian Lague's Coding Adventure: Clouds*](https://www.youtube.com/watch?v=4QOcCGI6xOU)

## Final
### Completed Tasks
#### Plane Controller & Volumetric Cloud & Stylized Water & Procedural Skybox (Hanting)
- **Plane Controller**

To travel on the micropic scale, I made a plane controller that can yaw, pitch, roll and throttle by simple controls.

|![](Images/planeController.png)|
|:--:|
|*Plane Control*|

- **Volumetric Cloud**

By getting the light transmittance from ray marching and sampling the cloud density from the cloud map and 3D noise texture, I render the cloud in some cartoon style. In this way, the player can freely traveling in or out of the clouds as they want. Also, they can change the density, color, light absorbtion of the clouds in the shader as they want.

|![cloud](Images/cloudFinal.png)|![](Images/cloudControl.png)|
|:--:|:--:|
|*Cloud Visualization*|*User Control*|

- **Stylized Water**

The stylized water is made with simple foam near the shore and waves by sampling and mixing wave normal texture. Players can adjust the water color as they want.

|![](Images/water.png)|
|:--:|
|*Procedural Sea*|

- **Procedural Skybox**

The procedural skybox shader takes in 4 color - day sky color, night sky color, day land color, night land color. Computing the dot product between the up vector and main light direction, I lerp between these colors to get the current color and draw the sun with the similar method. By rotating the directional light, the player can see the day and night changes.

|![skybox](Images/skyBox.png)|![](Images/SkyboxControl.png)|
|:--:|:--:|
|*Skybox Visualization*|*User Control*|


#### Microscopic view & Weather Shaders & Weather Transition (Christine)

- **Microscopic view**
  
The Microscopic View provides a detailed representation of weather effects on a smaller scale. Unlike the macroscopic view, which uses a sphere to encapsulate the environment, the microscopic view employs a dynamically deformed plane. I utilized cosine-weighted UV mapping to minimize distortions, especially near the poles, ensuring accurate texture sampling.

|![](https://github.com/user-attachments/assets/fd7f3493-d49c-471a-8fb5-72eecd30b2bf)|
|:--:|
|*Microscopic View*|

- **Weather Shaders**
  
  Our system incorporates specialized post-process shaders to render various weather effects.
  
  - **Rain Shader**

The Rain Shader simulates realistic rain by rendering raindrops with varying intensities and speeds. It dynamically adjusts the number of raindrop layers based on the current intensity, ensuring a smooth visual experience.

|![lightrain](https://github.com/user-attachments/assets/7aeed60d-fdf7-4635-94d5-084e234ccc0f)|![heavy](https://github.com/user-attachments/assets/9cede870-7c54-47a6-b0e0-2e27009289c1)|
|:--:|:--:|
|*Light Rain*|*Heavy Rain*|

  - **Snow Shader**

The Snow Shader replicates the serene effect of snow by rendering snowflakes with varying intensities and movements. It dynamically adjusts the number of snowflake layers based on the current intensity, ensuring a smooth and realistic snowfall.

|![lightsnow](https://github.com/user-attachments/assets/504a42f9-fbc1-4e49-9f22-cc85880cb924)|![heavy](https://github.com/user-attachments/assets/b2f96175-1956-4a08-a0c3-04b46385d99c)|
|:--:|:--:|
|*Light Snow*|*Heavy Snow*|

- **Weather Transition**
  
The Weather Transition component manages the smooth transition between different weather states. It maintains the current weather type and intensity, dynamically reads target weather conditions from a weather map using compute shaders, and interpolates between the current and target states to ensure seamless visual transitions. The current weather type and intensity is passed into the weather shaders and the skybox for rendering.

https://github.com/user-attachments/assets/1f362951-220c-4253-a6d8-addd978ed6ac

https://github.com/user-attachments/assets/a54bbe0a-1da9-4395-9539-ebdc514e5b9e

https://github.com/user-attachments/assets/2b98ba5b-1197-429b-ab4e-fe574b653743


#### References
- [*Snow Shader*](https://www.shadertoy.com/view/ldsGDn)

### Final Results

https://github.com/user-attachments/assets/588698cf-8f5f-458a-aaef-0bbef75492ac

https://github.com/user-attachments/assets/4b46032d-2c51-4b5e-a67e-8a5246e3a6ad

### Post Mortem

We successfully developed a dynamic weather system that can be seamlessly integrated into any game, providing users with extensive control over various weather parameters. We also delivered a working project that effectively demonstrates the system's capabilities and smooth weather transitions.

However, the demonstration scene is somewhat simplistic, featuring terrain with low levels of detail and lacking procedurally placed assets, which limits the visual complexity and realism. Additionally, while we have the underlying logic for cloudy and sunny weather, we only implemented shaders for rain and snow. 

For future work, we aim to enhance the scene's visual richness and expand our shader repertoire to include all planned weather types.
