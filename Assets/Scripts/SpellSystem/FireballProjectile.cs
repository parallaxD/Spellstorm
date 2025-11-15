using System.Collections;
using UnityEngine;

public class FireballProjectile : MonoBehaviour
{
    [SerializeField] private int damage = 30;
    [SerializeField] private float aoeDelay = 2f;
    [SerializeField] private float force = 2f;
    [SerializeField] private float aoeRadius = 3f;

    private bool hasExploded = false;

    public static FireballProjectile Create(int spellDamage = 30)
    {
        var fireballObj = GameObject.Instantiate(
            Constants.FireballPrefab,
            Constants.PlayerTransform.position,
            Quaternion.identity
        );

        var fireball = fireballObj.AddComponent<FireballProjectile>();

        return fireball;
    }

    public void Launch(Vector2 direction)
    {
        var rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.AddForce(direction * force, ForceMode2D.Impulse);
        }

        StartCoroutine(AOEEffect());
    }

    private IEnumerator AOEEffect()
    {
        yield return new WaitForSeconds(aoeDelay);
        TriggerImmediateAOE();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasExploded) return;
        Debug.Log('a');

        if (collision.gameObject.tag == "Enemy")
        {
            StopAllCoroutines();
            TriggerImmediateAOE();
        }
    }


    private void TriggerImmediateAOE()
    {
        if (hasExploded) return;
        hasExploded = true;

        ApplyAOEDamage();

        //CreateExplosionEffect();

        Destroy(gameObject);
    }

    private void ApplyAOEDamage()
    {
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, aoeRadius);

        foreach (var collider in hitColliders)
        {
            IDamagable damagable = collider.GetComponent<IDamagable>();
            if (damagable != null && damagable.IsAlive)
            {
                Vector2 damageDirection = (collider.transform.position - transform.position).normalized;

                damagable.TakeDamage(damage, damageDirection);

                Debug.Log($"Fireball hit: {collider.name} for {damage} damage");
            }
        }
    }

    //private void CreateExplosionEffect()
    //{

    //    GameObject explosion = new GameObject("ExplosionEffect");
    //    explosion.transform.position = transform.position;

    //    var spriteRenderer = explosion.AddComponent<SpriteRenderer>();
    //    spriteRenderer.color = Color.red;
    //    Destroy(explosion, 1f);
    //}

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(transform.position, aoeRadius);
    //}
}