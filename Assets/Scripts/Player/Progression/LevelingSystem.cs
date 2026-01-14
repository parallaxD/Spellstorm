using UnityEngine;

public class LevelingSystem : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    private float healthIncreaseMultiplier = 1.1f;

    public void UpgradeHealth()
    {
        Debug.Log("Health Increased");
        var newMaxHealth = playerHealth.MaxHealth * healthIncreaseMultiplier;
        playerHealth.SetMaxHealth((int) newMaxHealth);
    }

    public void UpgradeArmor()
    {
        Debug.Log("Armor Increased");
        var newDamageMultiplier = playerHealth.DamageMultiplier - 0.02f;
        playerHealth.SetDamageMultiplier(newDamageMultiplier);
    }
}
