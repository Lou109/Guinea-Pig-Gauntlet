using UnityEngine;

public class BoulderController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 currentPushDirection = Vector3.zero;

    [SerializeField] private float pushForce = 50f;     // Force applied each FixedUpdate
    [SerializeField] private float maxSpeed = 10f;      // Max allowed speed

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.None;
        GetComponent<RotateObjects>().enabled = false;
    }

    public void SetPushDirection(Vector3 direction)
    {
        currentPushDirection = direction.normalized;
    }

    public void LaunchInstantly(float initialSpeed)
    {
        rb.linearVelocity = currentPushDirection * initialSpeed;
        GetComponent <RotateObjects>().enabled = true;
    }

    public void StartApplyingForce()
    {
        // Call this to start applying force (if needed)
    }

    private void FixedUpdate()
    {
        if (currentPushDirection != Vector3.zero)
        {
            rb.AddForce(currentPushDirection * pushForce, ForceMode.Force);
        }

        // Limit the maximum speed
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
}
