using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IDamagable, IEffectable
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerHealth _playerHealth;

    public PlayerHealth Health => _playerHealth;

    public HealthBar healthBar;

    public bool IsAlive => _playerHealth.IsAlive();

    private void Start()
    {
        // _playerHealth.OnDeath += HandlePlayerDeath;
    }

    public void ApplySlow(float slowFactor, float duration)
    {
        
    }

    public void TakeDamage(int damage)
    {
        if (!IsAlive)
        {
            //Die();
            return;
        }

        _playerHealth.ReduceHealth(damage);

        StartCoroutine(DamageVisualFeedback());

        healthBar.UpdateBar(_playerHealth.HealthPercentage);
    }

    public void TakeDamage(int damage, Vector2 damageDirection)
    {

    }

    private IEnumerator DamageVisualFeedback()
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

    public void Die()
    {
        

    }
}