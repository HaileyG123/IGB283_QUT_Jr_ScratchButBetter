using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class which contains transformation functions
/// </summary>
public class IGB283Transform : MonoBehaviour
{
    /// <summary>
    /// determines the inverse of a matrix
    /// </summary>
    /// <param name="matrix">matrix that will be inverted</param>
    /// <returns>inverse of the matrix parameter</returns>
    public static Matrix3x3 GetInverse(Matrix3x3 matrix)
    {
        Matrix3x3 InverseMatrix = new Matrix3x3();

        InverseMatrix = matrix;

        Matrix3x3 inverseMatrixInverse = InverseMatrix.inverse;

        return inverseMatrixInverse;
    }
    
    /// <summary>
    /// Rotates a vertex around the origin
    /// </summary>
    /// <param name="angle">angle of rotation</param>
    /// <returns>a rotation matrix</returns>
    public static Matrix3x3 Rotate (float angle)
    {
        // Create a new matrix
        Matrix3x3 matrix = new Matrix3x3();

        // Set the rows of the matrix
        matrix.SetRow(0, new Vector3(Mathf.Cos(angle), -Mathf.Sin(angle), 0.0f));
        matrix.SetRow(1, new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0.0f));
        matrix.SetRow(2, new Vector3(0.0f, 0.0f, 1.0f));

        return matrix;
    }

    /// <summary>
    /// Translates a vertex by dx dy
    /// </summary>
    /// <param name="offset">a vector that contains (dx,dy,0.0f)</param>
    /// <returns>a translation matrix</returns>
    public static Matrix3x3 Translate(Vector3 offset)
    {
        //Create a new matrix
        Matrix3x3 matrix = new Matrix3x3();

        // Set rows of the matrix
        matrix.SetRow(0, new Vector3(1.0f, 0.0f, offset.x));
        matrix.SetRow(1, new Vector3(0.0f, 1.0f, offset.y));
        matrix.SetRow(2, new Vector3(0.0f, 0.0f, 1.0f));

        return matrix;
    }

    /// <summary>
    /// Scale a vertex by a factor of sx and sy
    /// </summary>
    /// <param name="sx">scale factor in the x direction</param>
    /// <param name="sy">scale factor in the y direction</param>
    /// <returns>a scaling matrix</returns>
    public static Matrix3x3 Scale(float sx, float sy)
    {
        //Create a new matrix
        Matrix3x3 matrix = new Matrix3x3();

        // Set rows of the matrix
        matrix.SetRow(0, new Vector3(sx, 0.0f, 0.0f));
        matrix.SetRow(1, new Vector3(0.0f, sy, 0.0f));
        matrix.SetRow(2, new Vector3(0.0f, 0.0f, 1.0f));

        return matrix;
    }
    
    /// <summary>
    /// calculates the magnitude of a vector
    /// </summary>
    /// <param name="V1">vector of interest</param>
    /// <returns>the magnitude of the vector parameter</returns>
    public static double GetMagnitude(Vector3 V1)
    {
        double magnitude = Math.Sqrt((V1.x * V1.x) + (V1.y * V1.y) + (V1.z * V1.z));

        return magnitude;
    }
    
    /// <summary>
    /// determines the vector between two points
    /// </summary>
    /// <param name="Q">a point</param>
    /// <param name="P">another point</param>
    /// <returns>the vector that goes from point P to Q</returns>
    public static Vector3 GetVectorFromPoints(Vector3 Q, Vector3 P)
    {
        Vector3 V = P - Q;

        return V;
    }

    /// <summary>
    /// Applies a transformation matrix onto a set of vertices
    /// </summary>
    /// <param name="M">a transformation matrix</param>
    /// <param name="mesh">mesh which contains vertices</param>
    public static void ApplyTransformation(Matrix3x3 M, Mesh mesh)
    {
        // Get the vertices from the matrix
        Vector3[] vertices = mesh.vertices;

        //Rotate each point in the mesh to its new position
        for(int i = 0; i < vertices.Length; i++)
        {
            //vertices[i] = R.MultiplyPoint(vertices[i]);
            vertices[i] = M.MultiplyPoint(vertices[i]);
        } 

        //Set the vertices in the mesh to their new position
        mesh.vertices = vertices;

        // Recalculate the bounding volume
        mesh.RecalculateBounds();
    }
    
}
