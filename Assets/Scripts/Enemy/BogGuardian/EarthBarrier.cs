using UnityEngine;

public class EarthBarrier : MonoBehaviour
{
    [SerializeField] private int maxHealth = 50;
    [SerializeField] private float destroyTime = 15f;

    private int currentHealth;
    private BogGuardian owner;

    public void Initialize(float duration, BogGuardian barrierOwner)
    {
        destroyTime = duration;
        currentHealth = maxHealth;
        owner = barrierOwner;

        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Projectile"))
        {
            TakeDamage(10);
            Destroy(other.gameObject);
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Визуальная обратная связь
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            float healthPercent = (float)currentHealth / maxHealth;
            spriteRenderer.color = Color.Lerp(Color.red, Color.white, healthPercent);
        }

        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }
}