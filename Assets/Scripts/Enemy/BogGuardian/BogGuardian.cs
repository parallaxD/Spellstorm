using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BogGuardian : EnemyBase
{
    [Header("Bog Guardian Specific Settings")]
    [SerializeField] private GameObject earthProjectilePrefab;
    [SerializeField] private GameObject earthPlatformPrefab;
    [SerializeField] private GameObject earthBarrierPrefab;
    [SerializeField] private GameObject quicksandTrapPrefab;
    [SerializeField] private GameObject mudMinionPrefab;

    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Transform platformSpawnPoint;

    [Header("Attack Settings")]
    [SerializeField] private float earthProjectileRange = 12f;
    [SerializeField] private float projectileSpeed = 7f;
    [SerializeField] private int projectileDamage = 25;
    [SerializeField] private float projectileCooldown = 3f;

    [SerializeField] private float platformCreationRange = 6f;
    [SerializeField] private float platformDuration = 10f;
    [SerializeField] private float platformCooldown = 8f;

    [SerializeField] private float barrierCreationRange = 4f;
    [SerializeField] private float barrierDuration = 15f;
    [SerializeField] private float barrierCooldown = 12f;

    [SerializeField] private float quicksandTrapRange = 8f;
    [SerializeField] private float trapSlowFactor = 0.4f;
    [SerializeField] private float trapDuration = 5f;
    [SerializeField] private float trapCooldown = 6f;

    [SerializeField] private float minionSummonHealthThreshold = 0.5f;
    [SerializeField] private int minionsToSummon = 3;
    [SerializeField] private float minionSummonCooldown = 20f;

    [Header("Earth Enhancement")]
    [SerializeField] private float earthEnhancementRadius = 5f;
    [SerializeField] private float earthEnhancementArmor = 10f;
    [SerializeField] private float earthEnhancementRegen = 5f;
    private bool isOnEarth = false;
    private Coroutine earthEnhancementCoroutine;

    [Header("Water Weakness")]
    [SerializeField] private float waterSlowFactor = 0.6f;
    [SerializeField] private float waterSlowDuration = 3f;
    private bool isSlowedByWater = false;

    [Header("Animation Parameters")]
    [SerializeField] private string projectileAnimTrigger = "EarthProjectile";
    [SerializeField] private string platformAnimTrigger = "CreatePlatform";
    [SerializeField] private string barrierAnimTrigger = "CreateBarrier";
    [SerializeField] private string trapAnimTrigger = "CreateTrap";
    [SerializeField] private string summonAnimTrigger = "SummonMinions";

    private bool canShootProjectile = true;
    private bool canCreatePlatform = true;
    private bool canCreateBarrier = true;
    private bool canCreateTrap = true;
    private bool canSummonMinions = true;

    private List<GameObject> createdPlatforms = new List<GameObject>();
    private List<GameObject> createdBarriers = new List<GameObject>();
    private List<GameObject> createdTraps = new List<GameObject>();

    private enum AIState { Idle, Defensive, Attack, Summon, Special }
    private AIState currentState = AIState.Idle;
    private float timeSinceLastStateChange = 0f;
    private float stateDuration = 5f;

    private float baseArmor = 0f;
    private float currentArmor = 0f;

    protected override void Start()
    {
        base.Start();

        moveSpeed *= 0.6f;
        maxSpeed *= 0.6f;
        acceleration *= 0.5f;
        deceleration *= 0.5f;
        health = Mathf.RoundToInt(health * 1.5f);

        baseArmor = 0f;
        currentArmor = baseArmor;

        StartEarthEnhancementCheck();
    }

    protected override void Update()
    {
        base.Update();

        if (!IsAlive || isDying) return;

        timeSinceLastStateChange += Time.deltaTime;

        if (timeSinceLastStateChange > stateDuration)
        {
            ChangeStateRandomly();
        }

        CheckEarthEnhancement();
    }

    protected override void FixedUpdate()
    {
        if (!IsAlive || isDying || isAttacking) return;

        if (Player == null) return;

        float distanceToPlayer = Vector2.Distance(transform.position, Player.position);


        if (distanceToPlayer > attackRange * 2f && !isOnEarth)
        {

            shouldMove = true;
            MoveTowardsPlayer();
        }
        else
        {

            shouldMove = false;
            ApplyDesiredVelocity(Vector2.zero);
        }


        switch (currentState)
        {
            case AIState.Idle:
                HandleIdleState(distanceToPlayer);
                break;
            case AIState.Defensive:
                HandleDefensiveState(distanceToPlayer);
                break;
            case AIState.Attack:
                HandleAttackState(distanceToPlayer);
                break;
            case AIState.Summon:
                HandleSummonState(distanceToPlayer);
                break;
            case AIState.Special:
                HandleSpecialState(distanceToPlayer);
                break;
        }
    }

    private void ChangeStateRandomly()
    {
        timeSinceLastStateChange = 0f;

        Dictionary<AIState, float> stateWeights = new Dictionary<AIState, float>
        {
            { AIState.Defensive, 0.4f },
            { AIState.Attack, 0.3f },
            { AIState.Special, 0.2f },
            { AIState.Summon, 0.1f }
        };


        float healthPercentage = (float)health / (100f * 1.5f);
        if (healthPercentage <= minionSummonHealthThreshold && canSummonMinions)
        {
            currentState = AIState.Summon;
            return;
        }


        float totalWeight = 0f;
        foreach (var weight in stateWeights.Values)
        {
            totalWeight += weight;
        }

        float randomValue = Random.Range(0f, totalWeight);
        float currentWeight = 0f;

        foreach (var kvp in stateWeights)
        {
            currentWeight += kvp.Value;
            if (randomValue <= currentWeight)
            {
                currentState = kvp.Key;
                break;
            }
        }

        Debug.Log($"{gameObject.name} changed state to: {currentState}");
    }

    private void HandleIdleState(float distance)
    {
        shouldMove = false;
        ApplyDesiredVelocity(Vector2.zero);

        if (Player != null)
        {
            Vector2 direction = (Player.position - transform.position).normalized;
            UpdateAnimatorParameters(direction);
        }
    }

    private void HandleDefensiveState(float distance)
    {
        if (canCreateBarrier && distance <= barrierCreationRange)
        {
            StartCoroutine(CreateBarrier());
        }
        else if (canCreatePlatform && distance <= platformCreationRange)
        {
            StartCoroutine(CreatePlatform());
        }

        if (distance <= quicksandTrapRange && canCreateTrap)
        {
            StartCoroutine(CreateQuicksandTrap());
        }
    }

    private void HandleAttackState(float distance)
    {

        if (distance <= earthProjectileRange && canShootProjectile)
        {
            StartCoroutine(ShootEarthProjectile());
        }
    }

    private void HandleSummonState(float distance)
    {

        if (canSummonMinions)
        {
            StartCoroutine(SummonMudMinions());
        }
    }

    private void HandleSpecialState(float distance)
    {

        if (Random.value > 0.5f && canCreateBarrier)
        {
            StartCoroutine(CreateBarrier());
        }
        else if (canShootProjectile && distance <= earthProjectileRange)
        {
            StartCoroutine(ShootEarthProjectile());
        }
    }

    private void StartEarthEnhancementCheck()
    {
        if (earthEnhancementCoroutine != null)
            StopCoroutine(earthEnhancementCoroutine);

        earthEnhancementCoroutine = StartCoroutine(EarthEnhancementRoutine());
    }

    private IEnumerator EarthEnhancementRoutine()
    {
        while (IsAlive && !isDying)
        {
            CheckEarthEnhancement();
            yield return new WaitForSeconds(1f);
        }
    }

    private void CheckEarthEnhancement()
    {
        // Проверяем, стоим ли мы на земле
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f);
        bool wasOnEarth = isOnEarth;
        isOnEarth = hit.collider != null && hit.collider.CompareTag("Ground");

        if (isOnEarth && !wasOnEarth)
        {
            // Вступаем на землю - получаем усиление
            OnEnterEarth();
        }
        else if (!isOnEarth && wasOnEarth)
        {
            // Сходим с земли - теряем усиление
            OnLeaveEarth();
        }
    }

    private void OnEnterEarth()
    {
        Debug.Log($"{gameObject.name} entered earth - gaining enhancement");
        currentArmor = baseArmor + earthEnhancementArmor;

        // Визуальный эффект усиления
        StartCoroutine(EarthEnhancementEffect());

        // Начинаем регенерацию
        StartCoroutine(EarthRegeneration());
    }

    private void OnLeaveEarth()
    {
        Debug.Log($"{gameObject.name} left earth - losing enhancement");
        currentArmor = baseArmor;
    }

    private IEnumerator EarthEnhancementEffect()
    {
        // Можно добавить particle effect или изменение цвета
        var spriteRenderer = GetComponent<SpriteRenderer>();
        Color originalColor = spriteRenderer.color;
        Color earthColor = new Color(0.6f, 0.4f, 0.2f, 1f);

        spriteRenderer.color = Color.Lerp(originalColor, earthColor, 0.3f);

        while (isOnEarth && IsAlive && !isDying)
        {
            // Пульсирующий эффект
            float pulse = Mathf.PingPong(Time.time * 2f, 0.2f);
            spriteRenderer.color = Color.Lerp(originalColor, earthColor, 0.3f + pulse);
            yield return null;
        }

        spriteRenderer.color = originalColor;
    }

    private IEnumerator EarthRegeneration()
    {
        while (isOnEarth && IsAlive && !isDying)
        {
            yield return new WaitForSeconds(1f);

            health = Mathf.Min(health + Mathf.RoundToInt(earthEnhancementRegen), Mathf.RoundToInt(100f * 1.5f));

            Debug.Log($"{gameObject.name} regenerated {earthEnhancementRegen} HP on earth");
        }
    }

    private IEnumerator ShootEarthProjectile()
    {
        canShootProjectile = false;
        isAttacking = true;
        shouldMove = false;

        if (Player == null)
        {
            isAttacking = false;
            canShootProjectile = true;
            yield break;
        }

        animator.SetTrigger(projectileAnimTrigger);

        Vector2 direction = (Player.position - transform.position).normalized;
        UpdateAnimatorParameters(direction);

        yield return new WaitForSeconds(0.5f);

        if (earthProjectilePrefab != null)
        {
            GameObject projectile = Instantiate(
                earthProjectilePrefab,
                projectileSpawnPoint?.position ?? transform.position,
                Quaternion.identity
            );

            EarthProjectile earthProj = projectile.GetComponent<EarthProjectile>();
            if (earthProj != null)
            {
                earthProj.Initialize(direction, projectileSpeed, projectileDamage, this);
            }
            else
            {
                Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.linearVelocity = direction * projectileSpeed;
                }
            }
        }

        isAttacking = false;
        yield return new WaitForSeconds(projectileCooldown);
        canShootProjectile = true;
    }

    private IEnumerator CreatePlatform()
    {
        canCreatePlatform = false;
        isAttacking = true;

        animator.SetTrigger(platformAnimTrigger);

        yield return new WaitForSeconds(0.7f);

        Vector3 platformPosition;
        if (Player != null)
        {
            Vector3 directionToPlayer = (Player.position - transform.position).normalized;
            platformPosition = transform.position + directionToPlayer * (platformCreationRange / 2f);
        }
        else
        {
            platformPosition = transform.position + new Vector3(Random.Range(-3f, 3f), 0, 0);
        }

        RaycastHit2D groundHit = Physics2D.Raycast(platformPosition, Vector2.down, 10f);
        if (groundHit.collider != null)
        {
            platformPosition = new Vector3(groundHit.point.x, groundHit.point.y + 0.5f, 0);

            if (earthPlatformPrefab != null)
            {
                GameObject platform = Instantiate(earthPlatformPrefab, platformPosition, Quaternion.identity);
                createdPlatforms.Add(platform);

                StartCoroutine(DestroyAfterTime(platform, platformDuration));

                Debug.Log($"{gameObject.name} created platform at {platformPosition}");
            }
        }

        isAttacking = false;
        yield return new WaitForSeconds(platformCooldown);
        canCreatePlatform = true;
    }

    private IEnumerator CreateBarrier()
    {
        canCreateBarrier = false;
        isAttacking = true;

        animator.SetTrigger(barrierAnimTrigger);

        yield return new WaitForSeconds(0.8f);

        Vector3 barrierPosition;

        if (Player != null)
        {
            Vector3 directionToPlayer = (Player.position - transform.position).normalized;
            barrierPosition = transform.position + directionToPlayer * (barrierCreationRange / 2f);
        }
        else
        {
            float angle = Random.Range(0f, 360f);
            Vector3 offset = Quaternion.Euler(0, 0, angle) * Vector3.right * barrierCreationRange;
            barrierPosition = transform.position + offset;
        }

        if (earthBarrierPrefab != null)
        {
            GameObject barrier = Instantiate(earthBarrierPrefab, barrierPosition, Quaternion.identity);
            createdBarriers.Add(barrier);

            EarthBarrier earthBarrier = barrier.GetComponent<EarthBarrier>();
            if (earthBarrier != null)
            {
                earthBarrier.Initialize(barrierDuration, this);
            }
            else
            {
                StartCoroutine(DestroyAfterTime(barrier, barrierDuration));
            }

            Debug.Log($"{gameObject.name} created barrier at {barrierPosition}");
        }

        isAttacking = false;
        yield return new WaitForSeconds(barrierCooldown);
        canCreateBarrier = true;
    }

    private IEnumerator CreateQuicksandTrap()
    {
        canCreateTrap = false;
        isAttacking = true;


        animator.SetTrigger(trapAnimTrigger);

        yield return new WaitForSeconds(0.6f);


        Vector3 trapPosition;

        if (Player != null)
        {
            Rigidbody2D playerRb = Player.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector3 predictedPosition = Player.position + (Vector3)playerRb.linearVelocity * 0.5f;
                trapPosition = predictedPosition;
            }
            else
            {
                trapPosition = Player.position;
            }
        }
        else
        {
            trapPosition = transform.position + new Vector3(Random.Range(-5f, 5f), 0, 0);
        }

        RaycastHit2D groundHit = Physics2D.Raycast(trapPosition, Vector2.down, 10f);
        if (groundHit.collider != null)
        {
            trapPosition = new Vector3(groundHit.point.x, groundHit.point.y + 0.1f, 0);

            if (quicksandTrapPrefab != null)
            {
                GameObject trap = Instantiate(quicksandTrapPrefab, trapPosition, Quaternion.identity);
                createdTraps.Add(trap);

                QuicksandTrap quicksand = trap.GetComponent<QuicksandTrap>();
                if (quicksand != null)
                {
                    quicksand.Initialize(trapSlowFactor, trapDuration);
                }
                else
                {
                    StartCoroutine(DestroyAfterTime(trap, trapDuration));
                }

                Debug.Log($"{gameObject.name} created quicksand trap at {trapPosition}");
            }
        }

        isAttacking = false;
        yield return new WaitForSeconds(trapCooldown);
        canCreateTrap = true;
    }

    private IEnumerator SummonMudMinions()
    {
        canSummonMinions = false;
        isAttacking = true;

        animator.SetTrigger(summonAnimTrigger);

        yield return new WaitForSeconds(1f);

        for (int i = 0; i < minionsToSummon; i++)
        {
            if (mudMinionPrefab != null)
            {
                float angle = (360f / minionsToSummon) * i;
                Vector3 offset = Quaternion.Euler(0, 0, angle) * Vector3.right * 2f;
                Vector3 spawnPosition = transform.position + offset;

                RaycastHit2D groundHit = Physics2D.Raycast(spawnPosition, Vector2.down, 5f);
                if (groundHit.collider != null)
                {
                    spawnPosition = new Vector3(groundHit.point.x, groundHit.point.y + 0.5f, 0);

                    GameObject minion = Instantiate(mudMinionPrefab, spawnPosition, Quaternion.identity);
                    Debug.Log($"{gameObject.name} summoned mud minion at {spawnPosition}");
                }
            }

            yield return new WaitForSeconds(0.3f);
        }

        isAttacking = false;
        yield return new WaitForSeconds(minionSummonCooldown);
        canSummonMinions = true;
    }

    private IEnumerator DestroyAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);

        if (obj != null)
        {
            var spriteRenderer = obj.GetComponent<SpriteRenderer>();
            if (spriteRenderer != null)
            {
                float fadeTime = 0.5f;
                float elapsed = 0f;
                Color startColor = spriteRenderer.color;

                while (elapsed < fadeTime)
                {
                    elapsed += Time.deltaTime;
                    float t = elapsed / fadeTime;
                    spriteRenderer.color = Color.Lerp(startColor, Color.clear, t);
                    yield return null;
                }
            }

            Destroy(obj);
        }
    }

    public override void TakeDamage(int damage)
    {
        if (!IsAlive || isDying) return;

        int actualDamage = Mathf.Max(1, damage - Mathf.RoundToInt(currentArmor));
        Debug.Log($"{gameObject.name} took {actualDamage} damage (armor reduced from {damage})");

        base.TakeDamage(actualDamage);

        if (Random.value < 0.3f && canCreateBarrier && !isAttacking)
        {
            StartCoroutine(CreateBarrier());
        }
    }

    public override void ApplySlow(float slowFactor, float duration)
    {
        if (isSlowedByWater)
        {
            slowFactor = Mathf.Max(0.7f, slowFactor);
        }

        StartCoroutine(ReduceMovementSpeed(slowFactor, duration));
    }

    public void ApplyWaterEffect(float slowFactor, float duration)
    {
        if (!IsAlive || isDying) return;

        Debug.Log($"{gameObject.name} affected by water");
        isSlowedByWater = true;


        float waterSlow = Mathf.Min(waterSlowFactor, slowFactor);
        ApplySlow(waterSlow, Mathf.Max(waterSlowDuration, duration));


        projectileSpeed *= 0.8f;

        StartCoroutine(RemoveWaterEffect(duration));
    }

    private IEnumerator RemoveWaterEffect(float duration)
    {
        yield return new WaitForSeconds(duration);

        isSlowedByWater = false;
        projectileSpeed /= 0.8f; 
    }

    public void OnAirCurrent(Vector2 windForce)
    {
        Debug.Log($"{gameObject.name} affected by air current");


        foreach (var platform in createdPlatforms.ToArray())
        {
            if (platform != null && Random.value < 0.5f)
            {
                StartCoroutine(DestroyPlatform(platform));
            }
        }

        foreach (var barrier in createdBarriers.ToArray())
        {
            if (barrier != null && Random.value < 0.7f)
            {
                StartCoroutine(DestroyBarrier(barrier));
            }
        }

        if (rb != null && IsAlive && !isDying)
        {
            rb.AddForce(windForce * 0.5f, ForceMode2D.Impulse);
        }
    }

    private IEnumerator DestroyPlatform(GameObject platform)
    {
        if (platform == null) yield break;

        Debug.Log($"Air current destroying platform created by {gameObject.name}");

        var spriteRenderer = platform.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            float duration = 0.3f;
            float elapsed = 0f;
            Color startColor = spriteRenderer.color;
            Vector3 startScale = platform.transform.localScale;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                spriteRenderer.color = Color.Lerp(startColor, Color.clear, t);
                platform.transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
                yield return null;
            }
        }

        Destroy(platform);
        createdPlatforms.Remove(platform);
    }

    private IEnumerator DestroyBarrier(GameObject barrier)
    {
        if (barrier == null) yield break;

        Debug.Log($"Air current destroying barrier created by {gameObject.name}");

        var spriteRenderer = barrier.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            float duration = 0.4f;
            float elapsed = 0f;
            Color startColor = spriteRenderer.color;

            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / duration;
                spriteRenderer.color = Color.Lerp(startColor, Color.clear, t);
                yield return null;
            }
        }

        Destroy(barrier);
        createdBarriers.Remove(barrier);
    }

    protected override void Die()
    {
        if (earthEnhancementCoroutine != null)
            StopCoroutine(earthEnhancementCoroutine);

        foreach (var platform in createdPlatforms)
        {
            if (platform != null) Destroy(platform);
        }

        foreach (var barrier in createdBarriers)
        {
            if (barrier != null) Destroy(barrier);
        }

        foreach (var trap in createdTraps)
        {
            if (trap != null) Destroy(trap);
        }

        CreateEarthCollapseEffect();

        base.Die();
    }

    private void CreateEarthCollapseEffect()
    {
        GameObject collapseEffect = new GameObject("EarthCollapse");
        collapseEffect.transform.position = transform.position;


        Destroy(collapseEffect, 3f);
    }

    protected override void UpdateAnimatorParameters(Vector2 moveDirection)
    {
        if (animator == null) return;


        animator.SetFloat("MoveX", moveDirection.x);
        animator.SetFloat("MoveY", moveDirection.y);

        // Для анимаций атак
        animator.SetBool("IsOnEarth", isOnEarth);
    }

    protected override void UpdateAttackAnimation(Vector2 attackDirection)
    {
        UpdateAnimatorParameters(attackDirection);
    }

    protected override string GetAttackAnimationName(Vector2 direction)
    {
        return projectileAnimTrigger;
    }

    protected override void OnDrawGizmosSelected()
    {
        base.OnDrawGizmosSelected();

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, earthProjectileRange);

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, platformCreationRange);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, barrierCreationRange);

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(transform.position, quicksandTrapRange);

        // Радиус усиления от земли
        if (isOnEarth)
        {
            Gizmos.color = new Color(0.6f, 0.4f, 0.2f, 0.3f);
            Gizmos.DrawWireSphere(transform.position, earthEnhancementRadius);
        }
    }
}