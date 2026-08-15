using UnityEngine;

public class DecisionZoneTrigger : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string targetKnotName = "decision_point_1"; 
    
    private bool hasTriggered = false; // Prevents repeating calls

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return; // Ignore if already activated

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            Debug.Log("Player entered decision trigger zone!");

            DialogueUIController uiController = Object.FindFirstObjectByType<DialogueUIController>();
            if (uiController != null)
            {
                uiController.SetPlayerInDecisionZone(true, targetKnotName);
            }
        }
    }

    // Reset if you ever need to re-use this trigger zone
    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}