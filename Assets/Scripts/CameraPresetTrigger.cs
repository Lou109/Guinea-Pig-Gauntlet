using UnityEngine;

public class CameraPresetTrigger : MonoBehaviour
{
    [SerializeField] private int presetIndex = 0; // Preset to switch to
    [SerializeField] private bool enableFacePlayer = true; // Toggle in inspector

    private bool isTriggered = false;
    private PlayerFaceCamera faceScript; // Cached reference

    private void Start()
    {
        faceScript = GameObject.FindFirstObjectByType<PlayerFaceCamera>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isTriggered)
        {
            // Switch the camera preset
            var cameraScript = Camera.main.GetComponent<PlayerFollowCamera>();
            if (cameraScript != null)
            {
                cameraScript.SwitchToPreset(presetIndex);
            }

            // Make player face the camera if enabled
            if (enableFacePlayer && faceScript != null)
            {
                faceScript.StartFacingCamera();
            }

            isTriggered = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isTriggered)
        {
            // Revert camera adjustment
            var cameraScript = Camera.main.GetComponent<PlayerFollowCamera>();
            if (cameraScript != null)
            {
                cameraScript.ClearAdjustment();
            }

            // Stop the player from facing the camera if enabled
            if (enableFacePlayer && faceScript != null)
            {
                faceScript.StopFacingCamera();
            }

            // Optional: Uncomment if you want to reset trigger for reuse
            // isTriggered = false;
        }
    }
}
