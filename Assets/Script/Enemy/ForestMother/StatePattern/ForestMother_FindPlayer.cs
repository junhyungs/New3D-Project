using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class ForestMother_FindPlayer : ForestMotherState, ICharacterState<ForestMother_FindPlayer>
    {
        public ForestMother_FindPlayer(ForestMother owner) : base(owner)
        {
            _playerScan = new PlayerScan(0.5f);
        }

        private PlayerScan _playerScan;

        public void OnStateEnter()
        {
            _playerScan.InitPlayerScan(Time.time);
        }

        public void OnStateUpdate()
        {
            if (_playerScan.IsReady(Time.time))
            {
                var radius = GetRange(_property.Data);
                var playerLayer = LayerMask.GetMask("Player");

                bool check = Physics.CheckSphere(_owner.transform.position,
                    radius, playerLayer);
                if (check)
                    _property.StateMachine.ChangeState(E_ForestMotherState.ChangePattern);
            }
        }
    }
}

