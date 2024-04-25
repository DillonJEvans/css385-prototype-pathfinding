using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class AgentController : MonoBehaviour
{
    public Tilemap obstacles;
    public PathFollower agent;


    private void Update()
    {
        if (agent != null && Input.GetKeyDown(KeyCode.Mouse0))
        {
            Vector3 origin = agent.transform.position;
            Vector3 target = GetMousePositionInWorld(origin.z);
            List<Vector3> path = Pathfinder.GetShortestPath(obstacles, origin, target);
            agent.StartFollowingPath(path);
        }
    }


    private Vector3 GetMousePositionInWorld(float z)
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePositionInWorld.z = z;
        return mousePositionInWorld;
    }
}
