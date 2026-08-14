using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class ClickToMove : MonoBehaviour
{
    public GameObject clickEffectPrefab;    // optional particle effect
    public float stoppingDistance = 0.5f;

    private NavMeshAgent agent;
    private Camera mainCamera;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.stoppingDistance = stoppingDistance;
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Mouse clicked. Over UI? " + EventSystem.current.IsPointerOverGameObject());

            if (EventSystem.current.IsPointerOverGameObject())
                return;   // clicked on UI, ignore

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.Log("Ray origin: " + ray.origin + " direction: " + ray.direction);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Hit object: " + hit.collider.name + " tag: " + hit.collider.tag + " point: " + hit.point);

                if (hit.collider.CompareTag("Ground"))
                {
                    Debug.Log("Ground clicked, setting destination.");
                    agent.SetDestination(hit.point);
                }
                else
                {
                    Debug.LogWarning("Clicked object is not tagged Ground. Check the tag!");
                }
            }
            else
            {
                Debug.LogWarning("Raycast hit nothing. Is there a collider on the ground?");
            }
        }
    }
}