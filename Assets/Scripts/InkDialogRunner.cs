using UnityEngine;
using Ink.Runtime;
using System;
using System.Collections.Generic;

public class InkDialogueRunner : MonoBehaviour
{
    [Header("Ink Story")]
    [SerializeField] private TextAsset inkJSON;

    // Events for UI and game systems
    public event Action<string, List<string>> OnLineReady;
    public event Action<List<Choice>> OnChoicesReady;
    public event Action<string, object> OnVariableChanged;

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

        // Register observers for the resource variables
        story.ObserveVariable("forest_stock", (varName, newValue) => {
            OnVariableChanged?.Invoke(varName, newValue);
        });
        story.ObserveVariable("river_clarity", (varName, newValue) => {
            OnVariableChanged?.Invoke(varName, newValue);
        });
        story.ObserveVariable("soil_fertility", (varName, newValue) => {
            OnVariableChanged?.Invoke(varName, newValue);
        });

        TryAdvance();
    }

    public void ContinueStory()
    {
        if (storyEnded) return;
        if (story.currentChoices.Count > 0) return; // wait for choice
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

    public void GoToKnot(string knotName)
    {
        if (story == null) return;
        story.ChoosePathString(knotName);
        storyEnded = false;
        TryAdvance();
    }

    private void TryAdvance()
    {
        string line = "";
        List<string> tags = new List<string>();

        if (story.canContinue)
        {
            line = story.Continue();
            tags = story.currentTags;
        }

        // After Continue, variables may have changed; observers fire automatically.

        List<Choice> choices = null;
        if (story.currentChoices.Count > 0)
        {
            choices = story.currentChoices;
        }

        if (!story.canContinue && choices == null)
        {
            storyEnded = true;
        }

        OnLineReady?.Invoke(line.Trim(), tags);

        if (choices != null)
        {
            OnChoicesReady?.Invoke(choices);
        }
    }
}