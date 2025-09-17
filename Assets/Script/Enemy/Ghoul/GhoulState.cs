using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EnemyComponent
{
    public class GhoulState : EnemyState<GhoulProperty, Ghoul, GhoulStateMachine, E_GhoulState>
    {
        public GhoulState(Ghoul owner) : base(owner) { }
    }

    public class Ghoul_CheckPlayer : GhoulState
    {
        public Ghoul_CheckPlayer(Ghoul owner) : base(owner)
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

