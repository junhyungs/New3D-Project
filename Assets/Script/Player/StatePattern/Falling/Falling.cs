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
        CheckGround();
    }

    public void OnStateExit()
    {
        _animator.SetBool(_falling, false);
    }

    protected override void CheckGround()
    {
        var origin = _playerTransform.position + Vector3.up * 0.1f;

        bool isGround = Physics.Raycast(origin, Vector3.down, RAYDISTANCE, _ground);
        if (isGround)
            _stateHandler.ChangeState(E_PlayerState.Idle);
    }
}
