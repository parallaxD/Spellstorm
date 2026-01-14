using System;
using UnityEngine;

public static class EventsManager
{
    public static event Action<int> OnEnemyKilled;
    public static event Action<int> OnEssenceChanged;
    public static event Action<int> OnShardsChanged;

    public static void TriggerEnemyKilled(int essenceAmount)
    {
        OnEnemyKilled?.Invoke(essenceAmount);
    }

    public static void TriggerEssenceChanged(int newAmount)
    {
        OnEssenceChanged?.Invoke(newAmount);
    }

    public static void TriggerShardsChanged(int newAmount)
    {
        OnShardsChanged?.Invoke(newAmount);
    }
}
