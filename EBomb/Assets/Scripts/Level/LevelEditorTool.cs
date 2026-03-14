using UnityEngine;

public class LevelEditorTool : MonoBehaviour
{
    public LevelData levelData;

    void Update()
    {
        if (!Input.GetMouseButtonDown(0))
            return;

        Vector3 mouseWorld =
            Camera.main.ScreenToWorldPoint(Input.mousePosition);

        mouseWorld.z = 0;

        Vector2Int grid =
            GridSystem.Instance.WorldToGrid(mouseWorld);

        AddWall(grid);
    }

    void AddWall(Vector2Int pos)
    {
        var list = new System.Collections.Generic.List<Vector2Int>(
            levelData.wallPositions
        );

        if (!list.Contains(pos))
        {
            list.Add(pos);
            levelData.wallPositions = list.ToArray();

            Debug.Log("Wall Added " + pos);
        }
    }
}