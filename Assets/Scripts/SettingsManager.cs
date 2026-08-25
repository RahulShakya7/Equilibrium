using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsManager : MonoBehaviour
{
    public Slider volumeSlider;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown fullscreenDropdown;

    private Resolution[] resolutions;

    void Start()
    {
        // Initialize Audio
        volumeSlider.value = AudioListener.volume;
        volumeSlider.onValueChanged.AddListener(SetVolume);

        // Initialize Resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        var options = new System.Collections.Generic.List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                currentResolutionIndex = i;
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        // Initialize Fullscreen
        fullscreenDropdown.value = Screen.fullScreen ? 1 : 0;
        fullscreenDropdown.RefreshShownValue();
        fullscreenDropdown.onValueChanged.AddListener(SetFullscreen);
    }

    public void SetVolume(float volume) { AudioListener.volume = volume; }

    public void SetResolution(int index) 
    { 
        Resolution r = resolutions[index];
        Screen.SetResolution(r.width, r.height, Screen.fullScreen);
    }

    public void SetFullscreen(int index) 
    { 
        Screen.fullScreen = (index == 1);
    }
}