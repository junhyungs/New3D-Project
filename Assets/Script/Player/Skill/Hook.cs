using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;

public class Hook : PlayerSkill, ISkill
{
    public Hook(PlayerAnimationEvent animationEvent) : base(animationEvent)
    {
        RequiresReload = false;
    }

    public override void Fire()
    {
        throw new System.NotImplementedException();
    }

    public override void InitializeSkill(Transform firePosition, int animatorTriggerCode)
    {
        throw new System.NotImplementedException();
    }

    public override void Reloading()
    {
        throw new System.NotImplementedException();
    }

    public override void TriggerAnimation()
    {
        throw new System.NotImplementedException();
    }
}
