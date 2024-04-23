using UnityEngine;


public class TilemapUtils
{
    public static Vector3Int VectorToTile(Vector3 vector)
    {
        return new(
            Mathf.FloorToInt(vector.x),
            Mathf.FloorToInt(vector.y),
            Mathf.FloorToInt(vector.z)
        );
    }

    public static Vector3 TileToVector(Vector3Int tile)
    {
        return new(
            tile.x + 0.5f,
            tile.y + 0.5f,
            tile.z + 0.5f
        );
    }
}
