using PlayerComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RollSlashStateBehaviour : StateMachineBehaviour
{
    public RollSlash RollSlashState;

    private const string Roll_slash = "Roll_slash";
    private const string Roll_slash_end = "Roll_slash_end";

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName(Roll_slash))
        {
            RollSlashState.IsRoll = true;
            RollSlashState.AnimationStop = false;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (stateInfo.IsName(Roll_slash))
            RollSlashState.IsRoll = false;
        else if (stateInfo.IsName(Roll_slash_end))
            RollSlashState.AnimationStop = true;
    }
}
