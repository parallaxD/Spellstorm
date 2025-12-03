using UnityEngine;

public class Fanatic : EnemyBase
{
    [SerializeField] private float personalStoppingDistance = 1.2f;

    public new float PreferredStoppingDistance => personalStoppingDistance;

    protected override void UpdateAnimatorParameters(Vector2 moveDirection)
    {
        animator.SetFloat("moveX", moveDirection.x);
        animator.SetFloat("moveY", moveDirection.y);

        if (moveDirection.x != 0 || moveDirection.y != 0)
        {
            animator.SetFloat("lastMoveX", moveDirection.x);
            animator.SetFloat("lastMoveY", moveDirection.y);
        }
    }

    protected override void UpdateAttackAnimation(Vector2 attackDirection)
    {
        animator.SetFloat("lastMoveX", attackDirection.x);
        animator.SetFloat("lastMoveY", attackDirection.y);

        animator.SetTrigger("Attack");
    }

    protected override string GetAttackAnimationName(Vector2 direction)
    {
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            return direction.x > 0 ? "AttackRight" : "AttackLeft";
        }
        else
        {
            return direction.y > 0 ? "AttackUp" : "AttackDown";
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

    }
}