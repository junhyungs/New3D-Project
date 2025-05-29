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

        public void OnStateEnter()
        {
            var skill = _skillSystem.GetSkill();
            skill.TriggerAnimation();
        }

        public void OnStateUpdate()
        {

        }

        public void OnStateFixedUpdate()
        {

        }

        public void OnStateExit()
        {

        }

        public void Fire(bool isFire)
        {
            

        }
    }
}

