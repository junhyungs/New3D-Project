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
        //TODO ������Ʈ Ǯ���� ����ü�� �����ͼ� fireTransform�� �ڽ����� ����.
    }
}
