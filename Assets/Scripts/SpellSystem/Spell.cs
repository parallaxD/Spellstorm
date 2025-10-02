using UnityEngine;
using System.Collections.Generic;
public abstract class Spell
{
    public int Cooldown { get; protected set; }
    public abstract void Action();

}

public class Fireball : Spell
{
    public Fireball()
    {
        Cooldown = 3;
    }
    public override void Action()
    {
        Debug.Log("Fireball casted!");
    }
}

public class Waterball : Spell
{
    public Waterball()
    {
        Cooldown = 3;
    }
    public override void Action()
    {
        Debug.Log("Waterball casted!");
    }
}
