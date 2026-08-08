using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using System.Collections;
using System.Collections.Generic;

public class DialogueUIController : MonoBehaviour
{
    [Header("Core References")]
    [SerializeField] private InkDialogueRunner dialogueRunner;
    [SerializeField] private TMP_Text lineText;
    [SerializeField] private Transform choiceContainer;
    [SerializeField] private GameObject choiceButtonPrefab;

    [Header("Speaker UI (Optional)")]
    [SerializeField] private TMP_Text speakerNameText;
    [SerializeField] private Image speakerPortrait;

    [Header("Typewriter Settings")]
    [SerializeField] private float charactersPerSecond = 30f;
    [SerializeField] private GameObject continueIndicator;   // ▼ or "Click to continue"

    private Coroutine typewriterCoroutine;
    private string currentFullLine;
    private bool waitingForClick = false;
    
    private List<Choice> pendingChoices;    // choices that appear after the line finishes

    void ClearChoices()
    {
        foreach (Transform child in choiceContainer)
            Destroy(child.gameObject);
    }
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
        // Skip typewriter with click or space
        if (typewriterCoroutine != null && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            SkipTypewriter();
        }

        // Advance to next line when waiting for click and no choices are pending
        if (waitingForClick && pendingChoices == null && (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space)))
        {
            Advance();
        }
    }

    // ----------------------------------------------------------------
    //  Line received from InkRunner
    // ----------------------------------------------------------------
    void HandleLine(string line, List<string> tags)
    {
        ClearChoices();        
        Debug.Log("[UI] HandleLine: " + line);   // DEBUG
        HandleTags(tags);
        currentFullLine = line;

        if (typewriterCoroutine != null)
            StopCoroutine(typewriterCoroutine);

        typewriterCoroutine = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        lineText.text = "";
        foreach (char c in currentFullLine)
        {
            lineText.text += c;
            yield return new WaitForSeconds(1f / charactersPerSecond);
        }
        typewriterCoroutine = null;
        OnTypewriterComplete();
    }

    void OnTypewriterComplete()
    {
        Debug.Log("[UI] Typewriter complete. pendingChoices=" + (pendingChoices != null));

        if (pendingChoices != null)
        {
            ShowChoicesNow(pendingChoices);
            pendingChoices = null;
        }
        else
        {
            waitingForClick = true;
            if (continueIndicator != null)
                continueIndicator.SetActive(true);
        }
    }

    public void SkipTypewriter()
    {
        if (typewriterCoroutine != null)
        {
            StopCoroutine(typewriterCoroutine);
            lineText.text = currentFullLine;
            typewriterCoroutine = null;
            OnTypewriterComplete();
        }
    }

    // ----------------------------------------------------------------
    //  Choices received from InkRunner
    // ----------------------------------------------------------------
    void ReceiveChoices(List<Choice> choices)
    {
        Debug.Log("[UI] ReceiveChoices called with " + choices.Count + " choices."); // DEBUG
        if (typewriterCoroutine != null)
        {
            // Still typing; store choices to show later
            pendingChoices = choices;
        }
        else
        {
            ShowChoicesNow(choices);
        }
    }

    void ShowChoicesNow(List<Choice> choices)
{
    // Immediately hide the click-to-continue indicator
    waitingForClick = false;
    if (continueIndicator != null)
        continueIndicator.SetActive(false);

    // Destroy old buttons
    foreach (Transform child in choiceContainer)
        Destroy(child.gameObject);

    // Create new buttons
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
            button.onClick.AddListener(() => dialogueRunner.MakeChoice(index));
    }
}

    // ----------------------------------------------------------------
    //  Advance to next line (when no choices)
    // ----------------------------------------------------------------
    void Advance()
    {
        Debug.Log("[UI] Advance clicked"); // DEBUG
        waitingForClick = false;
        if (continueIndicator != null)
            continueIndicator.SetActive(false);
        dialogueRunner.ContinueStory();
    }

    // ----------------------------------------------------------------
    //  Tag handling
    // ----------------------------------------------------------------
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
        }
    }

    
}