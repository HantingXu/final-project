using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetPlane : MonoBehaviour
{
    public int widthSegments = 20;
    public int lengthSegments = 10;
    public float size = 100;
    public Material planeMaterial;

    [SerializeField, HideInInspector]
    MeshFilter meshFilter;
    Mesh mesh;

    public void OnValidate()
    {
        Initialize();
        GeneratePlane();
    }

    void Initialize()
    {
        if (meshFilter == null)
        {
            meshFilter = new MeshFilter();
            GameObject meshObject = new GameObject("ProceduralPlane");

            meshObject.transform.parent = this.transform;
            meshFilter = meshObject.AddComponent<MeshFilter>();
            meshObject.AddComponent<MeshRenderer>().sharedMaterial = planeMaterial;

            meshFilter.sharedMesh = new Mesh();
            mesh = meshFilter.sharedMesh;
        }
        mesh = meshFilter.sharedMesh;
    }

    public void GeneratePlane()
    {
        int vertexCount = (widthSegments + 1) * (lengthSegments + 1);
        Vector3[] vertices = new Vector3[vertexCount];
        Vector2[] uv = new Vector2[vertexCount];
        int[] triangles = new int[widthSegments * lengthSegments * 6];

        float halfSize = size * 0.5f;
        float segmentWidth = (size * 2.0f) / widthSegments;
        float segmentLength = size / lengthSegments;

        int vertIndex = 0;
        int triIndex = 0;

        for (int y = 0; y <= lengthSegments; y++)
        {
            for (int x = 0; x <= widthSegments; x++)
            {
                float xPos = -size + x * segmentWidth;
                float zPos = -halfSize + y * segmentLength;
                vertices[vertIndex] = new Vector3(xPos, 0, zPos);
                uv[vertIndex] = new Vector2((float)x / widthSegments, (float)y / lengthSegments);

                if (x < widthSegments && y < lengthSegments)
                {
                    triangles[triIndex + 0] = vertIndex;
                    triangles[triIndex + 1] = vertIndex + widthSegments + 1;
                    triangles[triIndex + 2] = vertIndex + 1;

                    triangles[triIndex + 3] = vertIndex + 1;
                    triangles[triIndex + 4] = vertIndex + widthSegments + 1;
                    triangles[triIndex + 5] = vertIndex + widthSegments + 2;

                    triIndex += 6;
                }
                vertIndex++;
            }
        }
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
    }
}
