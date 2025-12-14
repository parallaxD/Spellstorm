using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthUI;
    private void OnEnable()
    {
        PlayerHealth.OnHealthChanged += UpdateUI;
    }

    private void UpdateUI(int currentHealth)
    {
        _healthUI.text = $"Player health: {currentHealth}";
    }
}
