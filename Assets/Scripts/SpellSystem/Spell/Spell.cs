using System.Collections;
using UnityEngine;

public abstract class Spell
{
    public SpellData Data { get; private set; } = new SpellData();

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

public class Fireball : Spell
{
    public Fireball(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var fireballProjectile = FireballProjectile.Create();
        fireballProjectile.Launch(direction);
    }
}

public class Waterball : Spell
{
    public Waterball(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var waterballProjectile = WaterballProjectile.Create();
        waterballProjectile.Launch(direction);
    }
}

public class Fire : Spell
{
    public Fire(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var fireballProjectile = FireballProjectile.Create();
        fireballProjectile.Launch(direction);
    }
}

public class FireEarth : Spell
{
    public FireEarth(SpellData data) : base(data) { }

    protected override void LaunchProjectile(Vector3 direction)
    {
        var fireEarth = FireEarthProjectile.Create();
        fireEarth.Launch(direction);
    }
}