using UnityEngine;

public class BombPlacer : MonoBehaviour
{
    public GameObject bombPrefab;          // 炸弹预制体
    public Transform pointer;              // 指针对象
    public LayerMask wallLayer;            // 墙体层
    public LayerMask bombLayer;            // 炸弹层
    public Rigidbody2D playerRb;           // 玩家 Rigidbody2D
    private PointerController pointerController;

    public float holdThreshold = 0.2f;     // 长按阈值
    public float zoomFactor = 1.5f;        // 镜头放大倍数
    public Color previewColor = new Color(1f, 1f, 1f, 0.5f); // 预览颜色

    private float holdTime = 0f;
    private bool isInExplodeMode = false;

    // 当玩家移动时禁止本次按键行为
    private bool cancelCurrentPress = false;

    private GameObject bombPreview;

    private CameraController cameraController;

    void Start()
    {
        cameraController = Camera.main.GetComponent<CameraController>();

        // 获取指针控制器
        pointerController = pointer.GetComponent<PointerController>();

        if (pointerController == null)
        {
            Debug.LogError("PointerController 未找到，请检查 Pointer 对象");
        }
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            // 如果本次按键已被取消，则直接忽略
            if (cancelCurrentPress)
                return;

            holdTime += Time.unscaledDeltaTime;

            // 长按检测
            if (!isInExplodeMode && holdTime > holdThreshold)
            {
                // 在进入瞬爆模式前检查玩家速度
                if (playerRb != null && playerRb.velocity.magnitude > 0.01f)
                {
                    Debug.Log("玩家正在移动，无法进入瞬爆模式");

                    // 取消本次按键
                    cancelCurrentPress = true;
                    holdTime = 0f;
                    return;
                }

                EnterExplodeMode();
            }
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            // 如果本次按键被取消，重置状态并退出
            if (cancelCurrentPress)
            {
                cancelCurrentPress = false;
                holdTime = 0f;
                return;
            }

            if (isInExplodeMode)
            {
                ExecuteInstantExplode();
            }
            else
            {
                PlaceBomb();
            }

            holdTime = 0f;
        }
    }

    // =============================
    // 进入瞬爆模式
    // =============================
    void EnterExplodeMode()
    {
        isInExplodeMode = true;

        Time.timeScale = 0f;

        if (cameraController != null)
            cameraController.ZoomIn(zoomFactor);

        Vector3 playerPos = transform.position;
        Vector2Int gridDir = pointerController.GetCurrentDirection();
        Vector3 dir = new Vector3(gridDir.x, gridDir.y, 0);
        Vector3 targetPos = playerPos + dir * GridSystem.Instance.gridSize;

        // 检查目标是否有墙
        if (!Physics2D.OverlapPoint(targetPos, wallLayer))
        {
            bombPreview = Instantiate(bombPrefab, targetPos, Quaternion.identity);
        }
        else
        {
            Vector3 pushPos = playerPos - dir * GridSystem.Instance.gridSize;

            if (Physics2D.OverlapPoint(pushPos, wallLayer) ||
                Physics2D.OverlapPoint(pushPos, bombLayer))
            {
                Debug.Log("反方向被阻挡，无法瞬爆");
                ExitExplodeMode();
                return;
            }

            transform.position = pushPos;

            bombPreview = Instantiate(bombPrefab, playerPos, Quaternion.identity);
        }

        // 半透明预览
        SpriteRenderer sr = bombPreview.GetComponent<SpriteRenderer>();
        if (sr != null)
            sr.color = previewColor;

        // 禁用Bomb逻辑
        Bomb bomb = bombPreview.GetComponent<Bomb>();
        if (bomb != null)
            bomb.enabled = false;
    }

    // =============================
    // 执行瞬爆
    // =============================
    void ExecuteInstantExplode()
    {
        if (!BombManager.Instance.CanPlaceBomb())
        {
            Debug.Log("炸弹数量已满");
            ExitExplodeMode();
            return;
        }

        Vector3 spawnPos = bombPreview.transform.position;

        GameObject bomb = Instantiate(bombPrefab, spawnPos, Quaternion.identity);

        Bomb bombScript = bomb.GetComponent<Bomb>();
        if (bombScript != null)
            bombScript.Explode();

        ExitExplodeMode();
    }

    // =============================
    // 退出瞬爆模式
    // =============================
    void ExitExplodeMode()
    {
        Time.timeScale = 1f;

        if (cameraController != null)
            cameraController.ZoomOut();

        if (bombPreview != null)
            Destroy(bombPreview);

        isInExplodeMode = false;
    }

    // =============================
    // 普通炸弹放置
    // =============================
    void PlaceBomb()
    {
        if (playerRb != null && playerRb.velocity.magnitude > 0.01f)
        {
            Debug.Log("玩家正在移动，无法放置炸弹");
            return;
        }

        if (!BombManager.Instance.CanPlaceBomb())
        {
            Debug.Log("炸弹数量已满");
            return;
        }

        Vector2Int gridDir = pointerController.GetCurrentDirection();
        Vector3 dir = new Vector3(gridDir.x, gridDir.y, 0);
        Vector3 spawnPos = transform.position + dir * GridSystem.Instance.gridSize;


        if (Physics2D.OverlapPoint(spawnPos, bombLayer))
        {
            Debug.Log("该位置已有炸弹");
            return;
        }

        // 检查墙
        if (Physics2D.OverlapPoint(spawnPos, wallLayer))
        {
            Vector3 oldPos = transform.position;
            Vector3 pushPos = transform.position - dir * GridSystem.Instance.gridSize;

            if (Physics2D.OverlapPoint(pushPos, wallLayer) ||
                Physics2D.OverlapPoint(pushPos, bombLayer))
            {
                Debug.Log("无法推开玩家");
                return;
            }

            transform.position = pushPos;

            Instantiate(bombPrefab, oldPos, Quaternion.identity);
            BombManager.Instance.IncreaseBombCount();
        }
        else
        {
            Instantiate(bombPrefab, spawnPos, Quaternion.identity);
            BombManager.Instance.IncreaseBombCount();
        }
    }
}