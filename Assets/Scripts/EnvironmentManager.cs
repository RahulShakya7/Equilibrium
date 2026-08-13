using UnityEngine;
using System.Collections.Generic;

public class EnvironmentManager : MonoBehaviour
{
    public static EnvironmentManager Instance { get; private set; }

    [Header("Forest State")]
    public GameObject[] healthyForestObjects;   // trees, canopy
    public GameObject[] clearedForestObjects;   // stumps, fallen logs
    public GameObject[] stumps;                 // extra stumps

    [Header("River State")]
    public GameObject[] clearRiverObjects;      // blue water, plants
    public GameObject[] chokedRiverObjects;     // brown water, debris
    public GameObject[] wallObjects;            // concrete walls

    [Header("Final Outcome States")]
    public GameObject[] balanceState;
    public GameObject[] vulnerableState;
    public GameObject[] partialState;
    public GameObject[] collapseState;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip birdsong;
    public AudioClip chainsaw;
    public AudioClip waterFlow;
    public AudioClip concretePound;

    private InkDialogueRunner dialogueRunner;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        // Initial world state: healthy forest, clear river, no final state
        SetForestState(true);
        SetRiverState(true);
        SetFinalState("none");

        // Subscribe to variable changes
        dialogueRunner = FindFirstObjectByType<InkDialogueRunner>();
        if (dialogueRunner != null)
            dialogueRunner.OnVariableChanged += HandleVariableChange;
    }

    void OnDestroy()
    {
        if (dialogueRunner != null)
            dialogueRunner.OnVariableChanged -= HandleVariableChange;
    }

    // Called by DialogueUIController when a tag is parsed
    public void HandleTag(string tag)
    {
        if (tag.StartsWith("environment "))
        {
            string action = tag.Substring(12).Trim();
            ProcessEnvironmentTag(action);
        }
        else if (tag.StartsWith("outcome "))
        {
            string outcome = tag.Substring(8).Trim();
            SetFinalState(outcome);
        }
        // Speaker tags can be used for audio or ambience
        else if (tag.StartsWith("speaker "))
        {
            // Optional: switch ambient sound or portraits
        }
    }

    // Called when an Ink variable changes
    private void HandleVariableChange(string varName, object value)
    {
        int intValue = (int)value;

        switch (varName)
        {
            case "forest_stock":
                // If forest drops below 50, show cleared forest
                SetForestState(intValue >= 50);
                break;
            case "river_clarity":
                // If river clarity drops below 80, show choked river
                SetRiverState(intValue >= 80);
                break;
            case "soil_fertility":
                // Could trigger soil degradation visuals
                break;
        }
    }

    private void ProcessEnvironmentTag(string action)
    {
        switch (action)
        {
            case "silviculture":
                SetForestState(true);
                PlaySound(birdsong);
                break;
            case "clearcut":
                SetForestState(false);
                PlaySound(chainsaw);
                break;
            case "water_recovering":
                SetRiverState(true);
                PlaySound(waterFlow);
                break;
            case "water_choked":
                SetRiverState(false);
                PlaySound(concretePound);
                break;
            default:
                Debug.LogWarning("Unknown environment tag: " + action);
                break;
        }
    }

    // forestHealthy = true shows healthy forest, false shows cleared
    public void SetForestState(bool forestHealthy)
    {
        SetActiveAll(healthyForestObjects, forestHealthy);
        SetActiveAll(clearedForestObjects, !forestHealthy);
        SetActiveAll(stumps, !forestHealthy);
    }

    // riverClear = true shows clear river, false shows choked/walled
    public void SetRiverState(bool riverClear)
    {
        SetActiveAll(clearRiverObjects, riverClear);
        SetActiveAll(chokedRiverObjects, !riverClear);
        SetActiveAll(wallObjects, !riverClear);
    }

    public void SetFinalState(string outcome)
    {
        // Disable all ending visuals first
        SetActiveAll(balanceState, false);
        SetActiveAll(vulnerableState, false);
        SetActiveAll(partialState, false);
        SetActiveAll(collapseState, false);

        switch (outcome)
        {
            case "balance": SetActiveAll(balanceState, true); break;
            case "vulnerable": SetActiveAll(vulnerableState, true); break;
            case "partial": SetActiveAll(partialState, true); break;
            case "collapse": SetActiveAll(collapseState, true); break;
        }
    }

    private void SetActiveAll(GameObject[] objects, bool active)
    {
        if (objects == null) return;
        foreach (var obj in objects)
            if (obj != null) obj.SetActive(active);
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}