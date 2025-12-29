using UnityEngine;

public class AttackStateLock : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Включаем блокировку движения
        animator.GetComponent<PlayerMovement>().SetAttacking(true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Выключаем блокировку движения
        animator.GetComponent<PlayerMovement>().SetAttacking(false);
    }
}