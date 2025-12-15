using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform target;

    private Rigidbody2D rb;
    [SerializeField] private Animator animator;

    [SerializeField] private float detectionDistance = 10f;
    [SerializeField] private float stopDistance = 1f;
    [SerializeField] private float acceleration = 5f;
    [SerializeField] private float deceleration = 5f;
    [SerializeField] private float maxSpeed = 0.8f;

    private Vector2 moveDirection;
    private float distance;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Walk();
        UpdateAnimation();
    }

    private void Walk()
    {
        var targetVelocity = moveDirection * maxSpeed;
        var accel = acceleration;

        distance = Vector2.Distance(transform.position, target.position);

        if (IsTargetDetected())
        {
            moveDirection = (target.position - transform.position).normalized;
        }
        else
        {
            moveDirection = Vector2.zero;
            accel = deceleration;
        }

        rb.linearVelocity = Vector2.Lerp(
            rb.linearVelocity,
            targetVelocity,
            accel * Time.fixedDeltaTime
        );
    }

    private bool IsTargetDetected() => distance <= detectionDistance && distance > stopDistance;

    private void UpdateAnimation()
    {
        if (moveDirection != Vector2.zero)
        {
            animator.SetFloat("LastMoveX", moveDirection.x);
            animator.SetFloat("LastMoveY", moveDirection.y);
        }

        animator.SetFloat("MoveX", moveDirection.x);
        animator.SetFloat("MoveY", moveDirection.y);
        animator.SetBool("IsMoving", moveDirection.sqrMagnitude > 0.001f);
    }
}