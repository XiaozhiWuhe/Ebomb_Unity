using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D rb;  // 玩家 Rigidbody2D 组件

    private void Awake()
    {
        // 获取玩家的 Rigidbody2D 组件，确保它已经存在
        if (rb == null)
        {
            rb = GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                Debug.LogError("PlayerController: Rigidbody2D not found on player object!");
            }
        }
    }
}