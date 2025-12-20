using System.Collections;
using UnityEngine;

public interface IStatusEffect
{
    void Apply(GameObject target);
    void Remove(GameObject target);
}

[System.Serializable]
public class SlowEffect : IStatusEffect
{
    public float slowFactor = 0.5f;
    public float duration = 3f;

    public void Apply(GameObject target)
    {
        var movement = target.GetComponent<IEffectable>();
        if (movement != null)
        {
            movement.ApplySlow(slowFactor, duration);
        }
    }

    public void Remove(GameObject target)
    {
        // Автоматически снимается через EnemyMovement
    }
}

[System.Serializable]
public class DOTEffect : IStatusEffect
{
    public int damagePerTick = 5;
    public float tickInterval = 0.5f;
    public int totalTicks = 6;

    public void Apply(GameObject target)
    {
        var spellEffectManager = GameObject.FindFirstObjectByType<SpellEffectManager>();
        var damagable = target.GetComponent<IDamagable>();
        if (spellEffectManager != null && damagable != null)
        {
            spellEffectManager.ApplyDOT(damagePerTick, totalTicks, tickInterval, damagable);
        }
    }

    public void Remove(GameObject target)
    {
        // Управляется через SpellEffectManager
    }
}