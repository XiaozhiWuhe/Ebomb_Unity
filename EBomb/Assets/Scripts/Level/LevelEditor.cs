using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public LevelData levelData;

    public GameObject wallPrefab;
    public GameObject wallPrefabA;
    public GameObject wallPrefabB;

    public GameObject exitPrefab;
    public GameObject playerMarkerPrefab;

    public Transform root;

    void Start()
    {
        DrawLevel();
    }

    public void DrawLevel()
    {
        Clear();

        // ===== 画墙（支持类型）=====
        foreach (var wall in levelData.walls)
        {
            GameObject prefab = GetWallPrefab(wall.type);

            Instantiate(
                prefab,
                GridSystem.Instance.GridToWorld(wall.position),
                Quaternion.identity,
                root
            );
        }

        // ===== 出口 =====
        Instantiate(
            exitPrefab,
            GridSystem.Instance.GridToWorld(levelData.exitPosition),
            Quaternion.identity,
            root
        );

        // ===== 玩家标记 =====
        Instantiate(
            playerMarkerPrefab,
            GridSystem.Instance.GridToWorld(levelData.playerStartPosition),
            Quaternion.identity,
            root
        );
    }

    GameObject GetWallPrefab(WallType type)
    {
        switch (type)
        {
            case WallType.Normal:
                return wallPrefab;

            case WallType.TypeA:
                return wallPrefabA;

            case WallType.TypeB:
                return wallPrefabB;

            default:
                return wallPrefab;
        }
    }

    void Clear()
    {
        foreach (Transform child in root)
        {
            DestroyImmediate(child.gameObject);
        }
    }
}