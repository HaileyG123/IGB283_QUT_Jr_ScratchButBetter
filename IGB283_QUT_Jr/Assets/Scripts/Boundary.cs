using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public AnimationController ani;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Mesh"))
        {
            Debug.Log("hi");
            
            ani.offset = -ani.offset;
        }
    }
}
