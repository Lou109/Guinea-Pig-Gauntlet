using UnityEngine;

public class PlayerFaceCamera : MonoBehaviour
{
    [SerializeField] private Transform player; // Assign your player object in Inspector
    [SerializeField] private Transform cameraTransform; // Assign your camera in Inspector
    [SerializeField] private float rotationSpeed = 5f; // Adjust for smoothness

    private bool shouldFaceCamera = false;

    public void StartFacingCamera()
    {
        shouldFaceCamera = true;
    }

    public void StopFacingCamera()
    {
        shouldFaceCamera = false;
    }

    void Update()
    {
        if (shouldFaceCamera)
        {
            Vector3 direction = cameraTransform.position - player.position;
            direction.y = 0; // keep only on the horizontal plane

            if (direction.sqrMagnitude > 0.001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                player.rotation = Quaternion.Slerp(player.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
    }
}


