using UnityEngine;

public class Mover : MonoBehaviour
{
    enum MovementState { Walking, Running }

    [SerializeField] float rotationSpeed = 120f; // degrees/sec
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float walkSpeed = 4f;
    float moveSpeed;

    [SerializeField] private MovementState currentState = MovementState.Walking;

    [SerializeField] float jumpForce = 8f; // Jump force
    Vector3 jump;
    Rigidbody myRigidbody;
    bool isGrounded = true;
    bool canJump = true;
    bool hasBeenSquashed = false;
    
    PlayerSquash playerSquash;

    [SerializeField] private Animator animator;

    void Start()
    {
        jump = new Vector3(0f, 0f, 0f);
        myRigidbody = GetComponent<Rigidbody>();
        playerSquash = GetComponent<PlayerSquash>();
        animator = GetComponent<Animator>();
        moveSpeed = walkSpeed; // start at walk speed
        animator.SetBool("isGrounded", true);
    }

    void Update()
    {
        HandleControls();
        HandleJump();
        HandleRun();
    }

    void HandleControls()
    {
        // Rotation
        if (Input.GetKey(KeyCode.A))
            transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
        if (Input.GetKey(KeyCode.D))
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);

        // Movement
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += transform.forward * moveSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            transform.position -= transform.forward * moveSpeed * Time.deltaTime;
        }

        // Set movement animation parameter
        animator.SetFloat("Speed", moveSpeed);
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded && canJump)
        {
            myRigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            // Trigger jump animation
            animator.SetTrigger("PlayJump");
            animator.SetBool("isGrounded", false);    
        }
    }

    void HandleRun()
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

    public void EnableJumping()
    {
        canJump = true;
    }

    public void DisableJumping()
    {
        canJump = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Detect landing
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Platform"))
        {
            isGrounded = true;
            // No need to set boolean anymore for jump animation
        }
        // Detect being squashed
        if (collision.gameObject.CompareTag("Boulder") && !hasBeenSquashed)
        {
            hasBeenSquashed = true;
            if (playerSquash != null)
            {
                playerSquash.Squash();
            }
        }
    }
}
