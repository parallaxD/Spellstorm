using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveInput;

    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float acceleration = 10f; 
    [SerializeField] private float deceleration = 12f;  

    public Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        if (moveInput != Vector2.zero)
        {
            animator.SetFloat("LastMoveX", moveInput.x);
            animator.SetFloat("LastMoveY", moveInput.y);
        }
    }

    private void FixedUpdate()
    {
        var targetVelocity = moveInput * maxSpeed;

        if (moveInput.sqrMagnitude > 0.01f)
        {
            rb.linearVelocity = Vector2.Lerp(
                rb.linearVelocity,
                targetVelocity,
                acceleration * Time.fixedDeltaTime
            );
        }
        else
        {
            rb.linearVelocity = Vector2.Lerp(
                rb.linearVelocity,
                Vector2.zero,
                deceleration * Time.fixedDeltaTime
            );
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);
        animator.SetBool("IsMoving", moveInput.magnitude > 0.01f);
    }

    public Vector2 GetMoveDir()
    {
        return moveInput;
    }

    public void ReduceMaxSpeed(int speedToSet)
    {
        maxSpeed = speedToSet;
    }
}