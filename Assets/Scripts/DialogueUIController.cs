using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;

public class DialogueUIController : MonoBehaviour
{
    [Header("Auto Advance Settings")]
    [SerializeField] private float autoAdvanceDelay = 2.5f; 
    private Coroutine autoAdvanceCoroutine;

    [Header("Dependencies")]
    [SerializeField] private InkDialogueRunner dialogueRunner;
    [SerializeField] private TMP_Text lineText;
    [SerializeField] private Transform choiceContainer;     
    [SerializeField] private GameObject choiceButtonPrefab; 

    [Header("Speaker UI (Optional)")]
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private Image speakerPortrait;

    [Header("Environment Reference")]
    [SerializeField] private EnvironmentVolumeManager volumeManager;

    [Header("Typewriter Settings")]
    [SerializeField] private float charactersPerSecond = 30f;
    [SerializeField] private GameObject continueIndicator;

    // State
    private Coroutine typewriterCoroutine;
    private string currentFullLine;
    private List<Choice> pendingChoices;

    private List<string> lineHistory = new List<string>();
    private int historyIndex = -1;

    private bool isPlayerInDecisionZone = false;
    private bool isShowingChoices = false; 

    private void OnEnable()
    {
        if (dialogueRunner != null)
        {
            dialogueRunner.OnLineReady += HandleLine;
            dialogueRunner.OnChoicesReady += ReceiveChoices;
        }
    }

    private void OnDisable()
    {
        if (dialogueRunner != null)
        {
            dialogueRunner.OnLineReady -= HandleLine;
            dialogueRunner.OnChoicesReady -= ReceiveChoices;
        }
    }

