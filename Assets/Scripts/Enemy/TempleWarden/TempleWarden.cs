using UnityEngine;

public class TempleWarden : EnemyBase
{
    protected override void UpdateAnimatorParameters(Vector2 moveDirection)
    {
        if (moveDirection.magnitude < 0.1f && Player != null)
        {
            moveDirection = Player.position - transform.position;
        }

        animator.SetFloat("moveX", moveDirection.x);
        animator.SetFloat("moveY", moveDirection.y);
    }

    protected override void UpdateAttackAnimation(Vector2 attackDirection)
    {
        animator.SetFloat("lastMoveX", attackDirection.x);
        animator.SetFloat("lastMoveY", attackDirection.y);
        animator.SetTrigger("Attack");
    }

    protected override string GetAttackAnimationName(Vector2 direction)
    {
        return "Attack";
    }
}
