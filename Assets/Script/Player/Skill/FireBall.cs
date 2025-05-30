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
        base.Fire();
    }

    public override void InitializeSkill(Transform firePosition, int animatorTriggerCode)
    {
        base.InitializeSkill(firePosition, animatorTriggerCode);
    }

    public override void Reloading()
    {
        
    }

    public override void Execute()
    {
        base .Execute();
    }
}
