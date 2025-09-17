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
        [Header("TestDataSO")]
        public ForestMotherSO _data;
        [Header("VineComponent")]
        [SerializeField] private Vine[] _vines;
        public Vine[] Vines => _vines;

        protected override void MaterialSetting()
        {
            var bodyMaterial = InstantiateMaterial();
            bodyMaterial.name = "BodyMaterial";
            Property.CopyMaterial = bodyMaterial;

            foreach(var vine in _vines)
            {
                var vineMaterial = Instantiate(_originalMaterial);
                vineMaterial.name = "VineMaterial";
                var materials = new Material[] { bodyMaterial, vineMaterial };
                vine.InitVine(materials, Property.Data);
            }
        }

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

