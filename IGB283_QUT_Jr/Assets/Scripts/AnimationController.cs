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

    public bool task2 = true;
    public bool task3 = true;
    
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (task2)
        {
            _base.MoveByOffset(offset);
        }

        if (task3)
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
}
