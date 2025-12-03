using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamagable, IEffectable
{
    [Header("Movement Settings")]
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected float smoothTime = 0.3f;
    [SerializeField] protected float detectionRange = 5f;

    [Header("Combat Settings")]
    [SerializeField] protected float attackRange = 1.5f;
    [SerializeField] protected int attackDamage = 10;
    [SerializeField] protected float attackCooldown = 2f;
    [SerializeField] protected float attackDelay = 0.3f;
    [SerializeField] protected int health = 100;

    [Header("Components")]
    [SerializeField] protected Animator animator;
    [SerializeField] protected Rigidbody2D rb;

    public float PreferredStoppingDistance => attackRange * 0.9f; 

    protected Vector2 currentVelocity;

    protected bool canAttack = true;
    protected bool isAttacking = false;
    protected Coroutine attackCoroutine;

    public bool IsAlive => health > 0;
    protected Transform Player => Constants.PlayerTransform;
    protected bool IsPlayerInDetectionRange => Player != null && Vector2.Distance(transform.position, Player.position) <= detectionRange;

    protected abstract void UpdateAnimatorParameters(Vector2 moveDirection);
    protected abstract void UpdateAttackAnimation(Vector2 attackDirection);
    protected abstract string GetAttackAnimationName(Vector2 direction);

    protected virtual void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void FixedUpdate()
    {
        if (!IsAlive || isAttacking) return;

        if (IsPlayerInDetectionRange && Player != null)
        {
            HandleCombatBehavior();
        }
        else
        {
            HandleIdleBehavior();
        }
    }

    protected virtual void HandleCombatBehavior()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, Player.position);

        if (distanceToPlayer <= attackRange && canAttack)
        {
            StartAttack();
        }
        else if (distanceToPlayer > PreferredStoppingDistance)
        {
            MoveTowardsPlayer();
        }
        else
        {
            HandleStationaryPosition();
        }
    }

    protected virtual void HandleIdleBehavior()
    {
        animator.SetBool("isMoving", false);
    }

    protected virtual void MoveTowardsPlayer()
    {
        Vector2 targetPosition = Player.position;
        Vector2 newPosition = Vector2.SmoothDamp(
            rb.position,
            targetPosition,
            ref currentVelocity,
            smoothTime,
            moveSpeed
        );

        Vector2 moveDirection = (newPosition - rb.position).normalized;
        UpdateAnimatorParameters(moveDirection);

        animator.SetBool("isMoving", true);
        rb.MovePosition(newPosition);
    }

    protected virtual void HandleStationaryPosition()
    {
        if (currentVelocity.magnitude > 0.1f)
        {
            Vector2 newPosition = Vector2.SmoothDamp(
                rb.position,
                rb.position,
                ref currentVelocity,
                smoothTime * 0.5f,
                moveSpeed
            );
            rb.MovePosition(newPosition);
        }

        animator.SetBool("isMoving", false);
    }

    protected virtual void StartAttack()
    {
        if (!canAttack || isAttacking) return;

        canAttack = false;
        isAttacking = true;
        animator.SetBool("isMoving", false);

        Vector2 directionToPlayer = (Player.position - transform.position).normalized;
        UpdateAttackAnimation(directionToPlayer);

        if (attackCoroutine != null) StopCoroutine(attackCoroutine);
        attackCoroutine = StartCoroutine(AttackSequence());
    }

    protected virtual IEnumerator AttackSequence()
    {
        yield return new WaitForSeconds(attackDelay);

        PerformAttack();
        yield return new WaitForSeconds(0.1f);
        isAttacking = false;

        yield return new WaitForSeconds(attackCooldown - attackDelay - 0.1f);
        canAttack = true;
    }

    protected virtual void PerformAttack()
    {
        if (Player != null && IsPlayerInAttackRange())
        {
            var playerDamagable = Player.GetComponent<IDamagable>();
            if (playerDamagable != null)
            {
                playerDamagable.TakeDamage(attackDamage);


                Debug.Log($"{gameObject.name} наносит {attackDamage} урона игроку. Оставшееся HP: {((MonoBehaviour)playerDamagable).GetComponent<PlayerHealth>()?.GetCurrentHealth() ?? 0}");
            }
        }
    }

    protected virtual bool IsPlayerInAttackRange()
    {
        return Player != null && Vector2.Distance(transform.position, Player.position) <= attackRange;
    }

    public virtual void TakeDamage(int damage)
    {
        if (!IsAlive) return;

        health -= damage;

        if (!IsAlive)
        {
            Die();
        }
    }

    public virtual void TakeDamage(int damage, Vector2 damageDirection)
    {
        TakeDamage(damage);

        if (rb != null)
        {
            rb.AddForce(damageDirection * 5f, ForceMode2D.Impulse);
        }
    }

    public virtual void ApplySlow(float slowFactor, float duration)
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

    protected virtual void Die()
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        Destroy(gameObject);
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, PreferredStoppingDistance);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}