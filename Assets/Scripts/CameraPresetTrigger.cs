using UnityEngine;

public class CameraPresetTrigger : MonoBehaviour
{
    [SerializeField] private int presetIndex = 0; // Preset to switch to
    private bool isTriggered = false;                // Flag to control if it's active

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            var cameraScript = Camera.main.GetComponent<PlayerFollowCamera>();
            if (cameraScript != null)
            {
                cameraScript.SwitchToPreset(presetIndex);
                isTriggered = true; // Prevent re-triggering
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isTriggered)
        {
            var cameraScript = Camera.main.GetComponent<PlayerFollowCamera>();
            if (cameraScript != null)
            {
                cameraScript.ClearAdjustment(); // Revert to default
            }
            // Optionally, if you want to allow re-trigger later:
            // isTriggered = false;
        }
    }

    // Call this method when you want to manually reset the trigger state
    public void ResetTrigger()
    {
        isTriggered = false;
    }
}
