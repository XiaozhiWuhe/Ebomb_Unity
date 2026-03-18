using UnityEngine;
using System.Collections.Generic;

public class LevelEditorTool : MonoBehaviour
{
    public LevelData levelData;

    // 当前选择的墙类型（可用按键切换）
    public WallType currentWallType = WallType.Normal;

    void Update()
    {
        HandleWallTypeHotkey();

        if (!Input.GetMouseButtonDown(0))
            return;

        Vector3 mouseWorld =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouseWorld.z = 0;

        Vector2Int grid =
            GridSystem.Instance.WorldToGrid(mouseWorld);

        AddWall(grid);
    }

    // =============================
    // 添加墙（支持类型）
    // =============================
    void AddWall(Vector2Int pos)
    {
        var list = new List<WallData>(levelData.walls);

        // ===== 检查是否已有墙 =====
        foreach (var wall in list)
        {
            if (wall.position == pos)
            {
                Debug.Log("该位置已有墙");
                return;
            }
        }

        // ===== 添加新墙 =====
        WallData newWall = new WallData
        {
            position = pos,
            type = currentWallType
        };

        list.Add(newWall);
        levelData.walls = list.ToArray();

        Debug.Log($"Wall Added {pos} Type:{currentWallType}");
    }

    // =============================
    // 快捷键切换墙类型
    // =============================
    void HandleWallTypeHotkey()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWallType = WallType.Normal;
            Debug.Log("切换到 Normal 墙");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWallType = WallType.TypeA;
            Debug.Log("切换到 TypeA 墙");
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            currentWallType = WallType.TypeB;
            Debug.Log("切换到 TypeB 墙");
        }
    }
}