using UnityEngine;

public abstract class MovementAbility
{
    public abstract void Action(Transform playerTransform);
}


public class Flip : MovementAbility
{
    [SerializeField] private float teleportDistance = 1f;
    private LayerMask obstacleLayer;
    [SerializeField] private float skinWidth = 0.05f;

    public Flip()
    {
        obstacleLayer = LayerMask.GetMask("ObstacleLayer");
    }

    public override void Action(Transform playerTransform)
    {
        PlayerMovement player = playerTransform.GetComponent<PlayerMovement>();
        if (player == null) return;

        player.animator.SetTrigger("Flip");

        Vector2 moveDirection = player.GetMoveDir().normalized;
        Vector2 startPosition = playerTransform.position;

        RaycastHit2D hit = Physics2D.Raycast(startPosition, moveDirection, teleportDistance, obstacleLayer);
        Vector2 targetPosition;

        if (hit.collider != null)
            targetPosition = hit.point - moveDirection * skinWidth;
        else
            targetPosition = startPosition + moveDirection * teleportDistance;

        playerTransform.position = targetPosition;
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
