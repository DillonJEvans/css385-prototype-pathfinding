using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class Pathfinder
{
    public static List<Vector3Int> GetShortestPath(Tilemap obstacles, Vector3 origin, Vector3 target)
    {
        Vector3Int originTile = VectorToTile(origin);
        Vector3Int targetTile = VectorToTile(target);
        List<Vector3Int> path = GetShortestPath(obstacles, originTile, targetTile);
        if (path == null) return path;
        if (path.Count > 1 && (origin - path[0]).sqrMagnitude > (origin - path[1]).sqrMagnitude)
        {
            path.RemoveAt(0);
        }
        return path;
    }

    public static List<Vector3Int> GetShortestPath(Tilemap obstacles, Vector3Int origin, Vector3Int target)
    {
        if (obstacles.HasTile(target)) return null;

        PriorityQueue<TileNode, int> frontier = new();
        HashSet<Vector3Int> visited = new();
        frontier.Push(new(origin, null, 0), 0);

        while (frontier.Count > 0)
        {
            TileNode node = frontier.Pop();
            if (obstacles.HasTile(node.Tile)) continue;
            if (visited.Contains(node.Tile)) continue;
            visited.Add(node.Tile);

            if (node.Tile == target)
            {
                return node.GetPath();
            }
            AddSuccessor(frontier, node, target, Vector3Int.up);
            AddSuccessor(frontier, node, target, Vector3Int.down);
            AddSuccessor(frontier, node, target, Vector3Int.left);
            AddSuccessor(frontier, node, target, Vector3Int.right);
        }

        return null;
    }


    private static Vector3Int VectorToTile(Vector3 vector)
    {
        return new(
            Mathf.FloorToInt(vector.x),
            Mathf.FloorToInt(vector.y),
            Mathf.FloorToInt(vector.z)
        );
    }

    private static void AddSuccessor(PriorityQueue<TileNode, int> frontier, TileNode node, Vector3Int target, Vector3Int direction)
    {
        Vector3Int tile = node.Tile + direction;
        int cost = node.Cost + 1;
        int heuristic = GetManhattanDistance(tile, target);
        frontier.Push(new(tile, node, cost), cost + heuristic);
    }

    private static int GetManhattanDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }

    private class TileNode
    {
        public Vector3Int Tile { get; }
        public TileNode Previous { get; }
        public int Cost { get; }

        public TileNode(Vector3Int tile, TileNode previous, int cost)
        {
            Tile = tile;
            Previous = previous;
            Cost = cost;
        }

        public List<Vector3Int> GetPath()
        {
            if (Previous == null) return new() { Tile };
            List<Vector3Int> path = Previous.GetPath();
            path.Add(Tile);
            return path;
        }
    }
}
