using UnityEngine;

public class LevelingSystem : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    private float healthIncreaseMultiplier = 1.1f;

    public void UpgradeHealth()
    {
        var newMaxHealth = playerHealth.MaxHealth * healthIncreaseMultiplier;
        playerHealth.SetMaxHealth((int) newMaxHealth);
    }

    public void UpgradeArmor()
    {
        var newDamageMultiplier = playerHealth.DamageMultiplier - 0.05f;
        playerHealth.SetDamageMultiplier(newDamageMultiplier);
    }
}
