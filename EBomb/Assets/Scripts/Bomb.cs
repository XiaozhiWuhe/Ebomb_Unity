using UnityEngine;

public class Bomb : MonoBehaviour
{
    Vector2Int placedDirection;  // 记录炸弹放置时的指针相对位置
    private PointerController pointerController;  // 引用 PointerController 脚本
    private PlayerController playerController;  // 引用 PlayerController 脚本

    public float explosionForce = 10f;  // 可调节的爆炸冲击力
    public float explosionRadius = 0.4f; // 爆炸半径
    public LayerMask bombLayer;        // 用于检测炸弹的层
    private bool isExploding = false;  // 防止重复爆炸的标志


    private void Awake()
    {
        // 获取 PointerController 组件
        pointerController = FindObjectOfType<PointerController>();

        // 获取 PlayerController 组件
        playerController = FindObjectOfType<PlayerController>();

        // 订阅指针方向变化事件
        pointerController.OnDirectionChanged.AddListener(OnDirectionChanged);
    }

    private void Start()
    {
        // 记录炸弹放置时的指针位置
        placedDirection = pointerController.GetCurrentDirection();
    }

    void OnDirectionChanged(Vector2Int newDir)
    {
        if (newDir == placedDirection)
        {
            Explode();
        }
    }

    public void Explode()
    {
        // 防止炸弹重复爆炸
        if (isExploding)
            return;

        // 设置炸弹正在爆炸
        isExploding = true;

        // 打印炸弹爆炸信息并销毁炸弹
        Debug.Log("Bomb exploded!");

        // 获取玩家对象
        if (playerController != null)
        {
            // 计算炸弹与玩家之间的方向
            Vector2 explosionDirection = (playerController.transform.position - transform.position).normalized;

            // 施加冲击力，反向推动玩家
            Rigidbody2D playerRb = playerController.GetComponent<Rigidbody2D>();
            playerRb.AddForce(explosionDirection * explosionForce, ForceMode2D.Impulse);

            // 确保玩家会受到重力影响
            playerRb.gravityScale = 1; // 使玩家在爆炸后受到重力影响

            Debug.Log("Player affected by explosion!");
        }

        // 通知 BombManager 减少炸弹数量
        BombManager.Instance.DecreaseBombCount();

        // 销毁炸弹对象
        Destroy(gameObject);

        // 触发连锁爆炸
        TriggerChainExplosion();
    }

    // 连锁爆炸：触发周围炸弹的爆炸
    private void TriggerChainExplosion()
    {
        // 获取炸弹爆炸周围的所有炸弹（上、下、左、右、左上、右上、左下、右下）
        Vector3[] directions = new Vector3[]
        {
            Vector3.up, Vector3.down, Vector3.left, Vector3.right,
            new Vector3(-1, 1, 0), new Vector3(1, 1, 0), new Vector3(-1, -1, 0), new Vector3(1, -1, 0)
        };

        // 检测相邻炸弹并触发它们的爆炸
        foreach (Vector3 direction in directions)
        {
            Vector3 adjacentPosition = transform.position + direction * GridSystem.Instance.gridSize;

            // 使用 OverlapCircle 检测相邻格子中是否有炸弹
            Collider2D[] nearbyBombs = Physics2D.OverlapCircleAll(adjacentPosition, explosionRadius, bombLayer);

            // 对每个检测到的炸弹进行爆炸
            foreach (Collider2D bombCollider in nearbyBombs)
            {
                Bomb nearbyBomb = bombCollider.GetComponent<Bomb>();
                if (nearbyBomb != null && !nearbyBomb.isExploding) // 如果是炸弹且没有爆炸
                {
                    nearbyBomb.Explode();  // 触发其爆炸
                }
            }
        }
    }

    private void OnDestroy()
    {
        // 在炸弹销毁时取消订阅，避免内存泄漏
        pointerController.OnDirectionChanged.RemoveListener(OnDirectionChanged);
    }
}