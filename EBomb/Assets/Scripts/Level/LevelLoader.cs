using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public LevelData[] levels;

    public GameObject wallPrefab;
    public GameObject wallPrefabA;  // 新墙1
    public GameObject wallPrefabB;  // 新墙2
    public GameObject exitPrefab;

    public Transform levelParent;

    public PlayerController player;

    private int currentLevelIndex = 0;

    private GameObject currentExit;
    public LevelCamera levelCamera;

    public void Start()
    {
        LoadLevel(0);
    }

    public void LoadNextLevel()
    {
        currentLevelIndex++;

        if (currentLevelIndex >= levels.Length)
        {
            Debug.Log("游戏完成！返回主菜单");

            // 加载主菜单场景
            SceneManager.LoadScene("MainMenuScene");

            return;
        }

        LoadLevel(currentLevelIndex);
    }

    public void LoadLevel(int index)
    {
        ClearLevel();

        // 先获取关卡数据
        LevelData level = levels[index];

        // 相机适配关卡
        levelCamera.FitLevel(level);

        // ===== 玩家位置 =====
        player.transform.position = new Vector3(
            level.playerStartPosition.x,
            level.playerStartPosition.y,
            0
        );

        // ===== 生成墙体 =====
        foreach (var wall in level.walls)
        {
            Vector3 worldPos = new Vector3(wall.position.x, wall.position.y, 0);

            GameObject prefab = GetWallPrefab(wall.type);

            Instantiate(
                prefab,
                worldPos,
                Quaternion.identity,
                levelParent
            );
        }

        // ===== 出口 =====
        Vector3 exitPos = new Vector3(
            level.exitPosition.x,
            level.exitPosition.y,
            0
        );

        currentExit = Instantiate(
            exitPrefab,
            exitPos,
            Quaternion.identity,
            levelParent
        );

        currentExit.GetComponent<ExitTrigger>().SetLoader(this);
    }

    void ClearLevel()
    {
        foreach (Transform child in levelParent)
        {
            Destroy(child.gameObject);
        }
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
}