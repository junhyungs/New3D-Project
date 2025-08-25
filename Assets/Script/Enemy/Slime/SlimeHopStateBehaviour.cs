using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeHopStateBehaviour : StateMachineBehaviour
{
    public IStateBehaviourController Controller { get; set; }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Controller.OnEnter(animator, stateInfo);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Controller.OnUpdate(animator, stateInfo);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Controller.OnExit(animator, stateInfo);
    }
}
