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
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Ground"))
                {
                    agent.SetDestination(hit.point);

                    if (clickEffectPrefab != null)
                    {
                        GameObject effect = Instantiate(clickEffectPrefab, hit.point + Vector3.up * 0.1f, Quaternion.identity);
                        Destroy(effect, 1.5f);
                    }
                }
            }
        }
    }
}