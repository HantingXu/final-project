using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MapSetup : MonoBehaviour
{
    [Header("References")]
    public MapGeneratorStats parameter;

    // Start is called before the first frame update
    void Start()
    {
        HeightMapGenerator heightMapGenerator = new HeightMapGenerator(this);
        heightMapGenerator.GenerateMap();
        WaterMapGenerator waterMapGenerator = new WaterMapGenerator(this);
        waterMapGenerator.GenerateMap();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
