using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveInput;
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float maxSpeed = 1f;
    [SerializeField] private float minSpeed = 0.1f;
    [SerializeField] private float friction = 8f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
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
    }
}