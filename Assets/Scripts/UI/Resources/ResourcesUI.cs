using TMPro;
using UnityEngine;

public class ResourcesUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI essenceText;
    [SerializeField] private TextMeshProUGUI shardsText;

    private void OnEnable()
    {
        EventsManager.OnEssenceChanged += UpdateEssenceUI;
        EventsManager.OnShardsChanged += UpdateShardsUI;
    }

    private void OnDisable()
    {
        EventsManager.OnEssenceChanged -= UpdateEssenceUI;
        EventsManager.OnShardsChanged -= UpdateShardsUI;
    }

    private void UpdateEssenceUI(int amount)
    {
        if (essenceText != null)
        {
            essenceText.text = amount.ToString();
        }
    }

    private void UpdateShardsUI(int amount)
    {
        if (shardsText != null)
        {
            shardsText.text = amount.ToString();
        }
    }
}
