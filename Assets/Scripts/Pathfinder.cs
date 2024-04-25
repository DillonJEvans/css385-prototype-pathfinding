using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;




/// <summary>Provides methods for finding paths.</summary>
public class Pathfinder
{
    /// <summary>
    /// Gets the shortest path from the origin to the target.
    /// All tiles in the <c>obstacles</c> tilemap are avoided.
    /// </summary>
    /// <param name="obstacles">
    /// The obstacles to avoid. All tiles are considered obstacles.
    /// </param>
    /// <param name="origin">The origin position in space.</param>
    /// <param name="target">The target position in space.</param>
    /// <returns>
    /// The shortest path from the origin to the target.
    /// The path will be a list of sequential points in space.
    /// Each point in the path is the center of a cell in the tilemap
    /// (with a z equal to the origin's z). As a result, the path will
    /// only include movements in the four cardinal directions, and
    /// will be a rigid path from cell to cell in the tilemap.
    /// The path will not include any cells that have tiles.
    /// Returns <c>null</c> if no path could be
    /// found from the origin to the target.
    /// </returns>
    public static List<Vector3> GetShortestPath(Tilemap obstacles, Vector3 origin, Vector3 target)
    {
        Vector3Int originCell = obstacles.WorldToCell(origin);
        Vector3Int targetCell = obstacles.WorldToCell(target);
        List<Vector3Int> cellPath = GetShortestPath(obstacles, originCell, targetCell);
        if (cellPath == null) return null;
        // Convert the path from a list of cells to a list of points.
        List<Vector3> pointPath = cellPath.ConvertAll(
            (Vector3Int c) =>
            {
                Vector3 point = obstacles.GetCellCenterWorld(c);
                point.z = origin.z;
                return point;
            }
        );
        if (pointPath.Count <= 1) return pointPath;
        // path[0] will always be the cell that contains the origin.
        // However, if the origin is between path[0] and path[1],
        // then there is no reason to go to path[0] just to turn around
        // and head back to path[1] (passing through the origin on the way).
        if (Vector3.Dot(origin - pointPath[0], pointPath[1] - pointPath[0]) < 0)
        {
            pointPath.RemoveAt(0);
        }
        return pointPath;
    }


    /// <summary>
    /// Gets the shortest path from the origin cell to the target cell.
    /// All tiles in the <c>obstacles</c> tilemap are avoided.
    /// </summary>
    /// <param name="obstacles">
    /// The obstacles to avoid. All tiles are considered obstacles.
    /// </param>
    /// <param name="origin">
    /// The origin cell in the tilemap.
    /// Will be the first cell in the returned path.
    /// </param>
    /// <param name="target">
    /// The target cell in the tilemap.
    /// Will be the last cell in the returned path.
    /// </param>
    /// <returns>
    /// The shortest path from the origin cell to the target cell.
    /// The path will be a list of sequential cells in the tilemap.
    /// The first cell in the path will be the origin.
    /// The last cell in the path will be the target.
    /// The path will not include any cells that have tiles.
    /// Returns <c>null</c> if no path could be
    /// found from the origin to the target.
    /// </returns>
    public static List<Vector3Int> GetShortestPath(Tilemap obstacles, Vector3Int origin, Vector3Int target)
    {
        if (obstacles.HasTile(target)) return null;

        PriorityQueue<CellNode, int> frontier = new();
        HashSet<Vector3Int> visited = new();
        frontier.Push(new(origin, null, 0), 0);

        while (frontier.Count > 0)
        {
            CellNode node = frontier.Pop();
            if (obstacles.HasTile(node.Cell)) continue;
            if (visited.Contains(node.Cell)) continue;
            visited.Add(node.Cell);

            if (node.Cell == target)
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




    #region Helpers


    /// <summary>Adds a successor to the frontier.</summary>
    /// <param name="frontier">The frontier to add to.</param>
    /// <param name="node">The node being expanded.</param>
    /// <param name="target">The target of the search.</param>
    /// <param name="direction">The direction to the successor.</param>
    private static void AddSuccessor(PriorityQueue<CellNode, int> frontier, CellNode node, Vector3Int target, Vector3Int direction)
    {
        Vector3Int cell = node.Cell + direction;
        int cost = node.Cost + 1;
        int heuristic = GetManhattanDistance(cell, target);
        CellNode successor = new(cell, node, cost);
        int priority = cost + heuristic;
        frontier.Push(successor, priority);
    }


    /// <summary>Gets the manhattan distance between two points.</summary>
    /// <param name="a">The first point.</param>
    /// <param name="b">The second point.</param>
    /// <returns>The manhattan distance.</returns>
    private static int GetManhattanDistance(Vector3Int a, Vector3Int b)
    {
        return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
    }


    /// <summary>Used by GetShortestPath(Tilemap).</summary>
    private class CellNode
    {
        public Vector3Int Cell { get; }
        public CellNode Previous { get; }
        public int Cost { get; }

        public CellNode(Vector3Int cell, CellNode previous, int cost)
        {
            Cell = cell;
            Previous = previous;
            Cost = cost;
        }

        /// <summary>Gets the path to this node.</summary>
        /// <returns>
        /// The path as a list of cells.
        /// The first cell in the path will be the origin of the search.
        /// The last cell in the path will be this node's cell.
        /// </returns>
        public List<Vector3Int> GetPath()
        {
            if (Previous == null) return new() { Cell };
            List<Vector3Int> path = Previous.GetPath();
            path.Add(Cell);
            return path;
        }
    }


    #endregion Helpers
}
