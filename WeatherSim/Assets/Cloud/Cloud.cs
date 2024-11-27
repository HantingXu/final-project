using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    [SerializeField] private BoxCollider _col;
    [SerializeField] private Material _cloudMaterial;
    [SerializeField] private Light _light;

    private Vector3 _boundsMin;
    private Vector3 _boundsMax;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        _boundsMax = _col.bounds.max;
        _boundsMin = _col.bounds.min;
        _cloudMaterial.SetVector("_BoundsMin", _boundsMin);
        _cloudMaterial.SetVector("_BoundsMax", _boundsMax);
        _cloudMaterial.SetVector("_SunDirection", _light.transform.forward);

        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;
        _cloudMaterial.SetFloat("_Timer", timer);
    }
}
