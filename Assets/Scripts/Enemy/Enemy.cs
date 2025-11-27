using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable
{
    public float moveSpeed = 3f;
    public float stoppingDistance = 1f;
    public float smoothTime = 0.3f;
    public float detectionRange = 0.0001f;

    private Vector2 currentVelocity;
    private Rigidbody2D rb;

    [SerializeField] private int health = 100;
    [SerializeField] private Animator _animator;

    private Vector2 moveDirection;

    public bool IsAlive => health > 0;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        _animator.SetBool("isMoving", true);
        if (IsPlayerInRange())
        {
            ChasePlayer();
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }
    }

    private bool IsPlayerInRange()
    {
        var player = Constants.PlayerTransform;
        if (player == null) return false;

        float distance = Vector2.Distance(transform.position, player.position);
        return distance <= detectionRange;
    }

    private void ChasePlayer()
    {
        var player = Constants.PlayerTransform;
        if (player == null) return;

        Vector2 targetPosition = player.position;
        float distance = Vector2.Distance(rb.position, targetPosition);

        if (distance > stoppingDistance)
        {
            Vector2 newPosition = Vector2.SmoothDamp(rb.position, targetPosition, ref currentVelocity, smoothTime, moveSpeed);
            moveDirection = (newPosition - rb.position).normalized;
            _animator.SetFloat("moveX", moveDirection.x);
            _animator.SetFloat("moveY", moveDirection.y);
            if (moveDirection.x != 0 || moveDirection.y != 0)
            {
                _animator.SetFloat("lastMoveX", moveDirection.x);
                _animator.SetFloat("lastMoveY", moveDirection.y);
            }
            rb.MovePosition(newPosition);
        }
    }

    public void TakeDamage(int damage)
    {
        if (!IsAlive) return;

        health -= damage;

        if (!IsAlive)
        {
            Die();
        }
    }

    public void TakeDamage(int damage, Vector2 damageDirection)
    {
        TakeDamage(damage);

        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(damageDirection * 5f, ForceMode2D.Impulse);
        }
    }

    private void Die()
    {
        Debug.Log("Enemy died!");
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);
    }
}