using UnityEngine;

public class TriggerZone : MonoBehaviour
{
    [SerializeField] BoulderController boulderController; // Reference to the boulder
    [SerializeField] Vector3 pushDirection; // Direction set here for flexibility
    [SerializeField] float pushForce; // Force magnitude

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            // Call a method on the boulder to push it
            if (boulderController != null)
            {
                boulderController.SetPushDirection(pushDirection);
                boulderController.LaunchInstantly(20f);
            }
        }
    }
}




