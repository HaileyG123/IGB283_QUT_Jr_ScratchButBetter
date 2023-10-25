using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private IGB283Transform transformScript;

    public CameraSystem CS;
    
    public GameObject player;

    private List<GameObject> players = new List<GameObject>();
    private ArticulatedArm AA;
    private double distanceToTravel;
    
    public List<spawnPoints> spawningPoints = new List<spawnPoints>(); //sets of spawn points
    
    // Start is called before the first frame update
    void Start()
    {
        //getting the needed scripts to calcualte 
        transformScript = GetComponent<IGB283Transform>();
        
        Debug.Log(spawningPoints.Count);
        //spawns in points at desired positions  
        foreach (spawnPoints SP in spawningPoints)
        {
            GameObject copy = Instantiate(player, new Vector3(0.0f, 0.0f, 1.0f), Quaternion.identity);
            
            //Set the object's properties
            //AA = copy.GetComponent<ArticulatedArm>();
            //AA.colour = 
            Debug.Log("executed");
            
            copy.GetComponent<GetBase>()._base.MoveByOffset(SP.startPosition);
            copy.GetComponent<GetBase>()._base.colour = SP.colour;
            
            players.Add(copy);
        }

        CS.player1 = players[0].GetComponent<GetBase>()._base;
        CS.player2 = players[1].GetComponent<GetBase>()._base;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    //container for list of players
    [Serializable]
    public class spawnPoints
    {
        //spawn point positions
        public Vector3 startPosition;
        public Vector3 endPosition;
        
        public Color colour;
    }
    
    public void TranslateToStart(Vector3 start, GameObject player)
    {
        //add a meshfilter and meshrenderer to the empty GameObject
        gameObject.AddComponent<MeshFilter>();
        gameObject.AddComponent<MeshRenderer>();        
        
        Material mat = player.GetComponent<MeshRenderer>().material;
        Mesh mesh = player.GetComponent<MeshFilter>().mesh;
        
        Matrix3x3 T = IGB283Transform.Translate(start);
        IGB283Transform.ApplyTransformation(T, mesh);

    }

}
