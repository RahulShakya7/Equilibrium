using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeManager : MonoBehaviour
{
    // Static variable so it survives scene changes
    public static string CurrentMode = "Story"; 

    public void PlayStoryMode()
    {
        CurrentMode = "Story";
        Debug.Log("Starting Story Mode");
        SceneManager.LoadScene("EnvBase"); 
    }

    public void PlaySimulationMode()
    {
        CurrentMode = "Simulation";
        Debug.Log("Starting Simulation Mode");
        SceneManager.LoadScene("EnvBase"); 
    }
}