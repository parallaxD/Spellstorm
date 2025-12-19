using System.Collections;
using UnityEngine;

public class AshWisp : EnemyBase
{
    [Header("Wisp Settings")]
    [SerializeField] private float shootIntervalMin = 0.8f;
    [SerializeField] private float shootIntervalMax = 1.2f;
    [SerializeField] private int minShots = 1;
    [SerializeField] private int maxShots = 3;
    [SerializeField] private float shotSpread = 15f;
    [SerializeField] private GameObject fireballPrefab;
    [SerializeField] private Transform firePoint;

    [Header("Mini Tornado")]
    [SerializeField] private GameObject miniTornadoPrefab;
    [SerializeField] private float tornadoCooldownMin = 8f;
    [SerializeField] private float tornadoCooldownMax = 12f;
    [SerializeField] private float tornadoDuration = 2f;
    [SerializeField] private float tornadoRadius = 2f;

    [Header("Levitation")]
    [SerializeField] private float hoverAmplitude = 0.5f;
    [SerializeField] private float hoverSpeed = 2f;

    private float nextTornadoTime;
    private float hoverOffset;
    private Vector3 basePosition;

    protected override void Start()
    {
        base.Start();
        ScheduleNextTornado();
        basePosition = transform.position;
    }

    protected override void FixedUpdate()
    {
        if (!IsAlive) return;
        base.FixedUpdate();

        HoverMovement();

        if (Time.time >= nextTornadoTime)
        {
            StartCoroutine(SpawnMiniTornado());
            ScheduleNextTornado();
        }
    }

    private void HoverMovement()
    {
        hoverOffset += Time.fixedDeltaTime * hoverSpeed;
        float yOffset = Mathf.Sin(hoverOffset) * hoverAmplitude;
        Vector3 targetPos = new Vector3(transform.position.x, basePosition.y + yOffset, transform.position.z);
        transform.position = targetPos;
    }

    private void ScheduleNextTornado()
    {
        nextTornadoTime = Time.time + Random.Range(tornadoCooldownMin, tornadoCooldownMax);
    }

    protected override void StartAttack()
    {
        if (!canAttack || isAttacking) return;
        isAttacking = true;
        shouldMove = false;
        if (attackCoroutine != null) StopCoroutine(attackCoroutine);
        attackCoroutine = StartCoroutine(FireballAttack());
    }

    private IEnumerator FireballAttack()
    {
        int shots = Random.Range(minShots, maxShots + 1);
        for (int i = 0; i < shots; i++)
        {
            if (fireballPrefab != null && firePoint != null && Player != null)
            {
                Vector2 dir = (Player.position - firePoint.position).normalized;
                float angleOffset = Random.Range(-shotSpread, shotSpread);
                Vector2 rotatedDir = Quaternion.Euler(0, 0, angleOffset) * dir;
                GameObject fb = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
                fb.GetComponent<Rigidbody2D>().linearVelocity = rotatedDir * 5f;
            }
            yield return new WaitForSeconds(Random.Range(shootIntervalMin, shootIntervalMax));
        }
        isAttacking = false;
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    private IEnumerator SpawnMiniTornado()
    {
        if (miniTornadoPrefab != null)
        {
            GameObject tornado = Instantiate(miniTornadoPrefab, transform.position, Quaternion.identity);
            tornado.transform.localScale = Vector3.one * tornadoRadius;
            yield return new WaitForSeconds(tornadoDuration);
            if (tornado != null) Destroy(tornado);
        }
    }

    protected override void UpdateAnimatorParameters(Vector2 moveDirection)
    {
        animator.SetFloat("moveX", moveDirection.x);
        animator.SetFloat("moveY", moveDirection.y);
        animator.SetFloat("lastMoveX", moveDirection.x);
        animator.SetFloat("lastMoveY", moveDirection.y);
    }

    protected override void UpdateAttackAnimation(Vector2 attackDirection)
    {
        animator.SetFloat("lastMoveX", attackDirection.x);
        animator.SetFloat("lastMoveY", attackDirection.y);
        animator.SetTrigger("Attack");
    }

    protected override string GetAttackAnimationName(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            return direction.x > 0 ? "AttackRight" : "AttackLeft";
        else
            return direction.y > 0 ? "AttackUp" : "AttackDown";
    }
}
