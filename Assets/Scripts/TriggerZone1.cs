using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    public string knotName = "decision_point_1";

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            InkDialogueRunner runner = FindObjectOfType<InkDialogueRunner>();
            if (runner != null)
                runner.GoToKnot(knotName);
        }
    }
}