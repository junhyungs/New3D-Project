using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class ForestMother : ShooterEnemy<ForestMotherProperty, ForestMotherShooter>
    {
        [Header("TestDataSO")]
        public ForestMotherSO _data;

        protected override ForestMotherProperty CreateProperty()
        {
            return new ForestMotherProperty(this);
        }

        protected override void Death()
        {
            Property.StateMachine.ChangeState(E_ForestMotherState.Death);
        }

        protected override int GetDamage()
        {
            return Property.Data.Damage;
        }
    }
}

