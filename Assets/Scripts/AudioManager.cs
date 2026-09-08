using UnityEngine;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    [Header("Sound Clips")]
    public AudioClip heavyRainSound;
    public AudioClip lightWindSound;
    public AudioClip riverSound;
    public AudioClip forestSound;     // The looping forest ambience
    public AudioClip chopwoodSound;
    public AudioClip initialbackgroundSound; // The looping BGM
    public AudioClip degradedForestSound;

    private AudioSource loopSource;  // For looping BGM and Forest
    private AudioSource sfxSource;   // For one-shot sounds like wood chopping

    private Dictionary<string, AudioClip> soundLibrary;

    void Start()
    {
        loopSource = gameObject.AddComponent<AudioSource>();
        loopSource.loop = true; 
        
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.loop = false; 

        soundLibrary = new Dictionary<string, AudioClip>
        {
            { "heavy_rain_erosion", heavyRainSound },
            { "light_acoustic_wind", lightWindSound },
            { "river_flow", riverSound },
            { "forest_sound", forestSound },       // Replaced birdsong
            { "chopwood", chopwoodSound },
            { "initial_background", initialbackgroundSound },
            { "degraded_forest", degradedForestSound }
        };
    }

    public void PlaySound(string soundName)
    {
        if (sfxSource == null || loopSource == null) 
        {
            Debug.LogError("[AudioManager] Audio Source components missing!");
            return;
        }

        if (soundLibrary != null && soundLibrary.ContainsKey(soundName))
        {
            // Looping Backgrounds
            if (soundName == "initial_background" || soundName == "degraded_forest" || soundName == "forest_sound")
            {
                loopSource.clip = soundLibrary[soundName];
                loopSource.Play();
                Debug.Log($"[AudioManager] Playing looping sound: {soundName}");
            }
            // One-shot Effects
            else
            {
                sfxSource.PlayOneShot(soundLibrary[soundName]);
                Debug.Log($"[AudioManager] Playing one-shot sound: {soundName}");
            }
        }
        else
        {
            Debug.LogWarning($"[AudioManager] No sound found with the name: '{soundName}'. Make sure it's in the dictionary!");
        }
    }
}