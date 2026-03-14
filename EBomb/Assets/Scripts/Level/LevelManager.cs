using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public LevelLoader loader;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            RestartToLevel1();
        }
    }

    // »Øµ½ Level1
    public void RestartToLevel1()
    {
        loader.LoadLevel(0);
    }
}