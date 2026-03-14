using UnityEngine;

public class LevelCamera : MonoBehaviour
{
    public float padding = 2f;

    public void FitLevel(LevelData level)
    {
        Camera cam = GetComponent<Camera>();

        float width = level.levelSize.x;
        float height = level.levelSize.y;

        float aspect = (float)Screen.width / Screen.height;

        float sizeX = width / aspect / 2f;
        float sizeY = height / 2f;

        cam.orthographicSize =
            Mathf.Max(sizeX, sizeY) + padding;

        transform.position = new Vector3(
            width / 2f,
            height / 2f,
            -10
        );
    }
}