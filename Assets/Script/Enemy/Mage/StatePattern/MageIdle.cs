using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class MageIdle : MageState, ICharacterState<MageIdle>
    {
        public MageIdle(Mage mage) : base(mage) { }

        private float _nextScanTime;
        private const float PLAYER_SCAN_INTERVAL = 0.5f;

        public void OnStateEnter()
        {
            _nextScanTime = Time.time + PLAYER_SCAN_INTERVAL;
        }

        public void OnStateUpdate()
        {
            if(Time.time >= _nextScanTime)
            {
                _nextScanTime = Time.time + PLAYER_SCAN_INTERVAL;
                if (IsPlayerRange())
                    _property.StateMachine.ChangeState(E_MageState.Teleport);
            }
        }

        private bool IsPlayerRange()
        {
            var radius = GetRange(_property.Data);
            var playerLayer = LayerMask.GetMask("Player");
            return Physics.CheckSphere(_owner.transform.position, radius, playerLayer);
        }
    }
}

