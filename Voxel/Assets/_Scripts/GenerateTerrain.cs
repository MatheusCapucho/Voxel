using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class GenerateTerrain : MonoBehaviour
{
    public float surfaceLevel = 0.0F;
    public int maxTrianglesPerMesh = 20000;
    public Vector3 scale = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3Int resolution = new Vector3Int(32, 32, 32);
    public Material material;
    public VolumeBuffer voxels;
    public GameObject snowPrefab;
    public GameObject treePrefab;
    public int seed = 0;
    public float smoothAmount = 1.0f;
    public float minTreeDistance = 10.0f;
    public float treeThreshold = -0.5f;
    public float snowThreshold = 0.5f;
    private List<GameObject> meshes = new List<GameObject>();
    private List<Vector3> treePositions = new List<Vector3>();
    void Start()
    {
        // Create a volume buffer. The size is resolution + 2 because the outer edges of the
        // volume buffer will always be empty to prevent holes.
        voxels = new VolumeBuffer(new Vector3Int(resolution.x + 2, resolution.y + 2, resolution.z + 2));
        //AddTerrain();
        //AddTrees();
        //SmoothTerrain();
        //AddSnow();
        //Generate();
    }

    /*
    private void AddTerrain()
    {
        PerlinNoise perlin = new PerlinNoise(seed, 1.0f, 1.0f);
        INoise voronoi = new VoronoiNoise(seed, 1.0f, 1.0f);
        FractalNoise fractal = new FractalNoise(perlin, 4, 3.0f, 0.5f);
        FractalNoise fractal2 = new FractalNoise(voronoi, 3, 2.0f, 0.25f);
        // Calculate the position of each voxel in world space
        float voxelSize = 1f / (resolution.x - 1);
        Vector3 voxelOffset = new Vector3(0.5f, 0.5f, 0.5f) * voxelSize;
        for (int x = 1; x < resolution.x + 1; x++)
        {
            for (int y = 1; y < resolution.y + 1; y++)
            {
                for (int z = 1; z < resolution.z + 1; z++)
                {
                    Vector3 pos = new Vector3(x, y, z) * voxelSize - voxelOffset;
                    // Use the noise functions to generate terrain features
                    float height = fractal.Sample3D(pos.x, pos.y, pos.z);
                    height += fractal2.Sample3D(pos.x, pos.y, pos.z) * 0.5f;
                    float density = pos.y - height;
                    if (density >= 0)
                    {
                        voxels[x, y, z] = 0;
                    }
                    else
                    {
                        voxels[x, y, z] = 1;
                    }
                }
            }
        }
        // Fill the bottom of the volume buffer with solid voxels
        for (int x = 1; x < resolution.x + 1; x++)
        {
            for (int z = 1; z < resolution.z + 1; z++)
            {
                voxels[x, 0, z] = 1;
            }
        }
    }

    // Generate updates the mesh and deletes any old meshes.
    public void Generate()
    {
        MarchingTetrahedra myMarchingTetrahedra = new
        MarchingTetrahedra(surfaceLevel, voxels.Voxels);
        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();
        List<Vector3> normals = new List<Vector3>();
        myMarchingTetrahedra.Generate(vertices, indices);
        // Line up mesh to be in the center of its parent's pivot point.
        var position = new Vector3(-resolution.x / 2, -resolution.y / 2, -resolution.z / 2);
        GenerateMesh(vertices, indices, normals);
    }

    public void GenerateMesh(List<Vector3> verts, List<int> indices, List<Vector3> normals)
    {
        int maxVertsPerMesh = maxTrianglesPerMesh * 3;
        int numMeshes = verts.Count / maxVertsPerMesh + 1;
        for (int i = 0; i < numMeshes; i++)
        {
            List<Vector3> currentVerts = new List<Vector3>();
            List<Vector3> currentNormals = new List<Vector3>();
            List<int> currentIndices = new List<int>();
            for (int j = 0; j < maxVertsPerMesh; j++)
            {
                int idx = i * maxVertsPerMesh + j;
                if (idx < verts.Count)
                {
                    currentVerts.Add(verts[idx]);
                    currentIndices.Add(j);
                    if (normals.Count != 0)
                    {
                        currentNormals.Add(normals[idx]);
                    }
                }
            }
            if (currentVerts.Count == 0)
            {
                continue;
            }
            Mesh mesh = new Mesh();
            // Set the mesh normals
            if (currentNormals.Count <= 0)
            {
                mesh.RecalculateNormals();
            }
            else
            {
                mesh.SetNormals(currentNormals);
            }

            // Set the mesh vertices
            mesh.SetVertices(currentVerts);
            Vector3[] vertices = mesh.vertices;

            // assign the array of colors to the Mesh.
            //mesh.colors = this.SetVertexColors(vertices);

            // Set the mesh triangles using the meshIndices list
            mesh.SetTriangles(currentIndices, 0);

            // Recalculate the normals of the mesh
            mesh.RecalculateNormals();

            // Recalculate the bounds of the mesh
            mesh.RecalculateBounds();
            GameObject myMesh = new GameObject("Mesh")
            {
                tag = "Mesh"
            };
            // Set the parent of the new GameObject to be the current transform
            myMesh.transform.parent = transform;
            myMesh.AddComponent<MeshFilter>();
            myMesh.AddComponent<MeshRenderer>();
            myMesh.GetComponent<Renderer>().material = material;
            // Set the MeshFilter component to use the generated mesh"
            myMesh.GetComponent<MeshFilter>().mesh = mesh;
            // Apply the scale of the object
            myMesh.transform.localScale = scale;
            myMesh.AddComponent<MeshCollider>();
            meshes.Add(myMesh);
        }
    }

    */


}
