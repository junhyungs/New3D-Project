using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;
using EnumCollection;

public class Falling : PlayerMoveState, ICharacterState<Falling>
{
    public Falling(Player player) : base(player) { }

    private readonly int _falling = Animator.StringToHash("IsFalling");

    public void OnStateEnter()
    {
        _animator.SetBool(_falling, true);
    }

    public void OnStateFixedUpdate()
    {
        IsFalling(E_PlayerState.Idle);
    }

    public void OnStateExit()
    {
        _animator.SetBool(_falling, false);
    }
}
