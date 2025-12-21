using UnityEngine;
using UnityEngine.EventSystems;
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

        var accel = moveInput.sqrMagnitude > 0.01f ? acceleration : deceleration;
        var velocity = moveInput.sqrMagnitude > 0.01f ? targetVelocity : Vector2.zero;

        rb.linearVelocity = Vector2.Lerp(
                rb.linearVelocity,
                velocity,
                accel * Time.fixedDeltaTime
            );

        UpdateAnimation();
    }

    public Vector2 GetMoveDir()
    {
        return moveInput;
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);
        animator.SetBool("IsMoving", moveInput.magnitude > 0.01f);
    }
}