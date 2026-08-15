using UnityEngine;

public class TutorialSpawner : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameObject tutorialMarkerPrefab;   // assign the prefab
    [SerializeField] private Transform spawnPosition;           // where to place the marker (e.g., ForestDecisionTrigger)
    [SerializeField] private float delay = 5f;                  // seconds before it appears

    [Header("Optional: Hide after first click")]
    [SerializeField] private bool hideAfterFirstClick = true;
    private bool hasClicked = false;
    private GameObject markerInstance;

    void Start()
    {
        Invoke(nameof(SpawnMarker), delay);
    }

    void SpawnMarker()
    {
        if (tutorialMarkerPrefab != null && spawnPosition != null)
        {
            markerInstance = Instantiate(tutorialMarkerPrefab, spawnPosition.position + Vector3.up * 2f, Quaternion.identity);
        }
    }

    void Update()
    {
        if (hideAfterFirstClick && !hasClicked && Input.GetMouseButtonDown(0))
        {
            hasClicked = true;
            if (markerInstance != null)
                Destroy(markerInstance);
        }
    }
}