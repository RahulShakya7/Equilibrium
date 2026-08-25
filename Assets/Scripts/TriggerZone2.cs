using UnityEngine;

public class DecisionZoneTrigger2 : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private string targetKnotName = "decision_point_2"; 
    
    private bool hasTriggered = false; 

    private void OnTriggerEnter(Collider other)
    {
        if (hasTriggered) return; 

        if (other.CompareTag("Player"))
        {
            hasTriggered = true;
            Debug.Log("Player entered decision trigger zone 2!");

            DialogueUIController uiController = FindFirstObjectByType<DialogueUIController>();
            if (uiController != null)
            {
                uiController.SetPlayerInDecisionZone(true, targetKnotName);
            }
        }
    }

    public void ResetTrigger()
    {
        hasTriggered = false;
    }
}