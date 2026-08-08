using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform targetPoint;
    [SerializeField] private float speed = 3f;
    [SerializeField] private float stoppingDistance = 1f;

    private bool reachedTarget = false;

    [System.Obsolete]
    void Update()
    {
        if (targetPoint == null || reachedTarget)
            return;

        // Move towards the target
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Face the direction of movement (optional)
        if (direction != Vector3.zero)
            transform.forward = direction;

        // Check if we've arrived
        if (Vector3.Distance(transform.position, targetPoint.position) <= stoppingDistance)
        {
            reachedTarget = true;
            TriggerDecisionPoint();
        }
    }

    [System.Obsolete]
    void TriggerDecisionPoint()
    {
        // Find the InkDialogueRunner and jump to the decision knot
        InkDialogueRunner runner = FindObjectOfType<InkDialogueRunner>();
        if (runner != null)
        {
            runner.GoToKnot("decision_point_1");
        }
        else
        {
            Debug.LogError("PlayerMovement: No InkDialogueRunner found!");
        }

        // Optionally stop the player completely
        this.enabled = false;
    }
}