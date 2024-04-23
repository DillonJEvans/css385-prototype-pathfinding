using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class TilemapPathfinder : MonoBehaviour
{
    public float speed = 5f;
    public Tilemap walls;

    private List<Vector3Int> path = null;
    private int pathIndex = -1;


    public void Move(Vector2 position)
    {
        path = Pathfinder.GetShortestPath(walls, transform.position, position);
        pathIndex = 0;
    }


    private void Update()
    {
        if (path == null || pathIndex >= path.Count) return;
        Debug.Log("Path = " + string.Join(" -> ", path));
        Vector3 target = TileToVector(path[pathIndex]);
        Vector3 direction = target - transform.position;
        float targetDelta = direction.magnitude;
        float positionDelta = speed * Time.deltaTime;
        Vector3 position = transform.position;
        if (targetDelta <= positionDelta)
        {
            position = target;
            pathIndex++;
            if (pathIndex < path.Count)
            {
                target = path[pathIndex];
                direction = target - position;
            }
            positionDelta -= targetDelta;
        }
        position += positionDelta * direction.normalized;
        transform.position = position;
    }


    private Vector3 TileToVector(Vector3Int tile)
    {
        return new(
            tile.x + 0.5f, tile.y + 0.5f, tile.z
        );
    }
}
