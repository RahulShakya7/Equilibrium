using UnityEngine;

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
        SetForestState(true);
        SetRiverState(true);
        SetFinalState("none");

        dialogueRunner = UnityEngine.Object.FindFirstObjectByType<InkDialogueRunner>();
        if (dialogueRunner != null)
            dialogueRunner.OnVariableChanged += HandleVariableChange;
    }

    void OnDestroy()
    {
        if (dialogueRunner != null)
            dialogueRunner.OnVariableChanged -= HandleVariableChange;
    }

    public void HandleTag(string tag)
    {
        if (tag.StartsWith("env_state:"))
        {
            if (int.TryParse(tag.Substring(10).Trim(), out int stateIndex))
            {
                ApplyEnvironmentState(stateIndex);
            }
        }
        else if (tag.StartsWith("environment "))
        {
            string action = tag.Substring(12).Trim();
            ProcessEnvironmentTag(action);
        }
        else if (tag.StartsWith("outcome "))
        {
            string outcome = tag.Substring(8).Trim();
            SetFinalState(outcome);
        }
    }

    private void ApplyEnvironmentState(int stateIndex)
    {
        bool isHealthy = (stateIndex <= 1);

        SetForestState(isHealthy);
        PlaySound(isHealthy ? birdsong : chainsaw);

        var volumeMgr = UnityEngine.Object.FindFirstObjectByType<EnvironmentVolumeManager>();
        if (volumeMgr != null)
        {
            volumeMgr.SetEnvironmentalState(stateIndex);
        }
    }

    private void HandleVariableChange(string varName, object value)
    {
        if (value is int intValue)
        {
            switch (varName)
            {
                case "forest_stock":
                    SetForestState(intValue >= 50);
                    break;
                case "river_clarity":
                    SetRiverState(intValue >= 80);
                    break;
            }
        }
    }

    private void ProcessEnvironmentTag(string action)
    {
        switch (action)
        {
            case "silviculture":
                ApplyEnvironmentState(1);
                break;
            case "clearcut":
                ApplyEnvironmentState(2);
                break;
            case "water_recovering":
                SetRiverState(true);
                PlaySound(waterFlow);
                break;
            case "water_choked":
                SetRiverState(false);
                PlaySound(concretePound);
                break;
        }
    }

    public void SetForestState(bool forestHealthy)
    {
        SetActiveAll(healthyForestObjects, forestHealthy);
        SetActiveAll(clearedForestObjects, !forestHealthy);
        SetActiveAll(stumps, !forestHealthy);
    }

    public void SetRiverState(bool riverClear)
    {
        SetActiveAll(clearRiverObjects, riverClear);
        SetActiveAll(chokedRiverObjects, !riverClear);
        SetActiveAll(wallObjects, !riverClear);
    }

    public void SetFinalState(string outcome)
    {
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