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
    [SerializeField] private Transform choiceContainer;     // Assign ChoiceObject
    [SerializeField] private GameObject choiceButtonPrefab; // Assign ChoiceButton prefab

    [Header("Speaker UI (Optional)")]
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private Image speakerPortrait;

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

        // If choices are waiting and player is in trigger zone, show buttons
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

        // If the typewriter is currently typing out a line, do NOT interrupt it.
        // Let it finish. OnTypewriterComplete() will automatically show these choices when done.
        if (typewriterCoroutine != null) 
        {
            return;
        }

        // Only stop coroutines and show choices instantly if no line is currently being typed
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

        // If knot name passed (e.g. "decision_point_1"), start that knot now!
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

        // Clear previous buttons
        foreach (Transform child in choiceContainer)
            Destroy(child.gameObject);

        // Instantiate new buttons
        for (int i = 0; i < choices.Count; i++)
        {
            Choice choice = choices[i];
            Debug.Log("Spawning Button for Choice: " + choice.text);

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

    void HandleTags(List<string> tags)
    {   
        foreach (string tag in tags)
        {
            if (tag.StartsWith("speaker "))
            {
                string speaker = tag.Substring(8).Trim();
                if (speakerNameText != null)
                    speakerNameText.text = speaker;
            }

            if (EnvironmentManager.Instance != null)
            {
                EnvironmentManager.Instance.HandleTag(tag);
            }
        }
    }
}