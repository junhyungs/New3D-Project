using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using UnityEngine.Animations.Rigging;
using System;


namespace EnemyComponent
{
    public class ForestMother : ShooterEnemy<ForestMotherProperty, ForestMotherShooter>
    {
        [Header("LowerBodyRotation")]
        [SerializeField] private LowerBodyRotation _lowerBodyRotation;

        [Header("TestDataSO")]
        public ForestMotherSO _data;

        protected override ForestMotherProperty CreateProperty()
        {
            var property = new ForestMotherProperty(this);
            property.LowerBodyRotationController = _lowerBodyRotation;
            property.LowerBodyRotationController.Owner = this;
            return property;
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

