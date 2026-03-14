using UnityEngine;

public class BombManager : MonoBehaviour
{
    public static BombManager Instance;  // 单例实例
    private int bombCount = 0;           // 当前炸弹数量
    public const int maxBombCount = 2;   // 最大炸弹数量

    private void Awake()
    {
        // 确保 BombManager 只存在一个实例
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // 增加炸弹数量
    public void IncreaseBombCount()
    {
        if (bombCount < maxBombCount)
        {
            bombCount++;
        }
        else
        {
            Debug.Log("已达到最大炸弹数量");
        }
    }

    // 减少炸弹数量
    public void DecreaseBombCount()
    {
        if (bombCount > 0)
        {
            bombCount--;
        }
        else
        {
            Debug.Log("炸弹数量为0");
        }
    }

    // 获取当前炸弹数量
    public int GetBombCount()
    {
        return bombCount;
    }

    // 检查是否可以放置炸弹
    public bool CanPlaceBomb()
    {
        return bombCount < maxBombCount;
    }
}