using System.Collections;
using UnityEngine;

public class Shardling : EnemyBase
{
    [Header("Shardling Settings")]
    [SerializeField] private GameObject shardPrefab;
    [SerializeField] private int shardCount = 8;
    [SerializeField] private float shardSpreadForce = 5f;
    [SerializeField] private float shardDamage = 5f;
    [SerializeField] private float deathExplosionRadius = 3f;

    private bool deathExplosionTriggered = false;

    protected override void Start()
    {
        base.Start();
        detectionRange = 4f;
        attackRange = 1.2f;
        maxSpeed = 4f;
    }

    protected override void UpdateAnimatorParameters(Vector2 moveDirection)
    {
        if (moveDirection.magnitude < 0.1f && Player != null)
        {
            moveDirection = Player.position - transform.position;
        }

        animator.SetFloat("moveX", moveDirection.x);
        animator.SetFloat("moveY", moveDirection.y);
        animator.SetFloat("lastMoveX", moveDirection.x);
        animator.SetFloat("lastMoveY", moveDirection.y);
    }

    protected override void UpdateAttackAnimation(Vector2 attackDirection)
    {
        animator.SetFloat("moveX", attackDirection.x);
        animator.SetFloat("moveY", attackDirection.y);
        animator.SetTrigger("Attack");
    }

    protected override string GetAttackAnimationName(Vector2 direction)
    {
        return "Attack";
    }

    protected override void HandleCombatBehavior()
    {
        float distance = Vector2.Distance(transform.position, Player.position);

        if (distance > attackRange)
        {
            shouldMove = true;
            MoveTowardsPlayer();
        }
        else
        {
            shouldMove = false;
            ApplyDesiredVelocity(Vector2.zero);

            if (canAttack)
            {
                StartAttack();
            }
        }
    }

    protected override void PerformAttack()
    {
        if (Player != null && IsPlayerInAttackRange() && !isDying)
        {
            var playerDamagable = Player.GetComponent<IDamagable>();
            if (playerDamagable != null)
            {
                playerDamagable.TakeDamage(attackDamage);
            }
        }
    }

    protected override void Die()
    {
        if (isDying) return;

        isDying = true;
        rb.linearVelocity = Vector2.zero;
        rb.angularVelocity = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = false;

        if (attackCoroutine != null)
            StopCoroutine(attackCoroutine);

        StopAllCoroutines();

        shouldMove = false;
        isAttacking = false;
        canAttack = false;

        animator.SetBool("isMoving", false);
        animator.SetTrigger("Death");

        TriggerDeathExplosion();

        EventsManager.TriggerEnemyKilled(essenceReward);

        StartCoroutine(DisappearAfterExplosion());
    }

    private void TriggerDeathExplosion()
    {
        if (deathExplosionTriggered) return;
        deathExplosionTriggered = true;

        for (int i = 0; i < shardCount; i++)
        {
            float angle = i * (360f / shardCount);
            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));

            GameObject shard = Instantiate(shardPrefab, transform.position, Quaternion.identity);
            Rigidbody2D shardRb = shard.GetComponent<Rigidbody2D>();

            if (shardRb != null)
            {
                shardRb.AddForce(direction * shardSpreadForce, ForceMode2D.Impulse);
            }

            ShardProjectile shardProjectile = shard.GetComponent<ShardProjectile>();
            if (shardProjectile != null)
            {
                shardProjectile.Initialize(shardDamage, 1f);
            }
        }
    }

    private IEnumerator DisappearAfterExplosion()
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}