using System.Collections;
using UnityEngine;

public class ForestStateManager : MonoBehaviour
{
    [Header("Forest State Parents")]
    [SerializeField] private GameObject healthyTrees;   // State 0
    [SerializeField] private GameObject choppedTrees;   // State 1
    [SerializeField] private GameObject clearedForest;  // State 2

    [Header("Transition Settings")]
    [SerializeField] private float transitionDuration = 2.0f;

    private GameObject currentState;

    private void Start()
    {
        // Inspector Reference Validation
        if (healthyTrees == null) Debug.LogError("[ForestStateManager] Healthy Trees reference is missing in the Inspector!");
        if (choppedTrees == null) Debug.LogError("[ForestStateManager] Chopped Trees reference is missing in the Inspector!");
        if (clearedForest == null) Debug.LogError("[ForestStateManager] Cleared Forest reference is missing in the Inspector!");

        Debug.Log("[ForestStateManager] Script initialized successfully.");
        SetInitialState(healthyTrees);
    }

    public void TransitionToState(int stateIndex)
    {
        Debug.Log($"[ForestStateManager] TransitionToState called with index: {stateIndex}");

        GameObject targetState = stateIndex switch
        {
            1 => choppedTrees,
            2 => clearedForest,
            _ => healthyTrees
        };

        if (targetState == null)
        {
            Debug.LogError($"[ForestStateManager] Target GameObject for state index {stateIndex} is NULL! Ensure slots are assigned in the Inspector.");
            return;
        }

        if (targetState == currentState)
        {
            Debug.LogWarning($"[ForestStateManager] Already in state '{targetState.name}'. Transition skipped.");
            return;
        }

        Debug.Log($"[ForestStateManager] Starting transition from '{(currentState != null ? currentState.name : "None")}' to '{targetState.name}' over {transitionDuration}s.");
        StopAllCoroutines();
        StartCoroutine(CrossFadeForestRoutine(currentState, targetState));
    }

    private void SetInitialState(GameObject activeState)
    {
        currentState = activeState;

        if (healthyTrees) healthyTrees.SetActive(healthyTrees == activeState);
        if (choppedTrees) choppedTrees.SetActive(choppedTrees == activeState);
        if (clearedForest) clearedForest.SetActive(clearedForest == activeState);

        if (healthyTrees) healthyTrees.transform.localScale = Vector3.one;
        if (choppedTrees) choppedTrees.transform.localScale = Vector3.one;
        if (clearedForest) clearedForest.transform.localScale = Vector3.one;

        Debug.Log($"[ForestStateManager] Initial forest state active object: {(activeState != null ? activeState.name : "None")}");
    }

    private IEnumerator CrossFadeForestRoutine(GameObject fromState, GameObject toState)
    {
        Debug.Log($"[ForestStateManager] CrossFade Coroutine Started: {(fromState != null ? fromState.name : "None")} -> {toState.name}");

        toState.SetActive(true);
        toState.transform.localScale = Vector3.zero;

        float elapsed = 0f;

        while (elapsed < transitionDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / transitionDuration);

            if (fromState != null)
            {
                fromState.transform.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, t);
            }

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

        Debug.Log($"[ForestStateManager] CrossFade Completed! Current active forest object: {currentState.name}");
    }
}