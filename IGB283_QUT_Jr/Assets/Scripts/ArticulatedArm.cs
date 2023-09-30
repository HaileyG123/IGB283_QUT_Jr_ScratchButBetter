using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.U2D.IK;
using UnityEngine.UI;

public class ArticulatedArm : MonoBehaviour
{
    //variables to allow the set and establish communication between 
    //each section
    public GameObject child;
    public GameObject control;
    
    public Vector3 jointLocation;
    public Vector3 jointOffset;
    public Vector3 offset = new Vector3(0.5f, 0.0f, 0.0f);
    
    public float angle;
    public float lastAngle;
    public float speed = 0.2f;
    public Vector3[] limbVertexLocations;

    private Color[] colours;
    
    public Mesh mesh;
    public Material mat;

    private float timeElapsed = 0.0f;

    private void Awake()
    {
        colours = new Color[limbVertexLocations.Length];
        DrawLimb();
    }

    // Start is called before the first frame update
    void Start()
    {
        //to ensure all children are moving with their parent
        if (child != null)
        {
            child.GetComponent<ArticulatedArm>().MoveByOffset(jointOffset);
        } 
    }
    
    // Update is called once per frame
    void Update()
    {
        /*lastAngle = angle;
        if (control != null) {
            angle = control.GetComponent<Slider>().value;
        }
        if (child != null) {
            child.GetComponent<ArticulatedArm>().RotateAroundPoint(
                jointLocation, angle, lastAngle);
        }*/
        
        //nodding
        if(child != null)
        {
            child.GetComponent<ArticulatedArm>().RotateAroundPoint(
                jointLocation, angle, lastAngle);
        }
        
        // Recalculate the bounds of the mesh
        mesh.RecalculateBounds();
        
        if (timeElapsed > 0.2f)
        {
            float temp = lastAngle;
            
            lastAngle = angle;
            angle = temp;
            
            timeElapsed = 0f;
            //Debug.Log("inside " + timeElapsed);
        }

        timeElapsed += Time.deltaTime;
        //Debug.Log(timeElapsed);
    }
    //Rotate the limb around a point 
    public void RotateAroundPoint(Vector3 point, float angle,
        float lastAngle)
    {
        // Move the point to the origin
        Matrix3x3 T1 = Translate(-point);
        // Undo the last rotation
        Matrix3x3 R1 = Rotate(-lastAngle * Time.deltaTime * speed);
        // Move the point back to the original position
        Matrix3x3 T2 = Translate(point);
        // Perform the new rotation
        Matrix3x3 R2 = Rotate(angle * Time.deltaTime * speed);
        // The final translation matrix
        Matrix3x3 M = T2 * R2 * R1 * T1;
        
        // Move the mesh
        Vector3[] vertices = mesh.vertices;
        
        for(int i = 0; i < vertices.Length; i++) {
            vertices[i] = M.MultiplyPoint(vertices[i]);
        }
        
        mesh.vertices = vertices;
        
        // Apply the transformation to the joint
        jointLocation = M.MultiplyPoint(jointLocation);
        
        // Apply the transformation to the children
        if (child != null) {
            child.GetComponent<ArticulatedArm>().RotateAroundPoint(
                point, angle, lastAngle);
        }

        
    }
    

    private void DrawLimb()
    {
        // Add a MeshFilter and MeshRenderer to the Empty
        //GameObject
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();

        
        
        //set mat to the material we have selected 
        GetComponent<MeshRenderer>().material = mat;
        mesh = GetComponent<MeshFilter>().mesh;

        Vector3[] tempArray = new Vector3[limbVertexLocations.Length];
        for (int i = 0; i < limbVertexLocations.Length; i++)
        {
            tempArray[i] = limbVertexLocations[i]; // sets the vertex locations through a loop
            colours[i] = new Color(0.8f, 0.3f, 0.3f, 1.0f);
        }
        
        mesh.vertices = tempArray;
        
        Debug.Log(mesh.vertices);

        
        // mesh.vertices = new Vector3[]
        // {
        //     limbVertexLocations[0],
        //     limbVertexLocations[1],
        //     limbVertexLocations[2],
        //     limbVertexLocations[3]
        // };
        
        mesh.colors = colours;
            
        //     new Color[] {
        //     new Color(0.8f, 0.3f, 0.3f, 1.0f),
        //     new Color(0.8f, 0.3f, 0.3f, 1.0f),
        //     new Color(0.8f, 0.3f, 0.3f, 1.0f),
        //     new Color(0.8f, 0.3f, 0.3f, 1.0f)
        // }; 
        //

        if (limbVertexLocations.Length == 4)
        {
            mesh.triangles = new int[]
            {
                0, 1, 2,
                0, 2, 3
            };
        }
        else
        {
            mesh.triangles = new int[]
            {
                0, 1, 2
            };
        }
    }
    
    public void MoveByOffset (Vector3 offset) {
        
        // Find the translation Matrix
        Matrix3x3 T = Translate(offset);

        Vector3[] vertices = mesh.vertices; //setting the vertices for the translation
        
        //calculating the next position for the mesh
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = T.MultiplyPoint(vertices[i]);
        }

        mesh.vertices = vertices; //updating the mesh to it new position

        jointLocation = T.MultiplyPoint(jointLocation);
        
        //to ensure all children are moving with their parent
        if (child != null)
        {
            child.GetComponent<ArticulatedArm>().MoveByOffset(offset);
        } 
    }

    // Rotate a vertex around the origin
    public static Matrix3x3 Rotate(float angle)
    {
        // Create a new matrix
        Matrix3x3 matrix = new Matrix3x3();
        // Set the rows of the matrix
        matrix.SetRow(0, new Vector3(Mathf.Cos(angle),
            -Mathf.Sin(angle), 0.0f));
        matrix.SetRow(1, new Vector3(Mathf.Sin(angle),
            Mathf.Cos(angle), 0.0f));
        matrix.SetRow(2, new Vector3(0.0f, 0.0f, 1.0f));
        // Return the matrix
        return matrix;
    }

    // Translate the mesh
    public static Matrix3x3 Translate(Vector3 offset)
    {
        // Create a new matrix
        Matrix3x3 matrix = new Matrix3x3();
        // Set the rows of the matrix
        matrix.SetRow(0, new Vector3(1.0f, 0.0f, offset.x));
        matrix.SetRow(1, new Vector3(0.0f, 1.0f, offset.y));
        matrix.SetRow(2, new Vector3(0.0f, 0.0f, 1.0f));
        // Return the matrix
        return matrix;
    }
}


