using UnityEngine;

public class LevelEditor : MonoBehaviour
{
    public LevelData levelData;

    public GameObject wallPrefab;
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

        // 画墙
        foreach (var wall in levelData.wallPositions)
        {
            Instantiate(
                wallPrefab,
                GridSystem.Instance.GridToWorld(wall),
                Quaternion.identity,
                root
            );
        }

        // 出口
        Instantiate(
            exitPrefab,
            GridSystem.Instance.GridToWorld(levelData.exitPosition),
            Quaternion.identity,
            root
        );

        // 玩家标记
        Instantiate(
            playerMarkerPrefab,
            GridSystem.Instance.GridToWorld(levelData.playerStartPosition),
            Quaternion.identity,
            root
        );
    }

    void Clear()
    {
        foreach (Transform child in root)
        {
            DestroyImmediate(child.gameObject);
        }
    }
}