using UnityEngine;

public class ShardProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 5f;
    [SerializeField] private float lifetime = 1f;
    [SerializeField] private GameObject hitEffect;

    private bool hasHit = false;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    public void Initialize(float shardDamage, float shardLifetime)
    {
        damage = shardDamage;
        lifetime = shardLifetime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasHit) return;
        if (other.CompareTag("Enemy")) return;
        if (other.CompareTag("Projectile")) return;

        IDamagable damagable = other.GetComponent<IDamagable>();
        if (damagable != null)
        {
            damagable.TakeDamage(Mathf.RoundToInt(damage));
        }

        HitEffect();
        Destroy(gameObject);
    }

    private void HitEffect()
    {
        if (hitEffect != null)
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
    }
}