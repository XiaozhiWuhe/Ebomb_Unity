using UnityEngine;

public class LevelLoader : MonoBehaviour
{
    public LevelData[] levels;

    public GameObject wallPrefab;
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
            Debug.Log("游戏完成！");
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
        foreach (Vector2Int wallPos in level.wallPositions)
        {
            Vector3 worldPos = new Vector3(wallPos.x, wallPos.y, 0);

            Instantiate(
                wallPrefab,
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
}