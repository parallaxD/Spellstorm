using System.Collections;
using UnityEngine;

public class WindStalker : EnemyBase
{
    [Header("Wind Stalker Specific Settings")]
    [SerializeField] private GameObject windBladeProjectilePrefab;
    [SerializeField] private Transform projectileSpawnPoint;

    [SerializeField] private float rangedAttackRange = 8f; 
    [SerializeField] private float dashRange = 3f; 
    [SerializeField] private float dashSpeed = 15f; 
    [SerializeField] private float dashCooldown = 4f;
    [SerializeField] private float dashDuration = 0.3f;

    [SerializeField] private int projectilesPerSalvo = 3; 
    [SerializeField] private float timeBetweenProjectiles = 0.2f; 
    [SerializeField] private float projectileSpeed = 10f; 
    [SerializeField] private float windBladeKnockbackForce = 8f;
    [SerializeField] private float dashKnockbackForce = 12f; 

    [SerializeField] private float evasionRange = 2.5f;
    [SerializeField] private float evasionSpeed = 8f;
    [SerializeField] private float evasionCooldown = 2f;

    [SerializeField] private float windAuraRadius = 4f; 
    [SerializeField] private float windAuraSlowFactor = 0.7f; 
    [SerializeField] private float windAuraTickInterval = 0.5f;

    private bool canDash = true;
    private bool canEvade = true;
    private bool isDashing = false;
    private bool isEvading = false;
    private Coroutine dashCoroutine;
    private Coroutine windAuraCoroutine;

    private enum AIState { Idle, Chasing, RangedAttack, DashAttack, Evading }
    private AIState currentState = AIState.Idle;

    [Header("Animation Parameters")]
    [SerializeField] private string windBladeAnimTrigger = "WindBlade";
    [SerializeField] private string dashAnimTrigger = "Dash";
    [SerializeField] private string evadeAnimTrigger = "Evade";

    protected override void Start()
    {
        base.Start();
        maxSpeed = moveSpeed * 1.2f; 
        StartWindAura();
    }

    protected override void FixedUpdate()
    {
        if (!IsAlive || isDying || isAttacking || isDashing || isEvading) return;

        if (Player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, Player.position);

        switch (currentState)
        {
            case AIState.Idle:
                HandleIdleState(distanceToPlayer);
                break;
            case AIState.Chasing:
                HandleChasingState(distanceToPlayer);
                break;
            case AIState.RangedAttack:
                HandleRangedAttackState(distanceToPlayer);
                break;
            case AIState.DashAttack:
                HandleDashAttackState(distanceToPlayer);
                break;
            case AIState.Evading:
                HandleEvadingState(distanceToPlayer);
                break;
        }

        UpdateAIState(distanceToPlayer);
    }

    private void UpdateAIState(float distanceToPlayer)
    {
        if (!IsPlayerInDetectionRange)
        {
            currentState = AIState.Idle;
            return;
        }

        if (isEvading)
        {
            currentState = AIState.Evading;
            return;
        }

        if (isDashing)
        {
            currentState = AIState.DashAttack;
            return;
        }

        if (distanceToPlayer <= evasionRange && canEvade)
        {
            currentState = AIState.Evading;
            StartCoroutine(Evade());
            return;
        }

        if (distanceToPlayer <= dashRange && canDash && !isAttacking)
        {
            currentState = AIState.DashAttack;
            StartCoroutine(DashAttack());
            return;
        }

        if (distanceToPlayer <= rangedAttackRange && !isAttacking)
        {
            currentState = AIState.RangedAttack;
            return;
        }

        if (distanceToPlayer > rangedAttackRange)
        {
            currentState = AIState.Chasing;
            return;
        }
    }

    private void HandleIdleState(float distance)
    {
        shouldMove = false;
        ApplyDesiredVelocity(Vector2.zero);
    }

    private void HandleChasingState(float distance)
    {
        shouldMove = true;
        MoveTowardsPlayer();
    }

    private void HandleRangedAttackState(float distance)
    {
        shouldMove = false;
        ApplyDesiredVelocity(Vector2.zero);

        if (Player != null)
        {
            Vector2 direction = (Player.position - transform.position).normalized;
            UpdateAnimatorParameters(direction);

            if (canAttack)
            {
                StartCoroutine(WindBladeSalvo());
            }
        }
    }

    private void HandleDashAttackState(float distance)
    {
       
    }

    private void HandleEvadingState(float distance)
    {
        
    }


    private IEnumerator WindBladeSalvo()
    {
        canAttack = false;
        isAttacking = true;
        shouldMove = false;


        animator.SetTrigger(windBladeAnimTrigger);
        yield return new WaitForSeconds(0.3f);


        for (int i = 0; i < projectilesPerSalvo; i++)
        {
            if (Player == null || !IsAlive) break;

            ShootWindBlade();
            yield return new WaitForSeconds(timeBetweenProjectiles);
        }

        isAttacking = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private void ShootWindBlade()
    {
        if (Player == null || windBladeProjectilePrefab == null) return;

        Vector2 direction = (Player.position - transform.position).normalized;


        float spreadAngle = Random.Range(-10f, 10f);
        Quaternion spreadRotation = Quaternion.Euler(0, 0, spreadAngle);
        direction = spreadRotation * direction;


        GameObject projectile = Instantiate(
            windBladeProjectilePrefab,
            projectileSpawnPoint?.position ?? transform.position,
            Quaternion.identity
        );


        WindBladeProjectile blade = projectile.GetComponent<WindBladeProjectile>();
        if (blade != null)
        {
            blade.Initialize(direction, projectileSpeed, attackDamage, windBladeKnockbackForce);
        }
        else
        {

            Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = direction * projectileSpeed;
            }
        }

        UpdateAnimatorParameters(direction);
    }


