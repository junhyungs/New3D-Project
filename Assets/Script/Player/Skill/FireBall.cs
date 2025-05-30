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
    
    public override void Reloading()
    {
        
    }

}
