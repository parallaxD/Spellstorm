using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public Animator animator;
    private SpellSystem spellSystem;

    [SerializeField] private float walkSpeed = 1f;

    [SerializeField] private float runSpeed = 5f;

    [SerializeField] private float acceleration = 10f; 
    [SerializeField] private float deceleration = 12f;

    private bool isAttacking = false;
    private bool isSprinting = false;
    private bool isDead = false;

    private Vector2 currentPosition;
    private Vector2 lastPosition;
    private Vector2 clickPosition;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spellSystem = GetComponent<SpellSystem>();

        ResetPosition();
    }

    private void FixedUpdate()
    {
        var effectiveInput = isAttacking ? Vector2.zero : currentPosition;
        var speed = isSprinting ? runSpeed : walkSpeed;

        var targetVelocity = effectiveInput * speed;
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
        currentPosition = value.Get<Vector2>();

        if (currentPosition != Vector2.zero)
        {
            lastPosition.x = currentPosition.x;
            lastPosition.y = currentPosition.y;
        }
    }

    public void OnSprint(InputValue value)
    {
        isSprinting = true;
    }

    public void OnAttack(InputValue value)
    {
        if (value.isPressed && !isAttacking && spellSystem.isCreated && !isDead)
        {
            var direction = GetMousePosition();

            clickPosition.x = direction.x;
            clickPosition.y = direction.y;

            lastPosition.x = direction.x;
            lastPosition.y = direction.y;

            animator.SetTrigger("Attack");
        }
    }

    public void SetAttacking(bool value) => isAttacking = value;

    public void SetIsDead(bool value) => isDead = value;

    public Vector2 GetMoveDir() => currentPosition;

    public void ResetPosition()
    {
        transform.position = Vector2.zero;
        lastPosition = Vector2.zero;
    }

    public void DeadAnimation()
    {
        if (!isDead)
        {
            animator.SetTrigger("Death");
        }
    }

    private Vector2 GetMousePosition()
    {
        var mousePos = Mouse.current.position.ReadValue();
        var screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
        var direction = (mousePos - screenCenter).normalized;

        return direction;
    }

    private void UpdateAnimation()
    {
        animator.SetFloat("MoveX", currentPosition.x);
        animator.SetFloat("MoveY", currentPosition.y);

        animator.SetFloat("LastMoveX", lastPosition.x);
        animator.SetFloat("LastMoveY", lastPosition.y);

        animator.SetFloat("ClickX", clickPosition.x);
        animator.SetFloat("ClickY", clickPosition.y);

        if (isDead) Debug.Log(isDead);
        animator.SetBool("IsMoving", !isAttacking && !isDead && currentPosition.magnitude > 0.01f);
        animator.SetBool("IsSprinting", isSprinting);
    }
}