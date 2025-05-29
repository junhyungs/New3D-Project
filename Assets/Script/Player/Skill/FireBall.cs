using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;

public class FireBall : PlayerSkill, ISkill
{
    public FireBall(PlayerAnimationEvent animationEvent) : base(animationEvent)
    {
        RequiresReload = true;
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
