using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour, IDamagable, IEffectable
{
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float stoppingDistance = 1f;
    [SerializeField] private float smoothTime = 0.3f;
    [SerializeField] private float detectionRange = 5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private float attackDelay = 0.3f;
    [SerializeField] private int health = 100;
    [SerializeField] private Animator animator;

    private Vector2 currentVelocity;
    private Rigidbody2D rb;
    private Vector2 moveDirection;
    private bool canAttack = true;
    private bool isAttacking = false;
    private Coroutine attackCoroutine;

    public bool IsAlive => health > 0;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (!IsAlive || isAttacking) return;

        if (IsPlayerInRange())
        {
            var player = Constants.PlayerTransform;
            if (player != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, player.position);

                if (distanceToPlayer <= attackRange && canAttack)
                {
                    AttackPlayer();
                }
                else if (distanceToPlayer > stoppingDistance)
                {
                    ChasePlayer();
                    animator.SetBool("isMoving", true);
                }
                else
                {
                    animator.SetBool("isMoving", false);
                }
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
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
            animator.SetFloat("moveX", moveDirection.x);
            animator.SetFloat("moveY", moveDirection.y);
            if (moveDirection.x != 0 || moveDirection.y != 0)
            {
                animator.SetFloat("lastMoveX", moveDirection.x);
                animator.SetFloat("lastMoveY", moveDirection.y);
            }
            rb.MovePosition(newPosition);
        }
    }

    private void AttackPlayer()
    {
        if (!canAttack || isAttacking) return;

        canAttack = false;
        isAttacking = true;
        animator.SetBool("isMoving", false);

        var player = Constants.PlayerTransform;
        if (player != null)
        {
            Vector2 directionToPlayer = (player.position - transform.position).normalized;

            // Устанавливаем lastMove параметры для анимации
            animator.SetFloat("lastMoveX", directionToPlayer.x);
            animator.SetFloat("lastMoveY", directionToPlayer.y);

            // Запускаем атаку (аниматор сам выберет правильную анимацию на основе lastMove)
            animator.SetTrigger("Attack");
        }

        StartCoroutine(AttackSequence());
    }

    private string GetAttackDirection(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? "Right" : "Left";
        }
        else
        {
            return direction.y > 0 ? "Up" : "Down";
        }
    }

    private IEnumerator AttackSequence()
    {
        yield return new WaitForSeconds(attackDelay);

        var player = Constants.PlayerTransform;
        if (player != null && IsPlayerInAttackRange())
        {
            var playerDamagable = player.GetComponent<IDamagable>();
            if (playerDamagable != null)
            {
                Vector2 attackDirection = (player.position - transform.position).normalized;
                playerDamagable.TakeDamage(attackDamage, attackDirection);
            }
        }

        yield return new WaitForSeconds(0.1f);

        isAttacking = false;

        yield return new WaitForSeconds(attackCooldown - attackDelay - 0.1f);
        canAttack = true;
    }

    private bool IsPlayerInAttackRange()
    {
        var player = Constants.PlayerTransform;
        if (player == null) return false;

        float distance = Vector2.Distance(transform.position, player.position);
        return distance <= attackRange;
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

        if (rb != null)
        {
            rb.AddForce(damageDirection * 5f, ForceMode2D.Impulse);
        }
    }

    public void ApplySlow(float slowFactor, float duration)
    {
        StartCoroutine(ReduceMovementSpeed(slowFactor, duration));
    }

    private IEnumerator ReduceMovementSpeed(float slowFactor, float duration)
    {
        float originalSpeed = moveSpeed;
        moveSpeed *= slowFactor;
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed;
    }

    private void Die()
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        Destroy(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, stoppingDistance);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}