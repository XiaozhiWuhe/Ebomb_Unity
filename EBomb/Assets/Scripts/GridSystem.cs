using UnityEngine;

public class GridSystem : MonoBehaviour
{
    public static GridSystem Instance;

    public float gridSize = 1f;

    void Awake()
    {
        Instance = this;
    }

    // Grid -> World
    public Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(
            gridPos.x * gridSize,
            gridPos.y * gridSize,
            0
        );
    }

    // World -> Grid
    public Vector2Int WorldToGrid(Vector3 worldPos)
    {
        return new Vector2Int(
            Mathf.RoundToInt(worldPos.x / gridSize),
            Mathf.RoundToInt(worldPos.y / gridSize)
        );
    }
}