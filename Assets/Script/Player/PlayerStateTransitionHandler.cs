using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;

namespace PlayerComponent
{
    public class PlayerStateTransitionHandler
    {
        public PlayerStateTransitionHandler(PlayerStateMachine stateMachine,
            PlayerInputHandler inputHandler)
        {
            _stateMachine = stateMachine;
            RegisterEvent(inputHandler);
        }

        private PlayerStateMachine _stateMachine;
        private List<Action> _unregisterActions = new List<Action>();

        public Vector2 MoveVector { get; private set; }

        private void RegisterEvent(PlayerInputHandler handler)
        {
            handler.MoveEvent += SetVector2;
            handler.RollEvent += ToRollState;
            handler.RollSlashEvent += ToRollSlashState;

            _unregisterActions.Add(() => handler.MoveEvent -= SetVector2);
            _unregisterActions.Add(() => handler.RollEvent -= ToRollState);
            _unregisterActions.Add(() => handler.RollSlashEvent -= ToRollSlashState);
        }

        public void UnRegisterEvent()
        {
            foreach(var action in _unregisterActions)
            {
                action?.Invoke();
            }

            _unregisterActions.Clear();
        }

        public void ChangeState(E_PlayerState state)
        {
            _stateMachine.ChangeState(state);
        }

        public void SetVector2(Vector2 vector)
        {
            MoveVector = vector;
        }

        public void IdleToMoveState()
        {
            if (MoveVector != Vector2.zero)
                _stateMachine.ChangeState(E_PlayerState.Move);
        }

        public void IsFalling(E_PlayerState state, bool isGround)
        {
            if(state == E_PlayerState.Falling && !isGround)
                ChangeState(E_PlayerState.Falling);
            else if(state == E_PlayerState.Idle && isGround)
                ChangeState(E_PlayerState.Idle);
        }

        public void ToRollState()
        {
            var currentState = _stateMachine.GetCurrentState();
            if (currentState is Roll)
                return;

            ChangeState(E_PlayerState.Roll);
        }

        public void ToRollSlashState()
        {
            var currentState = _stateMachine.GetCurrentState();
            if (currentState is RollSlash)
                return;

            ChangeState(E_PlayerState.RollSlash);
        }

        public void ToClimbingState((float lowPoint, float highPoint) ladderSize)
        {
            var currentState = _stateMachine.GetCurrentState();
            if (currentState is Climbing)
                return;

            var climbing = _stateMachine.GetState<Climbing>(E_PlayerState.Climbing);
            if (climbing != null)
                climbing.SetLadderSize(ladderSize);

            ChangeState(E_PlayerState.Climbing);
        }
    }
}

