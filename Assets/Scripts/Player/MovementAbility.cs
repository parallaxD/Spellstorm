using UnityEngine;

public abstract class MovementAbility
{
    public abstract void Action(Transform playerTransform);
}

[CreateAssetMenu(fileName = "TeleportAbility", menuName = "Abilities/Teleport")]
public class Teleport : MovementAbility
{
    //[SerializeField] private float teleportDistance = 1f;

    public override void Action(Transform playerTransform)
    {
        Player player = playerTransform.GetComponent<Player>();
        if (player != null)
        {
            //Vector2 moveDirection = player.GetMoveDir();

            //Vector2 newPosition = (Vector2)playerTransform.position + moveDirection * teleportDistance;
            //playerTransform.position = newPosition;

        }
    }
}

//[CreateAssetMenu(fileName = "MovementAbility", menuName = "Abilities/Rolling")]
//public class Rolling : MovementAbility
//{
//    [SerializeField] private float teleportDistance = 5f;

//    public override void Action()
//    {
//        Debug.Log("rolled");
//    }
//}
