using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class EnvironmentVolumeManager : MonoBehaviour
{
    [Header("Volume Reference")]
    public Volume globalVolume;

    [Header("Debug Readout")]
    [SerializeField] private int currentState = -1;

    private ColorAdjustments colorAdjustments;
    private WhiteBalance whiteBalance;

    void Start()
    {
        if (globalVolume != null && globalVolume.profile != null)
        {
            if (!globalVolume.profile.TryGet(out colorAdjustments))
            {
                Debug.LogError("[VolumeManager] ColorAdjustments missing from Global Volume Profile! Add it under 'Add Override -> Post Processing'.");
            }
            if (!globalVolume.profile.TryGet(out whiteBalance))
            {
                Debug.LogWarning("[VolumeManager] WhiteBalance missing from Global Volume Profile!");
            }
        }
        else
        {
            Debug.LogError("[VolumeManager] Global Volume or Profile is NOT assigned on Manager object!");
        }

        // Reset to state 0 on launch
        SetEnvironmentalState(0);
    }

    public void SetEnvironmentalState(int decisionState)
    {
        currentState = decisionState;
        Debug.Log($"[VolumeManager] SetEnvironmentalState executing for State {decisionState}");

        if (colorAdjustments == null)
        {
            Debug.LogError("[VolumeManager] Cannot apply color shift: ColorAdjustments component is missing!");
            return;
        }

        // FORCE URP to override the settings
        colorAdjustments.colorFilter.overrideState = true;
        if (whiteBalance != null) whiteBalance.temperature.overrideState = true;

        switch (decisionState)
        {
            case 0: // Healthy Baseline
                colorAdjustments.colorFilter.value = Color.white;
                if (whiteBalance != null) whiteBalance.temperature.value = 0f;
                Debug.Log("[VolumeManager] Global Volume set to HEALTHY (Normal Lighting)");
                break;

            case 1: // Moderate Harvest
                colorAdjustments.colorFilter.value = new Color(0.98f, 0.88f, 0.55f); // Distinct Warm Yellow
                if (whiteBalance != null) whiteBalance.temperature.value = 30f;
                Debug.Log("[VolumeManager] Global Volume set to MODERATE (Warm Amber Tint)");
                break;

            case 2: // Severe Clearcut
                colorAdjustments.colorFilter.value = new Color(0.85f, 0.55f, 0.25f); // Strong Dry Orange/Brown
                if (whiteBalance != null) whiteBalance.temperature.value = 75f;
                Debug.Log("[VolumeManager] Global Volume set to SEVERE (Dry Desert Tint)");
                break;

            default:
                Debug.LogWarning($"[VolumeManager] Unhandled state index: {decisionState}");
                break;
        }
    }
}