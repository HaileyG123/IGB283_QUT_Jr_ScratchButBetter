using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimationController : MonoBehaviour
{
    [Header("Movement")]
    public Vector3 offset = new Vector3(3f, 1f, 0.0f);
    public Vector3 jumpOffset = new Vector3(0.0f, 0.2f, 0.0f);
    private float jumpCounter = 0f;
    private float offsetX;
    private int direction = 1;
    
    [Header("Figure")]
    public ArticulatedArm _base;
    public ArticulatedArm upperArm;
    public ArticulatedArm lowerArm;

    [Header("Controls")] 
    public string moveLeft;
    public string moveRight;
    public string jumpUp;
    public string jumpForward;
    public string collapse;
    

    [Header("Boundaries")]
    public float end;
    public float boundaryBottom;

    public bool isControlled = false;

    //public float delay;
    //public float totalDelay;
    
    [Header("Modes")]
    public bool task2 = true;
    public bool task3 = true;
    public bool twoPlayerMode = true;
    
    //artifical gravity
    public float gravity;
    public Vector2 velocity;
    public float jumpForceNatural;
    public float jumpForceControlled;
    
    //to keep track of the state the mesh is in
    public enum states
    {
        grounded,
        airTime
    }

    private states meshesState;
    
    // Start is called before the first frame update
    void Start()
    {
        offsetX = offset.x;
        if (offset.x < 0)
        {
            direction = -direction;
        }
        else if (offset.x == 0)
        {
            Debug.Log("invalid start offset");
        }
        else
        {
            direction = direction;
        }
    }

    // Update is called once per frame
    void Update()
    {
        offset.y = 0; // to ensure the mesh obeys the bottom boundary
        
        //checks to see if the mesh is above the mesh boundary to which then if it is apply gravity 
        if(_base.mesh.bounds.min.y > boundaryBottom)
        {
            meshesState = states.airTime;
            velocity.y -= gravity * Time.deltaTime;
            offset.y += velocity.y;
        }
        else
        {
            meshesState = states.grounded;
        }
        
        
        Debug.Log(meshesState);

        if (meshesState == states.grounded)
        {
            velocity.y = jumpForceNatural;
            offset.y += velocity.y;
            if (direction == 1)
            {
                offset.x = 3;
            }
            else
            {
                offset.x = -3;
            }
        }
        
        //Move between two points
        if(Mathf.Abs(_base.mesh.bounds.max.x) > end || Mathf.Abs(_base.mesh.bounds.min.x) > end)
        {
            //Debug.Log(_base.mesh.bounds.center.x);
            //end = -end;
            
            offset.x = -offset.x;
            _base.FlipJunior();
            // scaleOffset = -scaleOffset;
            direction = -direction;
        }
        
        inputPT2();
        
        if (task2)
        {
            _base.MoveByOffset(offset * Time.deltaTime);
        }

        // if (task3)
        // {
        //     MoveUpDown();
        // }
    }
    
    void inputPT2()
    {
        //if the player presses a or d then the mesh will go in the direction it was pressed 
        if (Input.GetKey(moveLeft) && offset.x > 0)
        {
            offset.x = -offset.x;
            _base.FlipJunior();
            direction = -direction;
        }
        else if(Input.GetKey(moveRight) && offset.x < 0)
        {
            offset.x = -offset.x;
            _base.FlipJunior();
            direction = -direction;
        }
        else if(Input.GetKeyDown(jumpUp))
        {
            velocity.y = jumpForceControlled; //applys the force needed
            offset.y += velocity.y; //pushes the mesh just enough above the bounds to activate the jump
            offset.x = 0;
        }
        else if(Input.GetKeyDown(jumpForward))
        {
            velocity.y = jumpForceControlled; //applys the force needed
            offset.y += velocity.y; //pushes the mesh just enough above the bounds to activate the jump
        }
        else if (Input.GetKeyDown(collapse))
        {
            offset.x = 0;
            offset.y = 0;

            lowerArm.nodding = false;

            upperArm.RotateAroundPoint(_base.jointLocation, 90, _base.lastAngle);
            //false slider class and restore rotation code from workshop 7
            //modify false slider value
            //remove every call to rotate around point other than the workshop
            
            //StartCoroutine(Collapse());
        }
        
        // if(Time.time > totalDelay)// when no input is being pressed then turn back on auto
        // {
        //     Debug.Log("waiting done");
        //     isControlled = false;
        // }
        
    }

    IEnumerator Collapse()
    {
        Debug.Log("this has fired");
        //rotate upper arm by 45 degrees
        upperArm.RotateAroundPoint(upperArm.jointLocation, -45, upperArm.lastAngle);
        //wait 0.5 secs
        yield return new WaitForSeconds(0.5f);
        //rotate lower arm by 45 degrees
        lowerArm.RotateAroundPoint(lowerArm.jointLocation, -45, upperArm.lastAngle);
        //wait a bit
        yield return new WaitForSeconds(2.5f);
        //reverse that rotation
        lowerArm.RotateAroundPoint(lowerArm.jointLocation, 45, upperArm.lastAngle);
        yield return new WaitForSeconds(0.5f);
        upperArm.RotateAroundPoint(upperArm.jointLocation, 45, lowerArm.lastAngle);
        yield return new WaitForSeconds(1f);
    }
    
    //not currently used but dont want to delete yet
    IEnumerator IsControlled()
    {
        float timeDelay = 0f;
                Debug.Log("yes1");
                timeDelay = 2f;
                yield return new WaitForSeconds(timeDelay);
                task2 = true;
                task3 = true;
                isControlled = false;
    }
    
    //not used but keep just in case
    private void MoveUpDown()
    {
        _base.MoveByOffset(jumpOffset);
        jumpCounter += jumpOffset.y;
        
        if (Mathf.Abs(jumpCounter) >= 5f)
        {
            jumpOffset = -jumpOffset;
            jumpCounter = 0;
        }
    }
}
