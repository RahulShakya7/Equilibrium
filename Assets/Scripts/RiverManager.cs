using System.Collections;
using UnityEngine;

public class RiverStateManager : MonoBehaviour
{
    [Header("River State Containers")]
    [SerializeField] private GameObject healthyRiver;   // State 0
    [SerializeField] private GameObject restoredRiver;  // State 1
    [SerializeField] private GameObject chokedRiver;    // State 2

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 2.5f;

    private GameObject currentState;

    private void Start()
    {
        Debug.Log("[RiverStateManager] Initializing...");

        if (healthyRiver == null) Debug.LogError("[RiverStateManager] Healthy River reference is missing in the Inspector!");
        if (restoredRiver == null) Debug.LogError("[RiverStateManager] Restored River reference is missing in the Inspector!");
        if (chokedRiver == null) Debug.LogError("[RiverStateManager] Choked River reference is missing in the Inspector!");

        // Activate only the healthy river at start
        if (healthyRiver) healthyRiver.SetActive(true);
        if (restoredRiver) restoredRiver.SetActive(false);
        if (chokedRiver) chokedRiver.SetActive(false);
        currentState = healthyRiver;

        Debug.Log($"[RiverStateManager] Initialized. Active river state: {(currentState != null ? currentState.name : "None")}");
    }

    public void TransitionToRiverState(int stateIndex)
    {
        Debug.Log($"[RiverStateManager] TransitionToRiverState called with index: {stateIndex}");

        GameObject targetState = stateIndex switch
        {
            1 => restoredRiver,
            2 => chokedRiver,
            _ => healthyRiver
        };

        if (targetState == null)
        {
            Debug.LogError($"[RiverStateManager] Target GameObject for river state index {stateIndex} is NULL!");
            return;
        }

        if (targetState == currentState)
        {
            Debug.LogWarning($"[RiverStateManager] Already in river state '{targetState.name}'. Transition skipped.");
            return;
        }

        Debug.Log($"[RiverStateManager] Starting transition from '{(currentState != null ? currentState.name : "None")}' to '{targetState.name}' over {transitionDuration}s.");
        StopAllCoroutines();
        StartCoroutine(CrossFadeRiverRoutine(currentState, targetState));
    }

    private IEnumerator CrossFadeRiverRoutine(GameObject fromState, GameObject toState)
    {
        // Save the correct scale of the target river (prevents the giant block bug)
        Vector3 targetScale = toState.transform.localScale;

        toState.SetActive(true);
        toState.transform.localScale = Vector3.zero;
        Debug.Log($"[RiverStateManager] CrossFade Started. {toState.name} scaled to 0. Target scale is {targetScale}.");

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / transitionDuration);

            if (fromState != null) fromState.transform.localScale = Vector3.Lerp(targetScale, Vector3.zero, t);
            toState.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);

            yield return null;
        }

        // Final cleanup
        if (fromState != null)
        {
            fromState.SetActive(false);
            fromState.transform.localScale = targetScale; 
        }
        toState.transform.localScale = targetScale;
        currentState = toState;

        Debug.Log($"[RiverStateManager] CrossFade Completed! Current active river object: {currentState.name}");
    }
}