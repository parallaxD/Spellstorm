using UnityEngine;

public abstract class MovementAbility : ScriptableObject
{
    public abstract void Action();
}

[CreateAssetMenu(fileName = "MovementAbility", menuName = "Abilities/Teleport")]
public class Teleport : MovementAbility
{
    [SerializeField] private float teleportDistance = 5f;

    public override void Action()
    {
        Debug.Log("teleported");
    }
}

[CreateAssetMenu(fileName = "MovementAbility", menuName = "Abilities/Rolling")]
public class Rolling : MovementAbility
{
    [SerializeField] private float teleportDistance = 5f;

    public override void Action()
    {
        Debug.Log("rolled");
    }
}
