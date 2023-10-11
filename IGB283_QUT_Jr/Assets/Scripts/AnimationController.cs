using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class AnimationController : MonoBehaviour
{
    public Vector3 offset = new Vector3(0.0f, 1f, 0.0f);
    public Vector3 jumpOffset = new Vector3(0.0f, 0.2f, 0.0f);
    private float jumpCounter = 0f;
    
    public ArticulatedArm _base;

    public float end;
    public float boundaryBottom;

    public bool isControlled = false;

    public float delay;
    public float totalDelay;
    
    public bool task2 = true;
    public bool task3 = true;
    
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
     
    }

    // Update is called once per frame
    void Update()
    {
        offset.y = 0; // to ensure the mesh obeys the bottom boundary
        
        //checks to see if the mesh is above the mesh boundary to which then if it apply gravity 
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
        }
        
        //Move between two points
        if(Mathf.Abs(_base.mesh.bounds.max.x) > end || Mathf.Abs(_base.mesh.bounds.min.x) > end)
        {
            Debug.Log(_base.mesh.bounds.center.x);
            //end = -end;
            
            offset.x = -offset.x;
            _base.FlipJunior();
            // scaleOffset = -scaleOffset;
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

    private void MoveUpDown()
    {
        _base.MoveByOffset(jumpOffset);
        jumpCounter += jumpOffset.y;
        //Debug.Log("jumpCounter " + jumpCounter);
        
        if (Mathf.Abs(jumpCounter) >= 5f)
        {
            jumpOffset = -jumpOffset;
            jumpCounter = 0;
        }
    }

    void input()
    {
        //if the player presses a or d then the mesh will go in the direction it was pressed 
        if (Input.GetKey(KeyCode.A))
        {
            _base.MoveByOffset(-offset);
            // isControlled = true;
            totalDelay = delay + Time.time; //resetting the delay to the new Time based on in game world time 
            
            task2 = false;
            task3 = false;
        }
        else if(Input.GetKey(KeyCode.D))
        {
            _base.MoveByOffset(offset);
            // isControlled = true;
            totalDelay = delay + Time.time;
            
            task2 = false;
            task3 = false;
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            MoveUpDown();
            task2 = true;
            task3 = false;
        }
        else
        {
            task2 = true;
            task3 = false;
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            task2 = true;
            task3 = true;
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            task2 = true;
            task3 = true;
        }
        
        // if(Time.time > totalDelay)// when no input is being pressed then turn back on auto
        // {
        //     Debug.Log("waiting done");
        //     isControlled = false;
        // }
        
    }
    
    void inputPT2()
    {
        //if the player presses a or d then the mesh will go in the direction it was pressed 
        if (Input.GetKey(KeyCode.A) && offset.x > 0)
        {
            offset.x = -offset.x;
            _base.FlipJunior();
        }
        else if(Input.GetKey(KeyCode.D) && offset.x < 0)
        {
            offset.x = -offset.x;
            _base.FlipJunior();
        }
        else if(Input.GetKeyDown(KeyCode.W))
        {
            velocity.y = jumpForceControlled; //applys the force needed
            offset.y += velocity.y; //pushes the mesh just enough above the bounds to activate the jump
        }
        else
        {
        }

        // if (Input.GetKeyUp(KeyCode.A))
        // {
        //     task2 = true;
        //     task3 = true;
        // }
        // else if (Input.GetKeyUp(KeyCode.D))
        // {
        //     task2 = true;
        //     task3 = true;
        // }
        
        // if(Time.time > totalDelay)// when no input is being pressed then turn back on auto
        // {
        //     Debug.Log("waiting done");
        //     isControlled = false;
        // }
        
    }
    //not currently used but dont want to delete yet
    IEnumerator IsControlled()
    {
        // Debug.Log("hi");
        float timeDelay = 0f;
                Debug.Log("yes1");
                timeDelay = 2f;
                yield return new WaitForSeconds(timeDelay);
                task2 = true;
                task3 = true;
                isControlled = false;



    }
}
