using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Vector3 offset = new Vector3(0.0f, 1f, 0.0f);
    public Vector3 jumpOffset = new Vector3(0.0f, 0.2f, 0.0f);
    private float jumpCounter = 0f;
    
    public ArticulatedArm _base;

    public bool isControlled = false;

    public float delay;
    public float totalDelay;
    
    public bool task2 = true;
    public bool task3 = true;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        input();
        
        if (task2 && !isControlled)
        {
            _base.MoveByOffset(offset);
        }

        if (task3 && !isControlled)
        {
            MoveUpDown();
        }
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
            isControlled = true;
            totalDelay = delay + Time.time; //resetting the delay to the new Time based on in game world time 
        }
        else if(Input.GetKey(KeyCode.D))
        {
            _base.MoveByOffset(offset);
            isControlled = true;
            totalDelay = delay + Time.time;
        }
        else if(Input.GetKeyDown(KeyCode.S))
        {
            MoveUpDown();
            task2 = true;
            task3 = false;
        }
        else
        {
        }
        
        if(Time.time > totalDelay)// when no input is being pressed then turn back on auto
        {
            Debug.Log("waiting done");
            isControlled = false;
        }
        
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
