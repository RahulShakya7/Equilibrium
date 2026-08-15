using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceBarUI : MonoBehaviour
{
    [Header("Individual Resource Sliders")]
    public Slider forestSlider;
    public Slider riverSlider;
    public Slider soilSlider;
    public TMP_Text forestValueText;
    public TMP_Text riverValueText;
    public TMP_Text soilValueText;

    [Header("Equilibrium Slider")]
    public Slider equilibriumSlider;
    public TMP_Text equilibriumLabel;      // "Balanced" / "Imbalanced"
    public TMP_Text equilibriumValueText;  // optional percentage

    [Header("Settings")]
    public float maxValue = 100f;
    [Tooltip("Average above this threshold is considered balanced.")]
    public float balanceThreshold = 70f;

    private InkDialogueRunner dialogueRunner;

    void Start()
    {
        dialogueRunner = FindFirstObjectByType<InkDialogueRunner>();
        if (dialogueRunner != null)
        {
            dialogueRunner.OnVariableChanged += UpdateBar;
        }

        // Initialize bars with starting values
        UpdateBar("forest_stock", 100);
        UpdateBar("river_clarity", 100);
        UpdateBar("soil_fertility", 100);
    }

    void OnDestroy()
    {
        if (dialogueRunner != null)
            dialogueRunner.OnVariableChanged -= UpdateBar;
    }

    void UpdateBar(string varName, object value)
    {
        // Update individual sliders
        switch (varName)
        {
            case "forest_stock":
                SetSlider(forestSlider, forestValueText, (int)value);
                break;
            case "river_clarity":
                SetSlider(riverSlider, riverValueText, (int)value);
                break;
            case "soil_fertility":
                SetSlider(soilSlider, soilValueText, (int)value);
                break;
        }

        // Recalculate equilibrium after any change
        UpdateEquilibrium();
    }

    void SetSlider(Slider slider, TMP_Text label, int value)
    {
        if (slider != null)
        {
            // Normalize value to slider's range (0 to slider.maxValue)
            slider.value = Mathf.Clamp((float)value / maxValue * slider.maxValue, slider.minValue, slider.maxValue);
        }
        if (label != null)
            label.text = value.ToString();
    }

    void UpdateEquilibrium()
    {
        int forest = GetVariableValue("forest_stock");
        int river = GetVariableValue("river_clarity");
        int soil = GetVariableValue("soil_fertility");

        float average = (forest + river + soil) / 3f;
        float equilibriumPercent = Mathf.Clamp01(average / maxValue);

        // Update equilibrium slider
        if (equilibriumSlider != null)
            equilibriumSlider.value = equilibriumPercent * equilibriumSlider.maxValue;

        // Update value text
        if (equilibriumValueText != null)
            equilibriumValueText.text = Mathf.RoundToInt(equilibriumPercent * 100).ToString() + "%";

        // Update label
        if (equilibriumLabel != null)
        {
            bool balanced = average >= balanceThreshold;
            equilibriumLabel.text = balanced ? "Balanced" : "Imbalanced";
            equilibriumLabel.color = balanced ? Color.green : Color.red;
        }
    }

    int GetVariableValue(string varName)
    {
        if (dialogueRunner == null) return 100;
        return dialogueRunner.GetVariableValue(varName);
    }
}