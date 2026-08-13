using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResourceBarUI : MonoBehaviour
{
    [Header("UI References")]
    public Image forestBar;      // fill image for forest stock
    public Image riverBar;       // fill image for river clarity
    public Image soilBar;        // fill image for soil fertility
    public TMP_Text forestValueText;
    public TMP_Text riverValueText;
    public TMP_Text soilValueText;

    [Header("Settings")]
    public float maxValue = 100f;

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
        float amount = Mathf.Clamp01((float)(int)value / maxValue);

        switch (varName)
        {
            case "forest_stock":
                if (forestBar != null) forestBar.fillAmount = amount;
                if (forestValueText != null) forestValueText.text = ((int)value).ToString();
                break;
            case "river_clarity":
                if (riverBar != null) riverBar.fillAmount = amount;
                if (riverValueText != null) riverValueText.text = ((int)value).ToString();
                break;
            case "soil_fertility":
                if (soilBar != null) soilBar.fillAmount = amount;
                if (soilValueText != null) soilValueText.text = ((int)value).ToString();
                break;
        }
    }
}