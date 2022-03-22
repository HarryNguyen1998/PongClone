using UnityEngine;
using UnityEngine.Events;

public class SBToGameState : StateMachineBehaviour
{
    public UnityEvent EnterEvent;
    public UnityEvent ExitEvent;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        EnterEvent?.Invoke();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        ExitEvent?.Invoke();
    }
}
