using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    public Vector3 offset = new Vector3(0.0f, 1f, 0.0f);
    
    public Vector3 scaleOffset;
    public ArticulatedArm _base;

    public Collider2D Boundary1;
    public Collider2D Boundary2;
    
    
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        _base.MoveByOffset(offset);
    }

    
    
}
