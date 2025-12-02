using UnityEngine;

public class WaterballProjectile : Projectile
{
    [Header("Waterball Specific")]
    [SerializeField] private float waterSlowFactor = 0.3f;
    [SerializeField] private float waterSlowDuration = 4f;

    public static WaterballProjectile Create(int spellDamage = 30)
    {
        var projectile = Projectile.Create<WaterballProjectile>();
        projectile.applySlow = true;
        projectile.slowFactor = projectile.waterSlowFactor;
        projectile.slowDuration = projectile.waterSlowDuration;
        return projectile;
    }

    protected override Color GetExplosionColor()
    {
        return new Color(0f, 0.5f, 1f);
    }
}