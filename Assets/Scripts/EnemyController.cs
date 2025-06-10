using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private bool patrolMode = true; // toggle patrol/follow in inspector
    [SerializeField] private Transform[] patrolPoints; // patrol point waypoints
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float followSpeed = 3.5f;
    [SerializeField] private float detectionRange = 10f;
    [SerializeField] private float rotationSpeed = 10f; // <-- you can set this in the inspector
    private Vector3 velocity = Vector3.zero; // for SmoothDamp
    [SerializeField] private float smoothingTime = 0.2f; // tweak as needed
    [SerializeField] private float groundHeight = 5f;

    private Transform player;
    private Animator animator;
    private int patrolIndex = 0;
    Rigidbody rb;
    bool isChasing = false;
   
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {

        if (isChasing)
        {
            Vector3 pos = transform.position;
            pos.y = groundHeight;
            transform.position = pos;
        }


        float distToPlayer = Vector3.Distance(transform.position, player.position);

        if (patrolMode)
        {
            Patrol();
        }
        else
        {
            if (distToPlayer < detectionRange)
            {
                FollowPlayer();
            }
            else
            {
                if (animator != null)
                    animator.SetBool("isIdle", true);
            }
        }
    }

    private void Patrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0) return;

        Transform target = patrolPoints[patrolIndex];

        // Move towards patrol point
        Vector3 dir = (target.position - transform.position).normalized;

        // Move
        transform.position += dir * patrolSpeed * Time.deltaTime;

        // Face the direction of movement smoothly
        if (dir != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }

        if (animator != null)
            animator.SetBool("IsMoving", true);

        if (Vector3.Distance(transform.position, target.position) < 0.2f)
        {
            patrolIndex = (patrolIndex + 1) % patrolPoints.Length;
        }
    }


    private void FollowPlayer()
    {


        Vector3 direction = (player.position - transform.position);
        direction.y = 0;

        // Distance to player
        float distance = direction.magnitude;

        // Calculate a target position toward player
        Vector3 targetPos = transform.position + direction.normalized * Mathf.Min(distance, followSpeed * Time.deltaTime);
        targetPos.y = groundHeight; // keep on ground height

        // Smoothly move towards target position
        Vector3 smoothedPos = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smoothingTime);

        // Move Rigidbody with collision handling
        rb.MovePosition(smoothedPos);

        // Rotate to face player
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        if (animator != null)
            animator.SetBool("IsMoving", true);
    } 

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (animator != null)
                animator.SetBool("Attack", true);
            collision.gameObject.GetComponent<PlayerHealth>()?.TakeDamage(10);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (animator != null)
                animator.SetBool("Attack", false);
        }
    }
}





