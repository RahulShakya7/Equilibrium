using UnityEngine;

public class DecisionZoneTrigger1 : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string targetKnotName = "decision_point_1"; 
    
    private bool hasTriggered = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return; // Ignore if already activated

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            Debug.Log("Player entered decision trigger zone 1!");

            DialogueUIController uiController = FindFirstObjectByType<DialogueUIController>();
            if (uiController != null)
            {
                uiController.SetPlayerInDecisionZone(true, targetKnotName);
            }
            else
            {
                Debug.LogError("Could not find DialogueUIController in scene!");
            }
        }
    }

    // We only reset this if the scene reloads, or if you call ResetTrigger() manually.
    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}