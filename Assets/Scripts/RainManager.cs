using UnityEngine;

public class RainManager : MonoBehaviour
{
    [SerializeField] private float transitionSpeed = 0.5f;
    private ParticleSystem ps;
    private float currentIntensity = 0f;
    private float targetIntensity = 0f;

    void Start()
    {
        Debug.Log("[RainManager] Initializing...");
        ps = GetComponent<ParticleSystem>();
        
        if (ps == null) 
        {
            Debug.LogError("[RainManager] No Particle System attached to this GameObject!");
            return;
        }

        // Start with rain off
        gameObject.SetActive(false);
        currentIntensity = 0f;
        targetIntensity = 0f;
        Debug.Log("[RainManager] Initialized. Rain is currently off.");
    }

    void Update()
    {
        // Only log when intensity changes to avoid spamming console
        if (Mathf.Abs(currentIntensity - targetIntensity) > 0.01f)
        {
            float previousIntensity = currentIntensity;
            currentIntensity = Mathf.Lerp(currentIntensity, targetIntensity, Time.deltaTime * transitionSpeed);
            
            var emission = ps.emission;
            emission.rateOverTime = currentIntensity * 1000f; // Adjust this number for rain density

            if (Mathf.Abs(currentIntensity - previousIntensity) > 0.05f)
            {
                Debug.Log($"[RainManager] Transitioning Rain Intensity: {previousIntensity:F2} -> {currentIntensity:F2}");
            }
        }
    }

    // Call this from Ink
    public void SetSeasonalRain(float intensity)
    {
        Debug.Log($"[RainManager] SetSeasonalRain called from Ink with intensity: {intensity}");

        if (intensity > 0 && !gameObject.activeSelf)
        {
            gameObject.SetActive(true);
            Debug.Log("[RainManager] Rain GameObject activated.");
        }
        
        targetIntensity = intensity;
        
        if (intensity <= 0) 
        {
            // Fades out then stops
            Debug.Log("[RainManager] Rain target set to 0. Fading out...");
            Invoke(nameof(DeactivateRain), 3f); 
        }
        else
        {
            CancelInvoke(nameof(DeactivateRain)); // Cancel fade out if heavy rain called again
        }
    }
    
    private void DeactivateRain()
    {
        if (targetIntensity <= 0)
        {
            gameObject.SetActive(false);
            Debug.Log("[RainManager] Rain deactivated completely.");
        }
    }
}