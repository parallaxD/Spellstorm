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

    public bool IsAlive => health > 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (IsPlayerInRange())
        {
            ChasePlayer();
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