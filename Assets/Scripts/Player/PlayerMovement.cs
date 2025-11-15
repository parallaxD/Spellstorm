using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private Vector2 targetVelocity;

    [SerializeField] private InputHandler playerInput;

    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float acceleration = 15f;
    [SerializeField] private float deceleration = 20f;
    [SerializeField] private float velocityPower = 0.9f;
    [SerializeField] private float friction = 8f;

    [Header("Physics Settings")]
    [SerializeField] private float maxSpeed = 7f;
    [SerializeField] private bool preserveMomentum = true;

    private Matrix4x4 isoMatrix;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        SetupPhysics();
        SetupIsometricMatrix();
    }

    private void SetupPhysics()
    {
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        rb.gravityScale = 0f;
    }

    private void SetupIsometricMatrix()
    {
        float isoAngle = Mathf.Atan2(0.5f, 1f) * Mathf.Rad2Deg;
        isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 0, -isoAngle));
    }

    private Vector2 ConvertToIsometric(Vector2 input)
    {
        if (input.magnitude < 0.1f) return Vector2.zero;

        Vector3 worldDir = isoMatrix.MultiplyPoint3x4(new Vector3(input.x, input.y, 0));
        return new Vector2(worldDir.x, worldDir.y).normalized;
    }

    public void OnMove()
    {
        var input = InputHandler.Instance.Move.ReadValue<Vector2>();

        moveDirection = ConvertToIsometric(input);

        if (input.magnitude > 0.1f)
        {
            targetVelocity = moveDirection * moveSpeed;
        }
        else
        {
            targetVelocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        ApplyMovementPhysics();
        ApplyFriction();
        ClampMaximumSpeed();
    }

    private void ApplyMovementPhysics()
    {
        Vector2 velocityDifference = targetVelocity - rb.linearVelocity;

        float currentAcceleration = targetVelocity.magnitude > 0.1f ? acceleration : deceleration;

        float speedFactor = Mathf.Pow(Mathf.Abs(velocityDifference.magnitude) / moveSpeed, velocityPower);
        Vector2 movementForce = velocityDifference.normalized * currentAcceleration * speedFactor;

        rb.AddForce(movementForce, ForceMode2D.Force);
    }

    private void ApplyFriction()
    {
        if (moveDirection.magnitude < 0.1f || targetVelocity.magnitude < 0.1f)
        {
            Vector2 frictionForce = -rb.linearVelocity * friction;
            rb.AddForce(frictionForce, ForceMode2D.Force);

            if (rb.linearVelocity.magnitude < 0.2f)
            {
                rb.linearVelocity = Vector2.zero;
            }
        }
    }

    private void ClampMaximumSpeed()
    {
        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }

    private void Update()
    {
        OnMove();
        UpdateRotation();
        UpdateVisualFeedback();
    }

    private void UpdateRotation()
    {
        if (rb.linearVelocity.magnitude > 0.5f)
        {
            float targetAngle = Mathf.Atan2(rb.linearVelocity.y, rb.linearVelocity.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0, 0, targetAngle - 90f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    private void UpdateVisualFeedback()
    {
        float speedPercent = rb.linearVelocity.magnitude / maxSpeed;

        float scaleMultiplier = 1f + speedPercent * 0.1f;
        transform.localScale = Vector3.one * scaleMultiplier;
    }

    public Vector2 GetMoveDir()
    {
        return moveDirection;
    }

    //private void OnDrawGizmos()
    //{
    //    if (Application.isPlaying)
    //    {
    //        Gizmos.color = Color.blue;
    //        Gizmos.DrawRay(transform.position, moveDirection * 2f);

    //        Gizmos.color = Color.red;
    //        Gizmos.DrawRay(transform.position, rb.linearVelocity.normalized * 1.5f);

    //        Gizmos.color = Color.green;
    //        Gizmos.DrawRay(transform.position, targetVelocity.normalized * 1f);
    //    }
    //}
}