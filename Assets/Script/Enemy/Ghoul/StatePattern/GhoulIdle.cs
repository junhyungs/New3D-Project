using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using GameData;

namespace EnemyComponent
{
    public class GhoulIdle : Ghoul_CheckPlayer, ICharacterState<GhoulIdle>
    {
        public GhoulIdle(Ghoul owner) : base(owner) { }

        private const float CHANGE_TIME = 3f;
        private const float MOTION_VALUE = 0;
        private float _changeTimer;

        private readonly int _move = Animator.StringToHash("Move");
        private readonly int _idleMotion = Animator.StringToHash("IdleMotion");
        private const int MOVE_VALUE = 0;

        public void OnStateEnter()
        {
            AnimationSetting();
            _changeTimer = Time.time + CHANGE_TIME;

            InitPlayerScan();
        }

        private void AnimationSetting()
        {
            var animator = _property.Animator;
            animator.SetFloat(_idleMotion, MOTION_VALUE);
            animator.SetInteger(_move, MOVE_VALUE);
        }

        public void OnStateUpdate()
        {
            if (PlayerScan())
                _property.StateMachine.ChangeState(E_GhoulState.Trace);
            else if(Time.time >= _changeTimer)
                _property.StateMachine.ChangeState(E_GhoulState.Partrol);
        }
    }
}
