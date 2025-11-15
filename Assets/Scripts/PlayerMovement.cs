using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveInput;
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float maxSpeed = 1f;
    [SerializeField] private float minSpeed = 0.1f;
    [SerializeField] private float friction = 8f;

    private Animator animator;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();

        var x = moveInput.x;
        var y = moveInput.y;

        if (x != 0 || y != 0)
        {
            animator.SetFloat("LastMoveX", x);
            animator.SetFloat("LastMoveY", y);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(moveInput * moveSpeed);

        if (moveInput.magnitude < minSpeed)
        {
            rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, Vector2.zero, friction * Time.fixedDeltaTime);
        }


        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        UpdateAnimation();
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("MoveX", moveInput.x);
        animator.SetFloat("MoveY", moveInput.y);
        animator.SetBool("IsMoving", moveInput.magnitude > 0.01f);
    }
}