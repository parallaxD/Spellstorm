using UnityEngine;

public class EarthProjectile : Projectile
{
    public static EarthProjectile Create(int spellDamage = 30)
    {
        var projectile = Projectile.Create<EarthProjectile>();
        return projectile;
    }
}
