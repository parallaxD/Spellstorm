using UnityEngine;

public class EarthProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 7f;
    [SerializeField] private int damage = 25;
    [SerializeField] private float lifetime = 4f;
    [SerializeField] private GameObject impactEffect;

    private Vector2 direction;
    private BogGuardian owner;

    public void Initialize(Vector2 dir, float projectileSpeed, int projectileDamage, BogGuardian projectileOwner)
    {
        direction = dir.normalized;
        speed = projectileSpeed;
        damage = projectileDamage;
        owner = projectileOwner;

        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * speed;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.gameObject == owner?.gameObject) return;

        if (other.CompareTag("Player"))
        {
            var damagable = other.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage, direction * 5f);
            }
        }

        // Эффект удара
        if (impactEffect != null)
        {
            Instantiate(impactEffect, transform.position, Quaternion.identity);
        }

        Destroy(gameObject);
    }
}