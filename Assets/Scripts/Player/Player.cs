using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour, IDamagable, IEffectable
{
    [SerializeField] private PlayerMovement _playerMovement;
    [SerializeField] private PlayerHealth _playerHealth;

    [SerializeField] private GenerationManager _generationManager;
    [SerializeField] private UIManager _uiManager;

    public PlayerHealth Health => _playerHealth;

    public HealthBar healthBar;

    public bool IsAlive => _playerHealth.IsAlive();

    private void Start()
    {
        // _playerHealth.OnDeath += HandlePlayerDeath;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(110);
        }
    }

    public void ApplySlow(float slowFactor, float duration)
    {
        
    }

    public void TakeDamage(int damage)
    {
        _playerHealth.ReduceHealth(damage);
        if (!IsAlive)
        {
            Die();
            return;
        }

        StartCoroutine(DamageVisualFeedback());

        healthBar.UpdateBar(_playerHealth.HealthPercentage);
    }

    public void TakeDamage(int damage, Vector2 damageDirection)
    {
        _playerHealth.ReduceHealth(damage);

        StartCoroutine(DamageVisualFeedback());

        healthBar.UpdateBar(_playerHealth.HealthPercentage);
    }

    public void Heal(int amount)
    {
        _playerHealth.RestoreHealth(amount);
    }

    public void Die()
    {
        _playerMovement.DeadAnimation();

        StartCoroutine(ShowResult());
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

    private IEnumerator ShowResult()
    {
        Debug.Log("YO!");
        yield return new WaitForSeconds(2f);
        Debug.Log("YO!!!");
        _uiManager.ShowResultPanel();
        _generationManager.HubGeneration();
        Debug.Log("YO!!!!");
        _playerHealth.SetCurrentHealth(100);
        Debug.Log(_playerHealth.CurrentHealth);
        healthBar.UpdateBar(_playerHealth.HealthPercentage);
    }
}