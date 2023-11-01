using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBlur : MonoBehaviour
{
    public Shader depthOfFieldShader; // Assign the shader to this field
    public Vector2 focusPoint = new Vector2(0.5f, 0.5f); // Default focus point in UV space
    public float blurSize = 0.1f;

    void Update()
    {
        MaterialPropertyBlock props = new MaterialPropertyBlock();
        props.SetVector("_FocusPoint", focusPoint);
        props.SetFloat("_BlurSize", blurSize);

        // Apply the properties to the shader
        Renderer renderer = GetComponent<Renderer>(); // Assuming the shader is applied to a renderer
        renderer.SetPropertyBlock(props);
    }

    void Start()
    {
        // Ensure that the shader is assigned
        if (depthOfFieldShader != null)
        {
            Renderer renderer = GetComponent<Renderer>(); // Assuming the shader is applied to a renderer
            renderer.material.shader = depthOfFieldShader;
        }
        else
        {
            Debug.LogError("Shader not assigned!");
        }
    }
}
