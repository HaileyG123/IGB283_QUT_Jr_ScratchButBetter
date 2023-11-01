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
    public List<GameObject> children = new List<GameObject>();
    public GameObject control;
    
    public Vector3 jointLocation;
    public Vector3 jointOffset;

    private float angleTracker;
    public float angle;
    public float lastAngle;
    public float speed = 0.2f;
    public Vector3[] limbVertexLocations;

    private Color[] colours;
    public Color colour;
    
    public Mesh mesh;
    public Material mat;
    private PolygonCollider2D BC;
    private Rigidbody2D rb2;

    private float timeElapsed = 0.0f;
    public bool nodding = true;

    private void Awake()
    {
        colours = new Color[limbVertexLocations.Length];
        DrawLimb();
    }

    // Start is called before the first frame update
    void Start()
    {

        foreach (GameObject child in children)
        {
            //to ensure all children are moving with their parent
            if (child != null)
            {
                child.GetComponent<ArticulatedArm>().MoveByOffset(jointOffset);
            }    
        }
         
    }
    
    // Update is called once per frame
    void Update()
    {
        //nodding
        if (nodding)
        {
            foreach (GameObject child in children)
            {
                if (child != null)
                {
                    child.GetComponent<ArticulatedArm>().RotateAroundPoint(
                        jointLocation, angle, lastAngle);
                }
            }

            if(timeElapsed > 0.2f)
            {
                float temp = lastAngle;
            
                lastAngle = angle;
                angle = temp;
            
                timeElapsed = 0f;

                angleTracker = 0;
            
                //Debug.Log("change");
            }
            timeElapsed += Time.deltaTime;
        }
        else
        {
            lastAngle = angle;
            if(control != null)
            {
                angle = control.GetComponent<Slider>().value;
            }

            foreach (GameObject child in children)
            {
                if (child != null)
                {
                    child.GetComponent<ArticulatedArm>().RotateAroundPoint(jointLocation, angle, lastAngle);
                }
            }
        }
        
        //SyncColliderWithMesh();
        
        // Recalculate the bounds of the mesh
        mesh.RecalculateBounds();
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
        //Matrix3x3 M = T2 * T1;
        
        // Update the angleTracker with the current rotation angle
        angleTracker += angle * Time.deltaTime * speed;
        
        
        //Debug.Log($"todays angle {angleTracker}");
        // Move the mesh
        Vector3[] vertices = mesh.vertices;
        
        for(int i = 0; i < vertices.Length; i++) {
            vertices[i] = M.MultiplyPoint(vertices[i]);
        }
        
        mesh.vertices = vertices;
        
        // Apply the transformation to the joint
        jointLocation = M.MultiplyPoint(jointLocation);

        foreach (GameObject child in children)
        {
            // Apply the transformation to the children
            if (child != null)
            {
                child.GetComponent<ArticulatedArm>().RotateAroundPoint(
                    point, angle, lastAngle);
            }
        }
    }

    public void ColourChange(Color colour)
    {
        for (int i = 0; i < limbVertexLocations.Length; i++)
        {
            colours[i] = new Color(colour.r, colour.g, colour.b);
        } 
        mesh.colors = colours;

        foreach (GameObject child in children)
        {
            if (child != null)
            {
                child.GetComponent<ArticulatedArm>().ColourChange(colour);
            }
        }
    }
    
    private void DrawLimb()
    {
        // Add a MeshFilter and MeshRenderer to the Empty
        //GameObject
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();
        gameObject.AddComponent<PolygonCollider2D>();
        gameObject.AddComponent<Rigidbody2D>();
        
        //set mat to the material we have selected 
        GetComponent<MeshRenderer>().material = mat;
        mesh = GetComponent<MeshFilter>().mesh;

        BC = GetComponent<PolygonCollider2D>();
        rb2 = GetComponent<Rigidbody2D>();

        rb2.bodyType = RigidbodyType2D.Kinematic;
        
        
        Vector3[] tempArray = new Vector3[limbVertexLocations.Length];
        for (int i = 0; i < limbVertexLocations.Length; i++)
        {
            tempArray[i] = limbVertexLocations[i]; // sets the vertex locations through a loop
            //colours[i] = new Color(colour.r, colour.g, colour.b);
        }
        
        mesh.vertices = tempArray;
        
        Debug.Log(mesh.vertices);
        
        //mesh.colors = colours;

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

    private void SyncColliderWithMesh()
    {
            // Get the mesh vertices
            Vector3[] vertices = mesh.vertices;

            // Convert mesh vertices to local space (if necessary)
            // You may need to adjust this depending on your setup
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i] = transform.TransformPoint(vertices[i]);
            }

            // Update the PolygonCollider2D points with mesh vertices
            Vector2[] colliderPoints = new Vector2[vertices.Length];
            for (int i = 0; i < vertices.Length; i++)
            {
                colliderPoints[i] = new Vector2(vertices[i].x, vertices[i].y);
            }

            // Assign the updated points to the PolygonCollider2D
            BC.SetPath(0, colliderPoints);
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

        foreach (GameObject child in children)
        {
            //to ensure all children are moving with their parent
            if (child != null)
            {
                child.GetComponent<ArticulatedArm>().MoveByOffset(offset);
                Debug.Log(child);
            }
        }
    }

    public void FlipJunior()
    {
        // Move the point to the origin
        Matrix3x3 T1 = Translate(-jointLocation);
        // Undo the last rotation
        Matrix3x3 F = Flip();
        // Move the point back to the original position
        Matrix3x3 T2 = Translate(jointLocation);
        // Perform the new rotation
        
        // The final translation matrix
        Matrix3x3 M = T2 * F * T1;
        
        
        Vector3[] vertices = mesh.vertices; //setting the vertices for the translation
        
        //calculating the next position for the mesh
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = M.MultiplyPoint(vertices[i]);
        }

        mesh.vertices = vertices; //updating the mesh to it new position

        jointLocation = M.MultiplyPoint(jointLocation);

        foreach (GameObject child in children)
        {
            //to ensure all children are moving with their parent
            if (child != null)
            {
                child.GetComponent<ArticulatedArm>().FlipJunior();
            }
        }

        //recalculate angle
        float temp = lastAngle;
            
        lastAngle = angle;
        angle = temp;
    }

    public void ScaleJunior(Vector3 scaleOffset)
    {
        // Move the point to the origin
        Matrix3x3 T1 = Translate(-jointLocation);
        // Undo the last rotation
        Matrix3x3 S = Scale(scaleOffset.x, scaleOffset.y);
        // Move the point back to the original position
        Matrix3x3 T2 = Translate(jointLocation);
        // Perform the new rotation
        
        // The final translation matrix
        Matrix3x3 M = T2 * S * T1;
        
        
        Vector3[] vertices = mesh.vertices; //setting the vertices for the translation
        
        //calculating the next position for the mesh
        for (int i = 0; i < vertices.Length; i++)
        {
            vertices[i] = M.MultiplyPoint(vertices[i]);
        }

        mesh.vertices = vertices; //updating the mesh to it new position

        jointLocation = M.MultiplyPoint(jointLocation);

        foreach (GameObject child in children)
        {
            //to ensure all children are moving with their parent
            if (child != null)
            {
                child.GetComponent<ArticulatedArm>().ScaleJunior(scaleOffset);
            }
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

    public static Matrix3x3 Flip()
    {
        Matrix3x3 matrix = new Matrix3x3();
        
        matrix.SetRow(0, new Vector3(-1f, 0.0f, 0.0f));
        matrix.SetRow(1, new Vector3(0.0f, 1f, 0.0f));
        matrix.SetRow(2, new Vector3(0.0f, 0.0f, 1.0f));
        // Return the matrix
        return matrix;
    }
    
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
    
}


