using UnityEngine;

public class ExitTrigger : MonoBehaviour
{
    private LevelLoader levelLoader;

    public void SetLoader(LevelLoader loader)
    {
        levelLoader = loader;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<PlayerController>() != null)
        {
            Debug.Log("进入出口");

            levelLoader.LoadNextLevel();
        }
    }
}