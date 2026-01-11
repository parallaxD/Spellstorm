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

    public bool isAttacking = false;

    public Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        ResetPosition();
    }

    private void FixedUpdate()
    {
        var effectiveInput = isAttacking ? Vector2.zero : moveInput;
        var targetVelocity = effectiveInput * maxSpeed;
        var accel = effectiveInput.sqrMagnitude > 0.01f ? acceleration : deceleration;
        var velocity = effectiveInput.sqrMagnitude > 0.01f ? targetVelocity : Vector2.zero;

        rb.linearVelocity = Vector2.Lerp(
                rb.linearVelocity,
                velocity,
                accel * Time.fixedDeltaTime
            );
    }

    private void Update()
    {
        UpdateAnimation();
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

    public void OnAttack(InputValue value)
    {
        if (value.isPressed && !isAttacking)
        {
            animator.SetTrigger("Attack");
        }
    }

    public void SetAttacking(bool value) => isAttacking = value;

    public Vector2 GetMoveDir() => moveInput;

    public void ResetPosition() => transform.position = Vector2.zero;

    private void UpdateAnimation()
    {
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);
        animator.SetBool("IsMoving", !isAttacking && moveInput.magnitude > 0.01f);
    }
}