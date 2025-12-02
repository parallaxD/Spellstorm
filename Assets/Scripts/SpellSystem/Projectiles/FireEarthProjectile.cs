using UnityEngine;

public class FireEarthProjectile : Projectile
{
    public static FireEarthProjectile Create(int spellDamage = 30)
    {
        var projectile = Projectile.Create<FireEarthProjectile>();
        return projectile;
    }
}
