using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField] enum MovementState { Walking, Running }

    [SerializeField] float rotationSpeed = 120f; // degrees/sec
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float moveSpeed; // dynamic based on states

    [SerializeField] private MovementState currentState = MovementState.Walking;

    [SerializeField] float jumpForce = 8f;
    [SerializeField] float jumph = 0f; // unused if jumping physics remains same
    Vector3 jump;
    Rigidbody myRigidbody;
    bool isGrounded;
    bool canJump;
    bool hasBeenSquashed = false;

    PlayerSquash playerSquash;
    [SerializeField] private Animator animator;

    void Start()
    {
        jump = new Vector3(0f, jumph, 0f);
        myRigidbody = GetComponent<Rigidbody>();
        playerSquash = GetComponent<PlayerSquash>();
        animator = GetComponent<Animator>();
        canJump = true; // initialize jump ability
        moveSpeed = walkSpeed; // start at walk speed
    }

    void Update()
    {
        HandleTankControls();
        PlayerJump();
        PlayerRun();
    }

    void HandleTankControls()
    {
        // Rotation with A/D keys - no smooth rotation, instant turn
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // Movement with W/S keys
        if (Input.GetKey(KeyCode.W))
        {
            // Move forward in facing direction
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            // Move backward in facing direction
            transform.position -= transform.forward * moveSpeed * Time.deltaTime;
        }

        // Set animation parameter
        animator.SetFloat("Speed", moveSpeed);
    }

    void PlayerJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
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

    void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Boulder") && !hasBeenSquashed)
        {
            hasBeenSquashed = true;
            var playerSquash = GetComponent<PlayerSquash>();
            if (playerSquash != null)
            {
                playerSquash.Squash();
            }
        }

        if (other.gameObject.CompareTag("Ground") || other.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
        }
    }

    void PlayerRun()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            currentState = MovementState.Running;
            moveSpeed = runSpeed;
        }
        else
        {
            currentState = MovementState.Walking;
            moveSpeed = walkSpeed;
        }
    }
}

