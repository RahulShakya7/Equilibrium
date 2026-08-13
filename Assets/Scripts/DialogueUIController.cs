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
    [SerializeField] private GameObject continueIndicator;

    private Coroutine typewriterCoroutine;
    private string currentFullLine;
    private bool waitingForClick = false;
    private List<Choice> pendingChoices;

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

    void HandleLine(string line, List<string> tags)
    {
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

    void ReceiveChoices(List<Choice> choices)
    {
        if (typewriterCoroutine != null)
        {
            pendingChoices = choices;
        }
        else
        {
            ShowChoicesNow(choices);
        }
    }

    void ShowChoicesNow(List<Choice> choices)
    {
        waitingForClick = false;
        if (continueIndicator != null)
            continueIndicator.SetActive(false);

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
                button.onClick.AddListener(() => dialogueRunner.MakeChoice(index));
        }
    }

    void Advance()
    {
        waitingForClick = false;
        if (continueIndicator != null)
            continueIndicator.SetActive(false);
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

            // Forward the tag to the EnvironmentManager
            if (EnvironmentManager.Instance != null)
            {
                EnvironmentManager.Instance.HandleTag(tag);
            }
        }
    }
}