    void Update()
    {
        if (isShowingChoices) return;

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            GoToPreviousLine();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            SkipOrAdvance();
        }
    }

    void HandleLine(string line, List<string> tags)
    {
        string tagDebug = (tags != null && tags.Count > 0) ? string.Join(", ", tags) : "NONE";
        Debug.Log($"[DialogueUI] Line: '{line}' | Tags Found ({tags?.Count ?? 0}): [{tagDebug}]");

        HandleTags(tags);
        currentFullLine = line;

        lineHistory.Add(line);
        historyIndex = lineHistory.Count - 1;

        StopActiveCoroutines();

        typewriterCoroutine = StartCoroutine(TypeLine(currentFullLine));
    }

    IEnumerator TypeLine(string textToType)
    {
        lineText.text = "";
        foreach (char c in textToType)
        {
            lineText.text += c;
            yield return new WaitForSeconds(1f / charactersPerSecond);
        }
        typewriterCoroutine = null;
        OnTypewriterComplete();
    }

    void OnTypewriterComplete()
    {
        if (isShowingChoices) return;

        if (pendingChoices != null)
        {
            if (continueIndicator != null)
                continueIndicator.SetActive(false);

            if (isPlayerInDecisionZone)
            {
                ShowChoicesNow(pendingChoices);
                pendingChoices = null;
            }
            return;
        }

        if (historyIndex < lineHistory.Count - 1)
        {
            if (continueIndicator != null)
                continueIndicator.SetActive(true);
            return;
        }

        if (continueIndicator != null)
            continueIndicator.SetActive(true);

        autoAdvanceCoroutine = StartCoroutine(AutoAdvance());
    }

    IEnumerator AutoAdvance()
    {
        yield return new WaitForSeconds(autoAdvanceDelay);
        autoAdvanceCoroutine = null;
        Advance();
    }

    public void SkipOrAdvance()
    {
        if (isShowingChoices) return;

        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
            lineText.text = lineHistory[historyIndex];
            typewriterCoroutine = null;
            OnTypewriterComplete();
            return;
        }

        if (historyIndex < lineHistory.Count - 1)
        {
            historyIndex++;
            DisplayHistoryLine();
            return;
        }

        Advance();
    }

    public void GoToPreviousLine()
    {
        if (isShowingChoices || lineHistory.Count == 0) return;

        if (historyIndex > 0)
        {
            historyIndex--;
            DisplayHistoryLine();
        }
    }

    private void DisplayHistoryLine()
    {
        StopActiveCoroutines();
        lineText.text = lineHistory[historyIndex];
        if (continueIndicator != null)
            continueIndicator.SetActive(true);
    }

    private void StopActiveCoroutines()
    {
        if (continueIndicator != null)
            continueIndicator.SetActive(false);

        if (autoAdvanceCoroutine != null)
        {
            StopCoroutine(autoAdvanceCoroutine);
            autoAdvanceCoroutine = null;
        }
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
            typewriterCoroutine = null;
        }
    }

    void ReceiveChoices(List<Choice> choices)
    {
        pendingChoices = choices;

        if (typewriterCoroutine != null) 
        {
            return;
        }

        StopActiveCoroutines();

        if (isPlayerInDecisionZone)
        {
            ShowChoicesNow(pendingChoices);
            pendingChoices = null;
        }
    }

    public void SetPlayerInDecisionZone(bool inZone, string optionalKnotName = "")
    {
        isPlayerInDecisionZone = inZone;

        if (!inZone) return;

        StopActiveCoroutines();

        if (!string.IsNullOrEmpty(optionalKnotName) && dialogueRunner != null)
        {
            Debug.Log("Starting Knot via Trigger: " + optionalKnotName);
            dialogueRunner.GoToKnot(optionalKnotName);
            return;
        }

        if (pendingChoices != null)
        {
            ShowChoicesNow(pendingChoices);
            pendingChoices = null;
        }
    }

    void ShowChoicesNow(List<Choice> choices)
    {
        StopActiveCoroutines();

        isShowingChoices = true;
        if (continueIndicator != null)
            continueIndicator.SetActive(false);

        if (choiceContainer == null || choiceButtonPrefab == null)
        {
            Debug.LogError("DialogueUIController: Choice Container or Choice Button Prefab is missing in Inspector!");
            return;
        }

        foreach (Transform child in choiceContainer)
            Destroy(child.gameObject);

        for (int i = 0; i < choices.Count; i++)
        {
            Choice choice = choices[i];

            GameObject buttonObj = Instantiate(choiceButtonPrefab, choiceContainer);

            TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
            if (buttonText != null)
                buttonText.text = choice.text.Trim();

            int index = i;
            Button button = buttonObj.GetComponent<Button>();
            if (button != null)
            {
                button.onClick.AddListener(() => SelectChoice(index));
            }
        }
    }

    private void SelectChoice(int choiceIndex)
    {
        isShowingChoices = false;
        isPlayerInDecisionZone = false;

        foreach (Transform child in choiceContainer)
            Destroy(child.gameObject);

        if (dialogueRunner != null)
            dialogueRunner.MakeChoice(choiceIndex);
    }

    void Advance()
    {
        StopActiveCoroutines();
        dialogueRunner.ContinueStory();
    }
    private void HandleTags(List<string> tags)
    {
        if (tags == null || tags.Count == 0)
        {
            Debug.Log("[DialogueUIController] HandleTags received an empty or null tag list.");
            return;
        }

        Debug.Log($"[DialogueUIController] Processing {tags.Count} tag(s)...");

        foreach (string tag in tags)
        {
            Debug.Log($"[DialogueUIController] Parsing raw tag: '{tag}'");

            string key = tag.Split(':')[0].Trim();
            string value = tag.Contains(":") ? tag.Split(':')[1].Trim() : "";

            // --- Forest & Post-Processing State ---
            if (key == "env_state" && int.TryParse(value, out int stateIndex))
            {
                Debug.Log($"[DialogueUIController] Parsed 'env_state' tag with index: {stateIndex}");

                // 1. Update Post-Processing Lighting
                var volumeMgr = UnityEngine.Object.FindFirstObjectByType<EnvironmentVolumeManager>();
                if (volumeMgr != null) 
                {
                    Debug.Log($"[DialogueUIController] Found EnvironmentVolumeManager in scene. Executing state {stateIndex}.");
                    volumeMgr.SetEnvironmentalState(stateIndex);
                }
                else
                {
                    Debug.LogWarning("[DialogueUIController] Failed to find EnvironmentVolumeManager in scene!");
                }

                // 2. Trigger Tree Mesh Transition
                var forestMgr = UnityEngine.Object.FindFirstObjectByType<ForestStateManager>();
                if (forestMgr != null) 
                {
                    Debug.Log($"[DialogueUIController] Found ForestStateManager in scene. Executing transition to state {stateIndex}.");
                    forestMgr.TransitionToState(stateIndex);
                }
                else
                {
                    Debug.LogError("[DialogueUIController] Failed to find ForestStateManager in scene! Ensure script is attached to an active GameObject.");
                }
            }

            // --- River Mesh & Material State ---
            if (key == "river_state" && int.TryParse(value, out int riverIndex))
            {
                Debug.Log($"[DialogueUIController] Parsed 'river_state' tag with index: {riverIndex}");

                var riverMgr = UnityEngine.Object.FindFirstObjectByType<RiverStateManager>();
                if (riverMgr != null)
                {
                    Debug.Log($"[DialogueUIController] Found RiverStateManager in scene. Executing transition to state {riverIndex}.");
                    riverMgr.TransitionToRiverState(riverIndex);
                }
                else
                {
                    Debug.LogError("[DialogueUIController] Failed to find RiverStateManager in scene! Ensure script is attached to an active GameObject.");
                }
            }
        }
    }
}