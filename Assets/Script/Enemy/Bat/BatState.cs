using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class BatState : EnemyState<BatProperty, Bat, BatStateMachine, E_BatState>
    {
        public BatState(Bat owner) : base(owner) { }
    }

    public class Bat_CheckPlayer : BatState
    {
        public Bat_CheckPlayer(Bat owner) : base(owner)
        {
            _playerScan = new PlayerScan(0.5f);
        }

        private PlayerScan _playerScan;

        protected void InitPlayerScan() =>
            _playerScan.InitPlayerScan(Time.time);

        protected bool PlayerScan()
        {
            if (_playerScan.IsReady(Time.time))
                return IsPlayerInRange();

            return false;
        }

        private bool IsPlayerInRange()
        {
            var radius = GetRange(_property.Data);
            var playerLayer = LayerMask.GetMask("Player");
            return Physics.CheckSphere(_owner.transform.position, radius, playerLayer);
        }
    }
}

