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
        // Reference Validation
        if (healthyRiver == null) Debug.LogError("[RiverStateManager] Healthy River reference is missing in the Inspector!");
        if (restoredRiver == null) Debug.LogError("[RiverStateManager] Restored River reference is missing in the Inspector!");
        if (chokedRiver == null) Debug.LogError("[RiverStateManager] Choked River reference is missing in the Inspector!");

        Debug.Log("[RiverStateManager] Script initialized successfully.");
        SetInitialState(healthyRiver);
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
            Debug.LogError($"[RiverStateManager] Target GameObject for river state index {stateIndex} is NULL! Ensure slots are assigned in the Inspector.");
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

    private void SetInitialState(GameObject activeState)
    {
        currentState = activeState;
        if (healthyRiver) healthyRiver.SetActive(healthyRiver == activeState);
        if (restoredRiver) restoredRiver.SetActive(restoredRiver == activeState);
        if (chokedRiver) chokedRiver.SetActive(chokedRiver == activeState);

        if (healthyRiver) healthyRiver.transform.localScale = Vector3.one;
        if (restoredRiver) restoredRiver.transform.localScale = Vector3.one;
        if (chokedRiver) chokedRiver.transform.localScale = Vector3.one;

        Debug.Log($"[RiverStateManager] Initial river state active object: {(activeState != null ? activeState.name : "None")}");
    }

    private IEnumerator CrossFadeRiverRoutine(GameObject fromState, GameObject toState)
    {
        Debug.Log($"[RiverStateManager] CrossFade River Coroutine Started: {(fromState != null ? fromState.name : "None")} -> {toState.name}");

        toState.SetActive(true);
        toState.transform.localScale = Vector3.zero;

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / transitionDuration);

            if (fromState != null) fromState.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            toState.transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);

            yield return null;
        }

        if (fromState != null)
        {
            fromState.SetActive(false);
            fromState.transform.localScale = Vector3.one;
        }

        toState.transform.localScale = Vector3.one;
        currentState = toState;

        Debug.Log($"[RiverStateManager] CrossFade River Completed! Current active river object: {currentState.name}");
    }
}