using UnityEngine;
using Ink.Runtime;
using System;
using System.Collections.Generic;

public class InkDialogueRunner : MonoBehaviour
{
    [Header("Ink Story")]
    [SerializeField] private TextAsset inkJSON;

    public event Action<string, List<string>> OnLineReady;
    public event Action<List<Choice>> OnChoicesReady;

    private Story story;
    private bool storyEnded = false;

    void Start()
    {
        if (inkJSON == null)
        {
            Debug.LogError("InkDialogueRunner: Ink JSON is not assigned.");
            return;
        }
        story = new Story(inkJSON.text);
        TryAdvance();
    }

    public void ContinueStory()
    {
        if (storyEnded) return;
        // If there are choices showing, ignore click – player must choose
        if (story.currentChoices.Count > 0) return;
        TryAdvance();
    }

    public void MakeChoice(int choiceIndex)
    {
        if (story.currentChoices.Count > choiceIndex)
        {
            story.ChooseChoiceIndex(choiceIndex);
            TryAdvance();
        }
    }

    private void TryAdvance()
    {
        string line = "";
        List<string> tags = new List<string>();

        if (story.canContinue)
        {
            line = story.Continue();
            tags = story.currentTags;
            Debug.Log("[INK] Line: " + line);                          // DEBUG
        }
        else
        {
            Debug.Log("[INK] No more content can continue.");          // DEBUG
        }

        List<Choice> choices = null;
        if (story.currentChoices.Count > 0)
        {
            choices = story.currentChoices;
            Debug.Log("[INK] Choices found: " + choices.Count);        // DEBUG
            foreach (var c in choices)
                Debug.Log("   -> " + c.text);                          // DEBUG
        }
        else
        {
            Debug.Log("[INK] No choices right now.");                  // DEBUG
        }

        if (!story.canContinue && choices == null)
        {
            storyEnded = true;
            Debug.Log("[INK] Story ended.");                            // DEBUG
        }

        OnLineReady?.Invoke(line.Trim(), tags);
        if (choices != null)
        {
            Debug.Log("[INK] Firing OnChoicesReady.");                 // DEBUG
            OnChoicesReady?.Invoke(choices);
        }
    }

    public void GoToKnot(string knotName)
    {
        story.ChoosePathString(knotName);
        storyEnded = false;
        TryAdvance();
    }
}