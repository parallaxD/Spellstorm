using UnityEngine;

public class FireballProjectile : Projectile
{
    [Header("Fireball Specific")]
    [SerializeField] private int dotDamage = 5;
    [SerializeField] private int dotTicks = 6;
    [SerializeField] private float dotInterval = 0.5f;

    public static FireballProjectile Create(int spellDamage = 80)
    {
        return Projectile.Create<FireballProjectile>();
    }

    protected override void ApplyDirectHitEffects(Collider2D collision)
    {
        base.ApplyDirectHitEffects(collision);

        IDamagable damagable = collision.GetComponent<IDamagable>();
        if (damagable != null)
        {
            spellEffectManager?.ApplyDOT(dotDamage, dotTicks, dotInterval, damagable);
        }
    }

    protected override void ApplyAOEStatusEffects()
    {
        base.ApplyAOEStatusEffects();

        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, aoeRadius);
        foreach (var collider in hitColliders)
        {
            IDamagable damagable = collider.GetComponent<IDamagable>();
            if (damagable != null && damagable.IsAlive)
            {
                spellEffectManager?.ApplyDOT(dotDamage, dotTicks, dotInterval, damagable);
            }
        }
    }

    protected override Color GetExplosionColor()
    {
        return new Color(1f, 0.5f, 0f);
    }
}