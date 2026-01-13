using UnityEngine;

public class WindBladeProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 10f;
    [SerializeField] private int damage = 15;
    [SerializeField] private float knockbackForce = 8f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private LayerMask collisionLayers;

    private Vector2 direction;
    private Rigidbody2D rb;

    public void Initialize(Vector2 dir, float projectileSpeed, int projectileDamage, float projectileKnockback)
    {
        direction = dir.normalized;
        speed = projectileSpeed;
        damage = projectileDamage;
        knockbackForce = projectileKnockback;

        rb = GetComponent<Rigidbody2D>();
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
        if (other.CompareTag("Enemy")) return;

        if (other.CompareTag("Player"))
        {           
            var damagable = other.GetComponent<IDamagable>();
            if (damagable != null)
            {
                damagable.TakeDamage(damage, direction * knockbackForce);
                Debug.Log("damaged player");
            }

            var rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.AddForce(direction * knockbackForce, ForceMode2D.Impulse);
            }
        }
        Destroy(gameObject);
    }
}