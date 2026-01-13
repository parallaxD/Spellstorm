using UnityEngine;

public class QuicksandTrap : MonoBehaviour
{
    [SerializeField] private float slowFactor = 0.4f;
    [SerializeField] private float trapDuration = 5f;
    [SerializeField] private float trapRadius = 2f;

    private float destroyTime;

    public void Initialize(float slowAmount, float duration)
    {
        slowFactor = slowAmount;
        trapDuration = duration;
        destroyTime = Time.time + trapDuration;

        Destroy(gameObject, trapDuration);
    }

    private void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, trapRadius);
        foreach (var collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                var effectable = collider.GetComponent<IEffectable>();
                if (effectable != null)
                {
                    effectable.ApplySlow(slowFactor, 0.1f); 
                }

                // Слегка тянем вниз
                var rb = collider.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.AddForce(Vector2.down * 2f);
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0.8f, 0.6f, 0.2f, 0.3f);
        Gizmos.DrawWireSphere(transform.position, trapRadius);
    }
}