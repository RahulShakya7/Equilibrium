using UnityEngine;
using UnityEngine.AI;

public class SnapToTerrain : MonoBehaviour
{
    private NavMeshAgent agent;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        if (agent != null && agent.isOnNavMesh)
        {
            // Keep the player's X and Z, but force Y to match the NavMesh surface
            Vector3 newPos = transform.position;
            newPos.y = agent.nextPosition.y;
            transform.position = newPos;
        }
    }
}