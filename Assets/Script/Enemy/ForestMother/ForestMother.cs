using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using UnityEngine.Animations.Rigging;
using System;
using EventClass;

namespace EnemyComponent
{
    public class ForestMother : ShooterEnemy<ForestMotherProperty, ForestMotherShooter>
    {
        [Header("TestDataSO")]
        public ForestMotherSO _data;

        [Header("VineComponent")]
        [SerializeField] private Vine[] _vines;
        [Header("TimeLine")]
        [SerializeField] private TimeLineComponent.TimeLine _timeLine;        
        public Vine[] Vines => _vines;

        protected override void OnEnableEnemy()
        {
            GameEventManager.Instance.StartEvent(
                new BossEvent<ForestMother, Level_1_progress>(this)
                );
        }

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
            var gameManager = GameManager.Instance;
            if (!gameManager.PlayerDeath && _timeLine != null)
            {
                GameEventManager.Instance.CompleteEvent(GameEvent.ForestMotherBoss);
                _timeLine.PlayTimeLine();
            }

            Property.StateMachine.ChangeState(E_ForestMotherState.Death);
        }

        protected override int GetDamage()
        {
            return Property.Data.Damage;
        }
    }
}

