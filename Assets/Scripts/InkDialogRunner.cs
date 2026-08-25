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
    public RainManager rainManager;
    void Start()
    {
        if (inkJSON == null)
        {
            Debug.LogError("InkDialogueRunner: Ink JSON is not assigned.");
            return;
        }
        // Check what mode the player chose in the main menu
        if (GameModeManager.CurrentMode == "Simulation")
        {
            // Do simulation-specific things (e.g., Free roam, no story dialogue, infinite resources)
            Debug.Log("Simulation Mode is running!");
        }
        else
        {
            // Do story-specific things (e.g., Show story dialogue, limit resources)
            Debug.Log("Story Mode is running!");
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
        story.BindExternalFunction("setRain", (float intensity) => {
                Debug.Log($"[DialogueController] Ink called setRain with intensity: {intensity}");
                
                if (rainManager != null)
                {
                    rainManager.SetSeasonalRain(intensity);
                }
                else
                {
                    Debug.LogError("[DialogueController] Rain Manager is NOT assigned in the Inspector! Please drag the RainSystem GameObject into the 'Rain Manager' slot.");
                }
            }
        );
    
        TryAdvance();
    }

    public void ContinueStory()
    {
        if (storyEnded) return;
        if (story.currentChoices.Count > 0) return; // wait for choice
        TryAdvance();
    }

    public int GetVariableValue(string varName)
    {
        if (story == null) return 0;
        object value = story.variablesState[varName];
        if (value == null) return 0;
        return (int)value;
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