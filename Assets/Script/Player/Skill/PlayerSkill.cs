using EnumCollection;
using PlayerComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerSkill : ISkill
{
    public PlayerSkill(PlayerAnimationEvent animationEvent)
    {
        _animationEvent = animationEvent;
    }

    protected PlayerAnimationEvent _animationEvent;

    public bool RequiresReload { get; set; }
    public abstract void InitializeSkill(Transform firePosition, int animatorTriggerCode);
    public abstract void TriggerAnimation();
    public abstract void Reloading();
    public abstract void Fire();
}
