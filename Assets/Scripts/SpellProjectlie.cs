using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellProjectile : Projectile
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float rotationSpeed = 1000f;

    public int dotDamage = 0;
    public int dotTicks = 0;
    public float dotInterval = 0f;

    public bool applySlowEffect = false;
    public float slowFactorAmount = 0.5f;
    public float slowDurationAmount = 3f;

    private static SpellSphereCreator spellSphereCreator => GameObject.FindAnyObjectByType<SpellSphereCreator>();

    public void Initialize(int damageAmount = 30, float forceAmount = 2f,
                           float aoeRadiusAmount = 3f, bool applySlow = false,
                           float slowFactor = 0.5f, float slowDuration = 3f,
                           int fireDOTDamage = 0, int fireDOTTicks = 0, float fireDOTInterval = 0f)
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        damage = damageAmount;
        force = forceAmount;
        aoeRadius = aoeRadiusAmount;
        applySlowEffect = applySlow;
        slowFactorAmount = slowFactor;
        slowDurationAmount = slowDuration;

        dotDamage = fireDOTDamage;
        dotTicks = fireDOTTicks;
        dotInterval = fireDOTInterval;
    }

    private void FixedUpdate()
    {
        transform.Rotate(0f, 0f, rotationSpeed * Time.fixedDeltaTime);
    }

    public override void Launch(Vector2 direction)
    {
        base.Launch(direction);
        transform.right = direction.normalized;
    }

    protected override void ApplyDirectHitEffects(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy") || dotDamage <= 0 && !applySlowEffect)
            return;

        base.ApplyDirectHitEffects(collision);

        IDamagable damagable = collision.GetComponent<IDamagable>();
        if (damagable != null)
        {
            if (dotDamage > 0)
                spellEffectManager?.ApplyDOT(dotDamage, dotTicks, dotInterval, damagable);

            if (applySlowEffect)
                spellEffectManager?.ApplySlowEffect(collision.gameObject, slowFactorAmount, slowDurationAmount);
        }
    }

    protected override void ApplyAOEStatusEffects()
    {
        base.ApplyAOEStatusEffects();

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, aoeRadius);
        foreach (var collider in hitColliders)
        {
            if (!collider.CompareTag("Enemy"))
                continue;

            IDamagable damagable = collider.GetComponent<IDamagable>();
            if (damagable != null && damagable.IsAlive)
            {
                if (dotDamage > 0)
                    spellEffectManager?.ApplyDOT(dotDamage, dotTicks, dotInterval, damagable);

                if (applySlowEffect)
                    spellEffectManager?.ApplySlowEffect(collider.gameObject, slowFactorAmount, slowDurationAmount);
            }
        }
    }

    protected override Color GetExplosionColor()
    {
        if (dotDamage > 0) return new Color(1f, 0.5f, 0f);
        if (applySlowEffect) return new Color(0f, 0.5f, 1f);
        return Color.white;
    }

    public static SpellProjectile Create(Vector3 spawnPosition, List<ElementTuple> recipe,
                                         int damageAmount = 30, float forceAmount = 2f,
                                         float aoeRadiusAmount = 3f, bool applySlow = false,
                                         float slowFactor = 0.5f, float slowDuration = 3f,
                                         int fireDOTDamage = 0, int fireDOTTicks = 0, float fireDOTInterval = 0f)
    {
        var projectileObj = spellSphereCreator.CreateSpellSphere(recipe);
        projectileObj.transform.position = spawnPosition;
        var projectile = projectileObj.GetComponent<SpellProjectile>();
        projectile.Initialize(damageAmount, forceAmount, aoeRadiusAmount,
                              applySlow, slowFactor, slowDuration,
                              fireDOTDamage, fireDOTTicks, fireDOTInterval);
        return projectile;
    }
}
