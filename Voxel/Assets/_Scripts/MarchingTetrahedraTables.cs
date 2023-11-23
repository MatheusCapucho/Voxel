using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarchingTetrahedraTables : MonoBehaviour
{
    // The VertexOffsetTable stores the distance from vertex0 for each
    // of the cube's 12 vertices. Example, the irst row contains {0,0,0}
    // meaning that vertex0 is 0 units away from vertex0 on the x, y, and z-axis.
    // likewise, the last row stores a vertex that is 0 units away on the x-axis,
    // 1 unit away on the y-axis, and 1 unit away on the z-axis.
    public static readonly int[,] VertexOffsetTable = new int[,]
    {
        {0, 0, 0},
        {1, 0, 0},
        {1, 1, 0},
        {0, 1, 0},
        {0, 0, 1},
        {1, 0, 1},
        {1, 1, 1},
        {0, 1, 1}
    };

    // The EdgeConnectionTable stores the index of each vertice that makes up an edge in the cube.
    // There are two vertices for each of the cube's 6 tetrahedrons.

    public static readonly int[,] EdgeConnectionTable = new int[,]
    {
        {0,1}, {1,2}, {2,0}, {0,3}, {1,3}, {2,3}
    };
    // The EdgeConnectionTable stores the index of each vertice that makes up an edge in the cube.
    // Each row represents a tetrahedron in a cube.
    public static readonly int[,] Indextable = new int[,]
    {
        {0,5,1,6},
        {0,1,2,6},
        {0,2,3,6},
        {0,3,7,6},
        {0,7,4,6},
        {0,4,5,6}
    };


    // The EdgeIntersectTable stores the edge intersections for each of the 16 cases.
    // Conversion of the EdgeInstersectTable from hex to binary for reference.
    // Each entry contains a list of 12 bits that correspond to the 12 edges of the cube.
    // Edges are marked as intersecting (1) or not intersecting(0).
    public static readonly int[] EdgeIntersectTable = new int[]
    {
        0x00, 0x0d, 0x13, 0x1e, 0x26, 0x2b, 0x35, 0x38,
        0x38, 0x35, 0x2b, 0x26, 0x1e, 0x13, 0x0d, 0x00
    };

    // The triangleTable stores the triangle list for each case,
    // one for each row. Each column corresponds to a vertex in the cube with the exception of the inal column.
    // The inal column is always -1 to indicate that there are no more vertices to extract.
    // -1 is an "invalid" value and indicates that the cube has inished extracting vertices.

    public static readonly int[,] triangleTable = new int[,]
    {
        {-1, -1, -1, -1, -1, -1, -1},
        { 0, 3, 2, -1, -1, -1, -1},
        { 0, 1, 4, -1, -1, -1, -1},
        { 1, 4, 2, 2, 4, 3, -1},
        { 1, 2, 5, -1, -1, -1, -1},
        { 0, 3, 5, 0, 5, 1, -1},
        { 0, 2, 5, 0, 5, 4, -1},
        { 5, 4, 3, -1, -1, -1, -1},
        { 3, 4, 5, -1, -1, -1, -1},
        { 4, 5, 0, 5, 2, 0, -1},
        { 1, 5, 0, 5, 3, 0, -1},
        { 5, 2, 1, -1, -1, -1, -1},
        { 3, 4, 2, 2, 4, 1, -1},
        { 4, 1, 0, -1, -1, -1, -1},
        { 2, 3, 0, -1, -1, -1, -1},
        {-1, -1, -1, -1, -1, -1, -1}
    };
}
