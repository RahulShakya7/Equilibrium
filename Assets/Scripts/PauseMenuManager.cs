using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject pausePanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausePanel.activeSelf) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Freezes the game
        Debug.Log("Game Paused");
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Unfreezes the game
        Debug.Log("Game Resumed");
    }

    public void Restart()
    {
        Time.timeScale = 1f; // CRITICAL: Always reset time before reloading
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f; // CRITICAL: Always reset time before loading menu
        SceneManager.LoadScene("MainMenu");
    }
}