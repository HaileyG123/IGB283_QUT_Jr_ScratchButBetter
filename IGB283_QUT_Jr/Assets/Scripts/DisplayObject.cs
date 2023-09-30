using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DisplayObject : MonoBehaviour
{
    private Mesh mesh;
    public Material mat; //need to set material otherwise the mesh will ignore the colours being set here as default mesh doesnt support colours 
    //Angle by which the triangle will rotate each frame
    public float angle = 10f;

    // public static float speed;
    
    // Offset to the center of the rectangle
    private Vector3 offset = new Vector3(0.1f, 0, 0);
    private Vector3 scaleOffset = new Vector3(0.01f, 0.01f);

    //scale factors in x and y direction
    public float sx;
    public float sy;



    public float end; // getter and setter methods for end property which get sets in ShapeSpawner for each clone 

    //definingh the colours array which length is set based on the amount of vertices
    private Color[] colours;
    
    // Start is called before the first frame update
    void Start()
    {       
        //add a meshfilter and meshrenderer to the empty GameObject
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
     
        //set mat to the material we have selected 
        GetComponent<MeshRenderer>().material = mat;

        mesh = GetComponent<MeshFilter>().mesh;

        Display();
        Scale();

       colours = new Color[mesh.vertices.Length];

    }

    // Update is called once per frame
    void Update()
    {
        // Get the vertices from the matrix
        Vector3[] vertices = mesh.vertices;
        
        // Get rotation   
        Matrix3x3 R = IGB283Transform.Rotate(angle * Time.deltaTime);
        Matrix3x3 T2 = IGB283Transform.Translate(mesh.bounds.center + offset);
        Matrix3x3 S = IGB283Transform.Scale(1f + scaleOffset.x, 1f + scaleOffset.y);
        
        //invere time everybody
        Matrix3x3 T = IGB283Transform.Translate(-mesh.bounds.center);
        
        Matrix3x3 M = T2 * R * T * S;
        
        
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
        
        //changing colours dynamically
        ColorWheel();
        
        //Move between two points
        if(mesh.bounds.center.x >= end  || mesh.bounds.center.x <= 0)
        {
            offset = -offset;
            scaleOffset = -scaleOffset;
        }
        
    }

    void Scale()
    {
       // Get the vertices from the matrix
        Vector3[] vertices = mesh.vertices;

        // Get Scale
        Matrix3x3 M = IGB283Transform.Scale(sx, sy);

        //Rotate each point in the mesh to its new position
        for(int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = M.MultiplyPoint(vertices[i]);
        } 

        //Set the vertices in the mesh to their new position
        mesh.vertices = vertices;

        // Recalculate the bounding volume
        mesh.RecalculateBounds(); 
    }

    void Display()
    {
        //display object 
        
        //define object
        mesh.vertices = new Vector3[] {
            new Vector3(-7, 18, 0), 
            new Vector3(-7, 5, 0),
            new Vector3(16, 5, 0),
            new Vector3(16, 18, 0),
            new Vector3(-11, -4, 0),
            new Vector3(-11, 0, 0),
            new Vector3(-8, 0, 0),
            new Vector3(-8, -6, 0),
            new Vector3(-4, -6, 0),
            new Vector3(-4, -9, 0),
            new Vector3(3, -9, 0),
            new Vector3(11, -6, 0),
            new Vector3(11, -9, 0),
            new Vector3(15, -9, 0),
            new Vector3(14, -4, 0),
            new Vector3(17, -3, 0),
            new Vector3(16, 0 , 0),
            new Vector3(17, 0, 0),
            new Vector3(20, 0, 0),
            new Vector3(20, -4, 0),
            new Vector3(5, -6, 0),
            new Vector3(3, -6, 0),
            new Vector3(5, 5, 0),            
        };

        //specify triangle mesh
        mesh.triangles = new int[]{
            1,0,2,
            0,3,2,
            4,5,6,
            6,8,7,
            7,8,9,
            8,10,9,
            8,11,10,
            10,11,12,
            11,13,12,
            11,15,13,
            14,16,15,
            16,17,15,
            17,18,19,
            21,22,20
        };

        mesh.colors = colours;
        // {
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.4f, 0.3f, 0.7f),
        //     new Color(0.4f, 0.3f, 0.7f),
        //     new Color(0.4f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        //     new Color(0.8f, 0.3f, 0.7f),
        // };

    }

    public void ColorWheel()
    {
        Color startColor = new Color(0.4f, 0.3f, 0.7f);
        Color endColor = new Color(0.6f, 0.9f, 0.7f);

        float normalizedColor = mesh.bounds.center.x / end;

        for (int i = 0; i < mesh.vertices.Length; i++)
        {
            colours[i] = Color.Lerp(startColor, endColor, normalizedColor);
        }

        mesh.colors = colours;
    }
    
}
