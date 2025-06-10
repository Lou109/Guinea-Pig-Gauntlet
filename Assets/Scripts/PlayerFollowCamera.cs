using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class CameraPreset
{
    public Vector3 position;
    public Quaternion rotation;
}

public class PlayerFollowCamera : MonoBehaviour
{
    [SerializeField] Transform player; // Assign in inspector
    [SerializeField] private float followDistance = 5f;
    [SerializeField] private float followHeight = 2f;
    [SerializeField] private float followSpeed = 10f;

    [SerializeField]
    private List<CameraPreset> cameraPresets = new List<CameraPreset>();

    private Vector3 defaultPosition;
    private Quaternion defaultRotation;

    private bool isAdjusting = false;
    private Vector3 adjustmentTargetPosition;
    private Quaternion adjustmentTargetRotation;
    private float adjustmentTransitionSpeed = 2f;

    void Start()
    {
        defaultPosition = transform.position;
        defaultRotation = transform.rotation;
    }

    void LateUpdate()
    {
        if (player == null) return;

        if (isAdjusting)
        {
            // Using same logic but adding check and log
            Vector3 targetPosition = player.position + adjustmentTargetPosition;
            transform.position = Vector3.Lerp(transform.position, targetPosition, adjustmentTransitionSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, adjustmentTargetRotation, adjustmentTransitionSpeed * Time.deltaTime);

            Debug.Log($"Target Position: {targetPosition}, Target Rotation: {adjustmentTargetRotation}");
        }
        else
        {
            Vector3 desiredPosition = player.position - player.forward * followDistance + Vector3.up * followHeight;
            transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);
            transform.LookAt(player.position + Vector3.up * followHeight);
        }
    }   

    public void SwitchToPreset(int index)
    {
        if (index >= 0 && index < cameraPresets.Count)
        {
            var preset = cameraPresets[index];
            adjustmentTargetPosition = preset.position - player.position; // calculate relative position
            adjustmentTargetRotation = preset.rotation;

            Debug.Log($"Switching to preset {index} with Position: {adjustmentTargetPosition}, Rotation: {adjustmentTargetRotation}");

            isAdjusting = true;
        }
        else
        {
            Debug.LogWarning($"Preset index {index} is out of range");
        }
    }

    public void ClearAdjustment()
    {
        isAdjusting = false;
    }
}

