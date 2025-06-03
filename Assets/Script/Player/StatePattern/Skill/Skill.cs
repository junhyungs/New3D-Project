using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class Skill : PlayerAttackState, ICharacterState<Skill>
    {
        public Skill(Player player) : base(player)
        {
            _skillSystem = player.SkillSystem;
        }

        private PlayerSkillSystem _skillSystem;
        private ISkill _skill;

        public void OnStateEnter()
        {
            _skill = _skillSystem.GetSkill();

            if(_skill == null)
            {
                _stateHandler.ChangeIdleORMoveState();
                return;
            }

            _skill.Execute();
        }

        public void OnStateUpdate()
        {
            if (_skill.EndSkill)
                _stateHandler.ChangeIdleORMoveState();
            else
                _skill.OnUpdateSkill();
        }

        public void OnStateExit()
        {
            if (_skill == null)
                return;

            _skill.RemoveProjectile();
        }

        public void Fire(bool isFire)
        {
            if (_skill == null || !_skill.RequiresReload)
                return;

            _skill.Fire();
        }
    }
}

