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
            handler.SlashEvent += ToAttackState;

            _unregisterActions.Add(() => handler.MoveEvent -= SetVector2);
            _unregisterActions.Add(() => handler.RollEvent -= ToRollState);
            _unregisterActions.Add(() => handler.RollSlashEvent -= ToRollSlashState);
            _unregisterActions.Add(() => handler.SlashEvent -= ToAttackState);
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

        public void ChangeIdleORMoveState()
        {
            var nextState = MoveVector != Vector2.zero ?
                E_PlayerState.Move : E_PlayerState.Idle;
            
            _stateMachine.ChangeState(nextState);
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
            if (!IsState<Roll>())
                return;
            
            ChangeState(E_PlayerState.Roll);
        }

        public void ToRollSlashState()
        {
            if (!IsState<RollSlash>())
                return;

            ChangeState(E_PlayerState.RollSlash);
        }

        public void ToClimbingState((float lowPoint, float highPoint) ladderSize)
        {
            if (!IsState<Climbing>())
                return;

            var climbing = _stateMachine.GetState<Climbing>(E_PlayerState.Climbing);
            if (climbing != null)
                climbing.SetLadderSize(ladderSize);

            ChangeState(E_PlayerState.Climbing);
        }

        public void ToAttackState()
        {
            if (!IsState<Attack>())
            {
                var attack = _stateMachine.GetState<Attack>(E_PlayerState.Attack);
                if(attack != null) //TODO 광클 시 부하가 있을 수 있으므로 나중에 캐싱하는 방향으로.
                    attack.SetClick(true);

                return;
            }

            ChangeState(E_PlayerState.Attack); 
        }

        private bool IsState<T>() where T : PlayerState
        {
            var currentState = _stateMachine.GetCurrentState();
            if (currentState is T)
                return false;

            return true;
        }
    }
}

