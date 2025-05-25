using PlayerComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollStateBehaviour : StateMachineBehaviour
{
    public Roll RollState;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RollState.IsRoll = true;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RollState.IsRoll = false;
    }
}
