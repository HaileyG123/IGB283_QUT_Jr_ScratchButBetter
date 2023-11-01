using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletalTree : MonoBehaviour
{
    public GameObject rootNode;
    public List<treeNode> tree = new List<treeNode>();
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class treeNode
{
    public GameObject subParent;
    public List<leafnode1> children = new List<leafnode1>();
}

[Serializable]
public class leafnode1
{
    public GameObject subParent;
    public List<GameObject> children = new List<GameObject>();
}