using UnityEditorInternal;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelData", menuName = "EBomb/Level")]
public class LevelData : ScriptableObject
{
    public Vector2Int levelSize = new Vector2Int(10, 8);

    public Vector2Int playerStartPosition;
    public Vector2Int exitPosition;

    public WallData[] walls;
}

[System.Serializable]
public class WallData
{
    public Vector2Int position;
    public WallType type;
}
public enum WallType
{
    Normal,
    TypeA,
    TypeB
}