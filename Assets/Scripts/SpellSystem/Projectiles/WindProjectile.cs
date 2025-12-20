using UnityEngine;

public class WindProjectile : Projectile
{
    public static WindProjectile Create(int spellDamage = 30)
    {
        var projectile = Projectile.Create<WindProjectile>();
        return projectile;
    }
}
