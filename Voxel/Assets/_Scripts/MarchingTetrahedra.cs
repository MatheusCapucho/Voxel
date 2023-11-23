using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingTetrahedra
{
    public float Surface;
    public float[,,] Voxels;
    private float[] Cube;
    private float[] CubeIsovalues;
    private int[] WindingOrder;
    private Vector3[] EdgeVert;
    private Vector3[] CubePosition;
    private Vector3[] TetrahedronPosition;
    // Constructor for marching tetrahedra object
    public MarchingTetrahedra(float surface, float[,,] voxels)
    {
        EdgeVert = new Vector3[6];
        CubePosition = new Vector3[8];
        TetrahedronPosition = new Vector3[4];
        CubeIsovalues = new float[4];
        Surface = surface;
        Cube = new float[8];
        WindingOrder = new int[] { 2, 1, 0 };
        Voxels = voxels;
    }

    public virtual void Generate(IList<Vector3> verts, IList<int> indices)
    {
        Vector3 resolution = new Vector3(Voxels.GetLength(0), Voxels.GetLength(1),
        Voxels.GetLength(2));
        UpdateWindingOrder();
        int x, y, z, i;
        int ix, iy, iz;
        // Call March() to run the marching tetrahedra algorithm on each cube in the volume buffer.
        for (x = 0; x < resolution.x - 1; x++)
        {
            for (y = 0; y < resolution.y - 1; y++)
            {
                for (z = 0; z < resolution.z - 1; z++)
                {
                    // Get all values of the cube and store them in the Cube array, overwriting previous entries.
        for (i = 0; i < 8; i++)
                    {
                        ix = x + MarchingTetrahedraTables.VertexOffsetTable[i, 0];
                        iy = y + MarchingTetrahedraTables.VertexOffsetTable[i, 1];
                        iz = z + MarchingTetrahedraTables.VertexOffsetTable[i, 2];
                        Cube[i] = Voxels[ix, iy, iz];
                    }
                    // Run marching tetrahedra on the current cube.
                    MarchThroughCube(x, y, z, Cube, verts, indices);
                }
            }
        }
    }

    // Set winding order based on Isovalue level
    // to ensure faces are in the right direction.
    private void UpdateWindingOrder()
    {
        if (Surface < 0.0f)
        {
            WindingOrder[0] = 2;
            WindingOrder[1] = 1;
            WindingOrder[2] = 0;
        }
        else
        {
            WindingOrder[0] = 0;
            WindingOrder[1] = 1;
            WindingOrder[2] = 2;
        }
    }

    // GetEdgeIntersect inds the approximate point of intersection of the Isovalue
    // between two points with the values v1 and v2
    private float GetEdgeIntersect(float vertex1, float vertex2)
    {
        return (Surface - vertex1) / (vertex2 - vertex1);
    }

    // March through one cube. There are 6 tetrahedra per cube.
    private void MarchThroughCube(float x, float y, float z, float[] cube, IList<Vector3>vertList, IList<int> indexList)
    {
        int i, k, vertexInACube;
        for (i = 0; i < 8; i++)
        {
            CubePosition[i].x = x + MarchingTetrahedraTables.VertexOffsetTable[i, 0];
            CubePosition[i].y = y + MarchingTetrahedraTables.VertexOffsetTable[i, 1];
            CubePosition[i].z = z + MarchingTetrahedraTables.VertexOffsetTable[i, 2];
        }
        for (i = 0; i < 6; i++)
        {
            for (k = 0; k < 4; k++)
            {
                vertexInACube = MarchingTetrahedraTables.Indextable[i, k];
                TetrahedronPosition[k] = CubePosition[vertexInACube];
                CubeIsovalues[k] = cube[vertexInACube];
            }
            MarchThroughTetrahedron(vertList, indexList);
        }
    }

    // MarchThroughTetrahedron loops through a tetrahedron made up of four vertices and generates triangles to render a 3D Isovalue
    // The generated triangles are stored as indices in indexList and the vertices are store in vertList
    private void MarchThroughTetrahedron(IList<Vector3> vertList, IList<int> indexList)
    {
        int i, j, vert, vert0, vert1, idx;
        int lagIndex = 0, edgeFlags;
        float offset, inverseOffset;
        for (i = 0; i < 4; i++)
        {
            if (CubeIsovalues[i] <= Surface)
            {
                lagIndex |= 1 << i;
            }
        }
        // Determine which edges of the tetrahedron are intersected by the Isovalue using the lagIndex
        edgeFlags = MarchingTetrahedraTables.EdgeIntersectTable[lagIndex];
        // If none of the edges are intersected there are no triangles to generate so return early
        if (edgeFlags == 0)
        {
            return;
        }
        // Calculate the intersection points of the Isovalue with each intersected edge and store them in EdgeVert
        for (i = 0; i < 6; i++)
        {
            if ((edgeFlags & (1 << i)) != 0)
            {
                vert0 = MarchingTetrahedraTables.EdgeConnectionTable[i, 0];
                vert1 = MarchingTetrahedraTables.EdgeConnectionTable[i, 1];
                offset = GetEdgeIntersect(CubeIsovalues[vert0], CubeIsovalues[vert1]);
                inverseOffset = 1.0f - offset;
                EdgeVert[i].x = inverseOffset * TetrahedronPosition[vert0].x + offset *
                TetrahedronPosition[vert1].x;
                EdgeVert[i].y = inverseOffset * TetrahedronPosition[vert0].y + offset *
                TetrahedronPosition[vert1].y;
                EdgeVert[i].z = inverseOffset * TetrahedronPosition[vert0].z + offset *
                TetrahedronPosition[vert1].z;
            }
        }

        // Generate triangles using the intersection points and add them to indexList and vertList
        for (i = 0; i < 2; i++)
        {
            if (MarchingTetrahedraTables.triangleTable[lagIndex, 3 * i] < 0) break;
            idx = vertList.Count;
            for (j = 0; j < 3; j++)
            {
                vert = MarchingTetrahedraTables.triangleTable[lagIndex, 3 * i + j];
                indexList.Add(idx + WindingOrder[j]);
                vertList.Add(EdgeVert[vert]);
            }
        }
    }
}