    private IEnumerator DashAttack()
    {
        canDash = false;
        isDashing = true;
        shouldMove = false;

        if (Player == null)
        {
            isDashing = false;
            yield break;
        }


        Vector2 dashDirection = (Player.position - transform.position).normalized;


        animator.SetTrigger(dashAnimTrigger);
        UpdateAnimatorParameters(dashDirection);

 
        float timer = 0f;
        while (timer < dashDuration && IsAlive && !isDying)
        {
            if (Player == null) break;


            dashDirection = (Player.position - transform.position).normalized;
            rb.linearVelocity = dashDirection * dashSpeed;

            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }


        rb.linearVelocity = Vector2.zero;
        isDashing = false;

        float distanceAfterDash = Player != null ? Vector2.Distance(transform.position, Player.position) : float.MaxValue;
        if (distanceAfterDash <= dashRange * 1.5f)
        {
            ApplyDashKnockbackToPlayer();
        }


        yield return new WaitForSeconds(0.5f);
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private void ApplyDashKnockbackToPlayer()
    {
        if (Player == null) return;

        Vector2 knockbackDirection = (Player.position - transform.position).normalized;
        var playerRb = Player.GetComponent<Rigidbody2D>();
        if (playerRb != null)
        {
            playerRb.AddForce(knockbackDirection * dashKnockbackForce, ForceMode2D.Impulse);
        }

        var playerDamagable = Player.GetComponent<IDamagable>();
        if (playerDamagable != null)
        {
            playerDamagable.TakeDamage(attackDamage / 2); 
        }
    }


    private IEnumerator Evade()
    {
        canEvade = false;
        isEvading = true;
        shouldMove = false;

        if (Player == null)
        {
            isEvading = false;
            yield break;
        }

        
        Vector2 toPlayer = (Player.position - transform.position).normalized;
        Vector2 evadeDirection = Vector2.Perpendicular(toPlayer);

       
        if (Random.value > 0.5f) evadeDirection = -evadeDirection;

       
        animator.SetTrigger(evadeAnimTrigger);
        UpdateAnimatorParameters(evadeDirection);


        float evadeTimer = 0f;
        float evadeTime = 0.4f;

        while (evadeTimer < evadeTime && IsAlive && !isDying)
        {
            rb.linearVelocity = evadeDirection * evasionSpeed;
            evadeTimer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();
        }

        rb.linearVelocity = Vector2.zero;
        isEvading = false;

        yield return new WaitForSeconds(evasionCooldown);
        canEvade = true;
    }


    private void StartWindAura()
    {
        if (windAuraCoroutine != null) StopCoroutine(windAuraCoroutine);
        windAuraCoroutine = StartCoroutine(WindAuraRoutine());
    }

    private IEnumerator WindAuraRoutine()
    {
        while (IsAlive && !isDying)
        {
            yield return new WaitForSeconds(windAuraTickInterval);


            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, windAuraRadius);
            foreach (var collider in colliders)
            {
                if (collider.CompareTag("Player"))
                {

                    var effectable = collider.GetComponent<IEffectable>();
                    if (effectable != null)
                    {
                        effectable.ApplySlow(windAuraSlowFactor, windAuraTickInterval + 0.1f);
                    }


                    var rb = collider.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 pushDirection = (collider.transform.position - transform.position).normalized;
                        rb.AddForce(pushDirection * 2f, ForceMode2D.Impulse);
                    }
                }
            }
        }
    }

    protected override void UpdateAnimatorParameters(Vector2 moveDirection)
    {
        if (animator == null) return;

        animator.SetFloat("MoveX", moveDirection.x);
        animator.SetFloat("MoveY", moveDirection.y);

        if (Mathf.Abs(moveDirection.x) > Mathf.Abs(moveDirection.y))
        {
            animator.SetFloat("LastMoveX", Mathf.Sign(moveDirection.x));
            animator.SetFloat("LastMoveY", 0);
        }
        else
        {
            animator.SetFloat("LastMoveX", 0);
            animator.SetFloat("LastMoveY", Mathf.Sign(moveDirection.y));
        }
    }

    protected override void UpdateAttackAnimation(Vector2 attackDirection)
    {
        UpdateAnimatorParameters(attackDirection);
    }

    protected override string GetAttackAnimationName(Vector2 direction)
    {
        return windBladeAnimTrigger;
    }

    public override void ApplySlow(float slowFactor, float duration)
    {

        float reducedSlowFactor = Mathf.Max(0.3f, slowFactor * 0.7f);
        StartCoroutine(ReduceMovementSpeed(reducedSlowFactor, duration));
    }

    public override void TakeDamage(int damage, Vector2 damageDirection)
    {
        base.TakeDamage(damage);


        if (rb != null && IsAlive && !isDying)
        {
            rb.AddForce(damageDirection * 3f, ForceMode2D.Impulse);


            if (Random.value < 0.3f && canEvade && !isEvading)
            {
                StartCoroutine(Evade());
            }
        }
    }

    protected override void Die()
    {
        if (windAuraCoroutine != null) StopCoroutine(windAuraCoroutine);
        if (dashCoroutine != null) StopCoroutine(dashCoroutine);


        CreateWindDissipationEffect();

        base.Die();
    }

    private void CreateWindDissipationEffect()
    {
        GameObject windEffect = new GameObject("WindDissipation");
        windEffect.transform.position = transform.position;

        var spriteRenderer = windEffect.AddComponent<SpriteRenderer>();

        Destroy(windEffect, 2f);
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, rangedAttackRange);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, dashRange);

        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, evasionRange);


        Gizmos.color = new Color(0.5f, 1f, 1f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, windAuraRadius);
    }
}