using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject gameModePanel;
    public GameObject optionsPanel;

    public void Play() 
    { 
        mainMenuPanel.SetActive(false); 
        gameModePanel.SetActive(true); 
    }

    public void OpenOptions() 
    { 
        mainMenuPanel.SetActive(false); 
        optionsPanel.SetActive(true); 
    }

    public void BackToMain() 
    { 
        gameModePanel.SetActive(false); 
        optionsPanel.SetActive(false); 
        mainMenuPanel.SetActive(true); 
    }

    public void StartStoryMode() { SceneManager.LoadScene("EnvBase"); }
    public void StartSimulationMode() { SceneManager.LoadScene("EnvBase"); }

    public void QuitGame() 
    { 
        Debug.Log("Quitting Game...");
        Application.Quit(); 
    }
}