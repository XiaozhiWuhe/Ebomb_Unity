using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "EBomb/Level")]
public class LevelData : ScriptableObject
{
    public Vector2Int levelSize = new Vector2Int(10, 8);

    public Vector2Int playerStartPosition;
    public Vector2Int exitPosition;

    public Vector2Int[] wallPositions;
}