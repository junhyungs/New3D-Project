using PlayerComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : PlayerSkill, ISkill
{
    public Bow(PlayerAnimationEvent animationEvent) : base(animationEvent)
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
