using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSystem : MonoBehaviour
{
    public Camera cam;

    public ArticulatedArm player1;
    public ArticulatedArm player2;

    private Vector3 midpoint;
    private Vector3 lineBetweenPlayers;

    private Vector3 center = new Vector3(0.0f, 0.0f, 1.0f);

    private bool outOfView = true;
    
    // Start is called before the first frame update
    void Start()
    {
        midpoint = new Vector3(0.0f, 0.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        midpoint.x = (player1.mesh.bounds.center.x + player2.mesh.bounds.center.x) / 2;
        midpoint.y = (player1.mesh.bounds.center.y + player2.mesh.bounds.center.y) / 2;
        
        lineBetweenPlayers = IGB283Transform.GetVectorFromPoints(player1.mesh.bounds.center, player2.mesh.bounds.center);
        
        double mag = IGB283Transform.GetMagnitude(lineBetweenPlayers);
       
        outOfView = (Mathf.Abs(player1.mesh.bounds.center.x + 0.3f) > cam.orthographicSize 
                     || Mathf.Abs(player2.mesh.bounds.center.x + 0.3f) > cam.orthographicSize)
            || (Mathf.Abs(player1.mesh.bounds.center.x - 0.3f) > cam.orthographicSize 
                || Mathf.Abs(player2.mesh.bounds.center.x - 0.3f) > cam.orthographicSize);
        
        if (mag <= 10 && !outOfView)
        {
            cam.orthographicSize = 5f;
        }
        else
        {
            cam.orthographicSize = (float)(mag/2 + Mathf.Abs(midpoint.x));
            if (cam.orthographicSize < 5f)
            {
                cam.orthographicSize = 5f;
            }
        }
    }
}
