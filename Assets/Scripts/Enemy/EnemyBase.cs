using System.Collections;
using UnityEngine;

public abstract class EnemyBase : MonoBehaviour, IDamagable, IEffectable
{
    [Header("Movement Settings")]
    [SerializeField] protected float moveSpeed = 3f;
    [SerializeField] protected float smoothTime = 0.3f;
    [SerializeField] protected float detectionRange = 5f;
    [SerializeField] protected float maxSpeed = 1.5f;
    [SerializeField] protected float acceleration = 5f;
    [SerializeField] protected float deceleration = 5f;

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
    [SerializeField] private float stopDistance = 0.5f;
    [SerializeField] private float resumeMoveDistance = 0.01f;
    private bool isStopped;
    protected bool canAttack = true;
    protected bool isAttacking = false;
    protected Coroutine attackCoroutine;
    public bool IsAlive => health > 0;
    protected Transform Player => Constants.PlayerTransform;
    protected bool IsPlayerInDetectionRange => Player != null && Vector2.Distance(transform.position, Player.position) <= detectionRange;
    protected abstract void UpdateAnimatorParameters(Vector2 moveDirection);
    protected abstract void UpdateAttackAnimation(Vector2 attackDirection);
    protected abstract string GetAttackAnimationName(Vector2 direction);
    [SerializeField] private float animationVelocityThreshold = 0.05f;
    protected bool shouldMove;
    protected virtual void Start()
    {
        if (animator == null) animator = GetComponent<Animator>();
        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (rb != null) rb.interpolation = RigidbodyInterpolation2D.Interpolate;
    }
    protected virtual void Update()
    {
        if (!IsAlive) return;
        animator.SetBool("isMoving", shouldMove); 
        UpdateMovementAnimation();
    }
    protected virtual void UpdateMovementAnimation()
    {
        if (shouldMove)
        {
            Vector2 dir = rb.linearVelocity.normalized;
            if (dir.magnitude < 0.1f) dir = (Player.position - transform.position).normalized;
            UpdateAnimatorParameters(dir);
        }
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
        float distance = Vector2.Distance(transform.position, Player.position);
        if (!isStopped && distance <= stopDistance)
        {
            isStopped = true;
        }
        if (isStopped && distance >= resumeMoveDistance)
        {
            isStopped = false;
        }
        if (isStopped)
        {
            shouldMove = false;
            if (distance <= attackRange && canAttack)
            {
                StartAttack();
            }
            ApplyDesiredVelocity(Vector2.zero);
            return;
        }
        shouldMove = true;
        MoveTowardsPlayer();
    }
    protected virtual void HandleIdleBehavior()
    {
        shouldMove = false;
        ApplyDesiredVelocity(Vector2.zero);
    }
    protected virtual void MoveTowardsPlayer()
    {
        if (Player == null || !IsAlive) return;
        float distance = Vector2.Distance(transform.position, Player.position);
        Vector2 moveDirection = (Player.position - transform.position).normalized;
        Vector2 desiredVelocity;
        if (distance <= PreferredStoppingDistance)
        {
            desiredVelocity = Vector2.zero;
        }
        else
        {
            desiredVelocity = moveDirection * maxSpeed;
        }
        ApplyDesiredVelocity(desiredVelocity);
    }
    protected virtual void ApplyDesiredVelocity(Vector2 desired)
    {
        float lerpSpeed = desired.magnitude > 0 ? acceleration : deceleration;
        rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, desired, lerpSpeed * Time.fixedDeltaTime);
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
    }
    protected virtual void StartAttack()
    {
        if (!canAttack || isAttacking) return;
        canAttack = false;
        isAttacking = true;
        shouldMove = false; 
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
        maxSpeed *= slowFactor; 
        yield return new WaitForSeconds(duration);
        moveSpeed = originalSpeed;
        maxSpeed = originalSpeed * (1f / slowFactor);
    }
    protected virtual void Die()
    {
        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);
        animator.SetBool("isMoving", false);
        animator.SetTrigger("Death");
    }
    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, PreferredStoppingDistance);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stopDistance);
    }
}