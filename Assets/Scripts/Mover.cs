using UnityEngine;

public class Mover : MonoBehaviour
{
    // Movement states
    private enum MovementState { Walking, Running }

    [SerializeField] private float rotationSpeed = 120f; // degrees/sec
    [SerializeField] private float runSpeed = 7f;
    [SerializeField] private float walkSpeed = 4f;
    private float moveSpeed;

    
    [SerializeField] private float jumpForce = 8f; // Jump force
    private Rigidbody myRigidbody;
    private bool isGrounded = true;
    private bool canJump = true;
    private bool hasBeenSquashed = false;

    private PlayerSquash playerSquash;
    [SerializeField] private Animator animator;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        playerSquash = GetComponent<PlayerSquash>();

        // Initialize movement speed
        moveSpeed = walkSpeed;

        // Initialize animator parameters
        if (animator != null)
        {
            animator.SetBool("isGrounded", true);
        }
    }

    private void Update()
    {
        HandleControls();
        HandleJump();
        HandleRun();
    }

    private void HandleControls()
    {
        // Handle rotation
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // Handle movement
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * moveSpeed * Time.deltaTime;
        }

        // Set movement animation parameter
        if (animator != null)
        {
            animator.SetFloat("Speed", moveSpeed);
        }
    }

    private void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;

            if (animator != null)
            {
                animator.SetTrigger("PlayJump");
                animator.SetBool("isGrounded", false);
            }
        }
    }

    private void HandleRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
           
            moveSpeed = runSpeed;
        }
        else
        {
           
            moveSpeed = walkSpeed;
        }
    }

    public void EnableJumping()
    {
        canJump = true;
    }

    public void DisableJumping()
    {
        canJump = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        var tag = collision.gameObject.tag;

        // Detect landing
        if (tag == "Ground" || tag == "Platform")
        {
            isGrounded = true;
        }

        // Detect being squashed
        if (tag == "Boulder" && !hasBeenSquashed)
        {
            hasBeenSquashed = true;
            playerSquash?.Squash();
        }
    }
}
