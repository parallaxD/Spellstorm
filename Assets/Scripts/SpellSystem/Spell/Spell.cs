using System.Collections;
using UnityEngine;

public abstract class Spell
{
    public SpellData Data { get; private set; } = ScriptableObject.CreateInstance<SpellData>();

    public Spell(SpellData data)
    {
        Data.Name = data.name;
        Data.ID = data.ID;
        Data.Cooldown = data.Cooldown;
        if (Data.Sprite != null) Data.Sprite = data.Sprite;
        Data.Receipt = data.Receipt;
    }

    public virtual void Action()
    {
        Vector3 direction = GetShootDirection();
        LaunchProjectile(direction);
    }

    protected virtual Vector3 GetShootDirection()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Mathf.Abs(Constants.MainCamera.transform.position.z);
        Vector3 mouseWorldPosition = Constants.MainCamera.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0;
        return (mouseWorldPosition - Constants.PlayerTransform.position).normalized;
    }

    protected abstract void LaunchProjectile(Vector3 direction);
}

public class Water : Spell
{
    public Water(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction) 
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt, applySlow: true, slowDuration: 3, slowFactor: 0.5f);
        Projectile.Launch(direction); 
    }
}

public class Fire : Spell
{
    public Fire(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt, fireDOTDamage: 10, fireDOTTicks: 3, fireDOTInterval: 1, aoeRadiusAmount: 0.5f, applySlow: true);
        Projectile.Launch(direction);
    }
}

public class Earth : Spell
{
    public Earth(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt, applyKnockback: true, knockbackForce: 5, knockbackOnlyOnDirectHit: true, damageAmount: 10);
        Projectile.Launch(direction);
    }
}

public class Wind : Spell
{
    public Wind(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class FireFire : Spell
{
    public FireFire(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class FireWater : Spell
{
    public FireWater(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class FireEarth : Spell
{
    public FireEarth(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class FireWind : Spell
{
    public FireWind(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class WaterWater : Spell
{
    public WaterWater(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class WaterEarth : Spell
{
    public WaterEarth(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class WaterWind : Spell
{
    public WaterWind(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class EarthEarth : Spell
{
    public EarthEarth(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class EarthWind: Spell
{
    public EarthWind(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class WindWind : Spell
{
    public WindWind(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class FireFireFire : Spell
{
    public FireFireFire(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class FireFireWater : Spell
{
    public FireFireWater(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class FireFireEarth : Spell
{
    public FireFireEarth(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class FireFireWind : Spell
{
    public FireFireWind(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class FireWaterWater : Spell
{
    public FireWaterWater(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class FireWaterEarth : Spell
{
    public FireWaterEarth(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class FireWaterWind : Spell
{
    public FireWaterWind(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class FireEarthEarth : Spell
{
    public FireEarthEarth(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class FireEarthWind : Spell
{
    public FireEarthWind(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class FireWindWind : Spell
{
    public FireWindWind(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class WaterWaterWater : Spell
{
    public WaterWaterWater(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class WaterWaterEarth : Spell
{
    public WaterWaterEarth(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class WaterWaterWind : Spell
{
    public WaterWaterWind(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class WaterEarthEarth : Spell
{
    public WaterEarthEarth(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class WaterEarthWind : Spell
{
    public WaterEarthWind(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class WaterWindWind : Spell
{
    public WaterWindWind(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}


public class EarthEarthEarth : Spell
{
    public EarthEarthEarth(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class EarthEarthWind : Spell
{
    public EarthEarthWind(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class EarthWindWind : Spell
{
    public EarthWindWind(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}

public class WindWindWind : Spell
{
    public WindWindWind(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var Projectile = SpellProjectile.Create(Constants.PlayerTransform.position, Data.Receipt);
        Projectile.Launch(direction);
    }
}
