using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    // 开始游戏
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    // 退出游戏
    public void QuitGame()
    {
        Debug.Log("退出游戏");

        Application.Quit();
    }
}