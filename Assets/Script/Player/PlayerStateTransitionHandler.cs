using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;
using System.Linq;

namespace PlayerComponent
{
    public class PlayerStateTransitionHandler
    {
        public PlayerStateTransitionHandler(PlayerStateMachine stateMachine,
            PlayerInputHandler inputHandler)
        {
            _stateMachine = stateMachine;
            _transitionList = new PlayerTransitionList(_stateMachine);

            RegisterEvent(inputHandler);
        }

        private PlayerStateMachine _stateMachine;
        private List<Action> _unregisterActions = new List<Action>();
        private PlayerTransitionList _transitionList;

        public Vector2 MoveVector { get; private set; }

        private void RegisterEvent(PlayerInputHandler handler)
        {
            handler.MoveEvent += SetVector2;
            handler.RollEvent += ToRollState;
            handler.RollSlashEvent += ToRollSlashState;
            handler.SlashEvent += ToAttackState;
            handler.ChargeSlashEvent += ToChargeAttackState;

            _unregisterActions.Add(() => handler.MoveEvent -= SetVector2);
            _unregisterActions.Add(() => handler.RollEvent -= ToRollState);
            _unregisterActions.Add(() => handler.RollSlashEvent -= ToRollSlashState);
            _unregisterActions.Add(() => handler.SlashEvent -= ToAttackState);
            _unregisterActions.Add(() => handler.ChargeSlashEvent -= ToChargeAttackState);
        }

        public void UnRegisterEvent()
        {
            foreach(var action in _unregisterActions)
            {
                action?.Invoke();
            }

            _unregisterActions.Clear();
        }

        public void SetVector2(Vector2 vector)
        {
            MoveVector = vector;
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
            if (EqualsCurrentState<Roll>())
                return;
            
            ChangeState(E_PlayerState.Roll);
        }

        public void ToRollSlashState()
        {
            if (EqualsCurrentState<RollSlash>())
                return;

            ChangeState(E_PlayerState.RollSlash);
        }

        public void ToClimbingState((float lowPoint, float highPoint) ladderSize)
        {
            if (EqualsCurrentState<Climbing>())
                return;

            var climbing = _stateMachine.GetState(E_PlayerState.Climbing) as Climbing;
            if (climbing != null)
                climbing.SetLadderSize(ladderSize);

            ChangeState(E_PlayerState.Climbing);
        }

        public void ToAttackState()
        {
            if (EqualsCurrentState<Attack>())
            {
                var attack = _stateMachine.GetState(E_PlayerState.Attack) as Attack;
                if(attack != null) //TODO 광클 시 부하가 있을 수 있으므로 나중에 캐싱하는 방향으로.
                    attack.SetClick(true);

                return;
            }

            ChangeState(E_PlayerState.Attack); 
        }

        public void ToChargeAttackState(bool pressed)
        {
            if (pressed)
            {
                var state = _stateMachine.GetState(E_PlayerState.ChargeAttack);
                if (state is ChargeAttack chargeAttack)
                {
                    chargeAttack.Pressed = pressed;
                    ChangeState(E_PlayerState.ChargeAttack);
                }
            }
            else
            {
                var currentState = _stateMachine.GetCurrentState();
                if (currentState is ChargeAttack chargeAttack)
                {
                    chargeAttack.Pressed = pressed;
                }
            }
        }

        private bool EqualsCurrentState<T>() where T : PlayerState
        {
            var currentState = _stateMachine.GetCurrentState();
            if (currentState is T)
                return true;

            return false;
        }

        private void ChangeState(E_PlayerState state)
        {
            if (!_transitionList.CanChange(state))
                return;

            _stateMachine.ChangeState(state);
        }
    }
}

