using UnityEngine;

public class Player : MonoBehaviour, IDamagable, IEffectable
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerHealth _playerHealth;

    public PlayerHealth Health => _playerHealth;

    public bool IsAlive => _playerHealth.IsAlive();

    private void Start()
    {
        // Можно подписаться на события здесь, если нужно
        // _playerHealth.OnDeath += HandlePlayerDeath;
    }

    public void ApplySlow(float slowFactor, float duration)
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (!IsAlive) return;

        _playerHealth.ReduceHealth(damage);

        StartCoroutine(DamageVisualFeedback());
    }

    public void TakeDamage(int damage, Vector2 damageDirection)
    {
        
    }

    private System.Collections.IEnumerator DamageVisualFeedback()
    {
        var spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = Color.white;
        }
    }

    public void Heal(int amount)
    {
        _playerHealth.RestoreHealth(amount);
    }

    private void OnDestroy()
    {
        // if (_playerHealth != null)
        //     _playerHealth.OnDeath -= HandlePlayerDeath;
    }
}