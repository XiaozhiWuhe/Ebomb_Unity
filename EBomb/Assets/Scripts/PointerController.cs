using UnityEngine;
using UnityEngine.Events;
using System.Collections;

public class PointerController : MonoBehaviour
{
    public Transform player;

    public float rotationInterval = 0.5f;

    private int currentRotationIndex = 0;

    // Grid方向
    private Vector2Int[] directions =
    {
        Vector2Int.up,
        Vector2Int.right,
        Vector2Int.down,
        Vector2Int.left
    };

    // 方向变化事件
    public UnityEvent<Vector2Int> OnDirectionChanged;

    void Start()
    {
        UpdatePointerPosition();
        StartCoroutine(RotatePointer());
    }

    IEnumerator RotatePointer()
    {
        while (true)
        {
            yield return new WaitForSeconds(rotationInterval);

            currentRotationIndex =
                (currentRotationIndex + 1) % directions.Length;

            UpdatePointerPosition();

            OnDirectionChanged?.Invoke(GetCurrentDirection());
        }
    }

    void UpdatePointerPosition()
    {
        Vector2Int dir = directions[currentRotationIndex];

        float grid = GridSystem.Instance.gridSize;

        // 设置指针位置
        transform.localPosition = new Vector3(
            dir.x * grid,
            dir.y * grid,
            0
        );

        // 设置箭头方向
        transform.rotation =
            Quaternion.LookRotation(
                Vector3.forward,
                new Vector3(dir.x, dir.y, 0)
            );
    }

    public Vector2Int GetCurrentDirection()
    {
        return directions[currentRotationIndex];
    }
}