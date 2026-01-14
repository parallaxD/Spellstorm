using UnityEngine;
using System;

public class ResourceManager : MonoBehaviour
{
    private int essence;
    private int shards;

    public int Essence
    {
        get => essence;
        private set
        {
            essence = value;
            EventsManager.TriggerEssenceChanged(essence);
            SaveResources();
        }
    }

    public int Shards
    {
        get => shards;
        private set
        {
            shards = value;
            EventsManager.TriggerShardsChanged(shards);
            SaveResources();
        }
    }

    private void OnEnable()
    {
        EventsManager.OnEnemyKilled += AddEssence;
        LoadResources();

        EventsManager.TriggerEssenceChanged(Essence);
        EventsManager.TriggerShardsChanged(Shards);
    }

    private void OnDisable()
    {
        EventsManager.OnEnemyKilled -= AddEssence;
    }

    private void AddEssence(int amount)
    {
        Essence += amount;
        Debug.Log($"Получено {amount} эссенции. Всего: {Essence}");
    }

    public void IncreaseEssence(int increaseAmount)
    {
        Essence += increaseAmount;
    }

    public void IncreaseShards(int increaseAmount)
    {
        Shards += increaseAmount;
    }

    public bool TrySpendEssence(int amount)
    {
        if (Essence >= amount)
        {
            Essence -= amount;
            return true;
        }
        return false;
    }

    public bool TrySpendShards(int amount)
    {
        if (Shards >= amount)
        {
            Shards -= amount;
            return true;
        }
        return false;
    }

    private void SaveResources()
    {
        PlayerPrefs.SetInt("Essence", Essence);
        PlayerPrefs.SetInt("Shards", Shards);
        PlayerPrefs.Save();
    }

    private void LoadResources()
    {
        Essence = PlayerPrefs.GetInt("Essence", 0);
        Shards = PlayerPrefs.GetInt("Shards", 0);
    }
}