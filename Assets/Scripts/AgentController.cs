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
            List<Vector3Int> path = Pathfinder.GetShortestPath(obstacles, origin, target);
            agent.StartFollowingPath(TilesToVectors(path));
        }
    }


    private Vector3 GetMousePositionInWorld(float z)
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePositionInWorld.z = z;
        return mousePositionInWorld;
    }


    // Temporary until Pathfinder is updated to
    // return Vector3s instead of Vector3Ints.
    private List<Vector3> TilesToVectors(List<Vector3Int> tiles)
    {
        List<Vector3> vectors = new();
        foreach (Vector3Int tile in tiles)
        {
            vectors.Add(new(tile.x + 0.5f, tile.y + 0.5f));
        }
        return vectors;
    }
}
