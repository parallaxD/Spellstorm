using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    [SerializeField] private float moveSpeed = 5.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void OnMove(InputValue value)
    {
        moveDirection = value.Get<Vector2>();
        Debug.Log(moveDirection);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection.normalized * moveSpeed;
    }
}