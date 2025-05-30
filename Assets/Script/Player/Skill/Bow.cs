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
   
    public override void Reloading()
    {
        //TODO 오브젝트 풀에서 투사체를 가져와서 fireTransform의 자식으로 설정.
    }
}
