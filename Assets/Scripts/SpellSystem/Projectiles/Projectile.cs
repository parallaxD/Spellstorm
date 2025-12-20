using System.Collections;
using UnityEngine;

public abstract class Projectile : MonoBehaviour
{
    [SerializeField] protected int damage = 30;
    [SerializeField] protected float force = 2f;
    [SerializeField] protected float lifetime = 5f;
    [SerializeField] protected float aoeDelay = 2f;
    [SerializeField] protected float aoeRadius = 3f;
    [SerializeField] protected bool explodeOnEnemyContact = true;
    [SerializeField] protected bool explodeAfterDelay = true;
    [SerializeField] protected bool applySlow = false;
    [SerializeField] protected float slowFactor = 0.5f;
    [SerializeField] protected float slowDuration = 3f;
    [SerializeField] protected float slowRadius = 3f;

    protected bool hasExploded = false;
    protected Rigidbody2D rb;
    protected SpellEffectManager spellEffectManager;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spellEffectManager = GameObject.FindAnyObjectByType<SpellEffectManager>();
    }

    public static T Create<T>() where T : Projectile
    {
        GameObject prefab = GetPrefabForType<T>();
        var projectileObj = Instantiate(prefab, Constants.PlayerTransform.position, Quaternion.identity);
        return projectileObj.GetComponent<T>();
    }

    private static GameObject GetPrefabForType<T>() where T : Projectile
    {
        return typeof(T).Name switch
        {
            "FireballProjectile" => Constants.FireballPrefab,
            "WaterballProjectile" => Constants.WaterballPrefab,
            "EarthProjectile" => Constants.EarthProjectile,
            "WindProjectile" => Constants.WindProjectile,
            _ => null
        };
    }

    public virtual void Launch(Vector2 direction)
    {
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        if (explodeAfterDelay)
            StartCoroutine(DelayedExplosion());

        StartCoroutine(LifetimeCountdown());
    }

    protected virtual IEnumerator DelayedExplosion()
    {
        yield return new WaitForSeconds(aoeDelay);
        TriggerImmediateAOE();
    }

    protected virtual IEnumerator LifetimeCountdown()
    {
        yield return new WaitForSeconds(lifetime);
        if (!hasExploded)
            TriggerImmediateAOE();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasExploded) return;

        if (explodeOnEnemyContact && collision.CompareTag("Enemy"))
        {
            ApplyDirectHitEffects(collision);
            TriggerImmediateAOE();
        }
    }

    protected virtual void ApplyDirectHitEffects(Collider2D collision)
    {
        var damagable = collision.GetComponent<IDamagable>();
        if (damagable != null)
            damagable.TakeDamage(damage);

        if (applySlow && collision.CompareTag("Enemy"))
            spellEffectManager?.ApplySlowEffect(collision.gameObject, slowFactor, slowDuration);
    }

    protected virtual void TriggerImmediateAOE()
    {
        if (hasExploded) return;
        hasExploded = true;

        ApplyAOEDamage();
        ApplyAOEStatusEffects();
        CreateExplosionEffect();

        StopAllCoroutines();
        Destroy(gameObject);
    }

    protected virtual void ApplyAOEDamage()
    {
        var hitColliders = Physics2D.OverlapCircleAll(transform.position, aoeRadius);
        foreach (var collider in hitColliders)
        {
            var damagable = collider.GetComponent<IDamagable>();
            if (damagable != null && damagable.IsAlive)
                damagable.TakeDamage(damage);
        }
    }

    protected virtual void ApplyAOEStatusEffects()
    {
        if (applySlow)
            spellEffectManager?.ApplyAOESlow(transform.position, slowRadius, slowFactor, slowDuration);
    }

    protected virtual void CreateExplosionEffect()
    {
        var explosion = new GameObject($"{GetType().Name}Explosion");
        explosion.transform.position = transform.position;

        var spriteRenderer = explosion.AddComponent<SpriteRenderer>();
        spriteRenderer.color = GetExplosionColor(); 

        Destroy(explosion, 1f);
    }

    protected virtual Color GetExplosionColor()
    {
        return Color.red;
    }

    protected virtual void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, aoeRadius);

        if (applySlow && slowRadius != aoeRadius)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, slowRadius);
        }
    }
}