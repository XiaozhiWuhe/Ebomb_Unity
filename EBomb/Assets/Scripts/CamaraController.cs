using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour
{
    public Transform target;      // 玩家
    public float zoomSpeed = 4f;  // 缩放速度

    private Camera cam;

    private float originalSize;      // 初始摄像机大小
    private Vector3 originalPosition; // 初始摄像机位置

    private bool isZoomed = false;

    void Start()
    {
        cam = GetComponent<Camera>();

        // 记录初始状态（展示整个关卡）
        originalSize = cam.orthographicSize;
        originalPosition = transform.position;
    }

    // =========================
    // 进入瞬爆聚焦
    // =========================
    public void ZoomIn(float zoomFactor)
    {
        if (isZoomed)
            return;

        isZoomed = true;

        StopAllCoroutines();

        // 立即移动到玩家位置
        if (target != null)
        {
            transform.position = new Vector3(
                target.position.x,
                target.position.y,
                originalPosition.z
            );
        }

        float targetSize = originalSize / zoomFactor;

        StartCoroutine(ZoomCoroutine(
            transform.position,
            targetSize
        ));
    }

    // =========================
    // 退出瞬爆
    // =========================
    public void ZoomOut()
    {
        if (!isZoomed)
            return;

        isZoomed = false;

        StopAllCoroutines();

        StartCoroutine(ZoomCoroutine(
            originalPosition,
            originalSize
        ));
    }

    // =========================
    // 平滑插值位置 + 缩放
    // =========================
    IEnumerator ZoomCoroutine(Vector3 targetPos, float targetSize)
    {
        Vector3 startPos = transform.position;
        float startSize = cam.orthographicSize;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.unscaledDeltaTime * zoomSpeed;

            transform.position = Vector3.Lerp(
                startPos,
                targetPos,
                t
            );

            cam.orthographicSize = Mathf.Lerp(
                startSize,
                targetSize,
                t
            );

            yield return null;
        }

        transform.position = targetPos;
        cam.orthographicSize = targetSize;
    }
}