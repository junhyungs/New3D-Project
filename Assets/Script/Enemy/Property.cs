using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnumCollection;
using System;

namespace EnemyComponent
{
    public interface IPropertyBase
    {
        int Health { get; set; }
        NavMeshAgent NavMeshAgent { get; }
        Animator Animator { get; }
        Material CopyMaterial { get; set; }
        bool IsSpawn { get; set; }
    }

    public interface IStateMachine<TStateMachine, TEnum>
        where TStateMachine : IStateController<TEnum>
        where TEnum : Enum
    {
        TStateMachine StateMachine { get; }
    }

    public interface IDataProvider<T> where T : MonsterDataSO
    {
        T Data { get; }
    }

    public abstract class EnemyProperty<T>
        where T : MonoBehaviour
    {
        protected EnemyProperty(T owner)
        {
            InitializeProperty(owner);
        }

        protected abstract void InitializeProperty(T owner);
        protected TDataSO GetData<TDataSO>(ScriptableDataKey key) where TDataSO : MonsterDataSO
        {
            return DataManager.Instance.GetScriptableData(key) as TDataSO;
        }
    }

    public class SlimeProperty : EnemyProperty<Slime>,
        IDataProvider<SlimeSO>, IStateMachine<SlimeStateMachine, E_SlimeState>, IPropertyBase
    {
        public SlimeProperty(Slime owner) : base(owner) { }

        public SlimeSO Data { get; private set; }
        public SlimeStateMachine StateMachine { get; private set; }
        public NavMeshAgent NavMeshAgent{ get; private set; }
        public Animator Animator { get; private set; }
        public Material CopyMaterial { get; set; }
        public Transform TargetTransform { get; set; }
        public int Health { get; set; }
        public bool IsSpawn { get; set; }

        protected override void InitializeProperty(Slime owner)
        {
            StateMachine = owner.GetComponent<SlimeStateMachine>();
            NavMeshAgent = owner.GetComponent<NavMeshAgent>();
            Animator = owner.GetComponent<Animator>();
            Data = owner._slimeSO; //TODO 완성되면 GetData로 변경.

            NavMeshAgent.speed = 0;
            NavMeshAgent.acceleration = Data.Acceleration;
            NavMeshAgent.stoppingDistance = Data.AgentStopDistance;

            Health = Data.Health;
        }
    }

    public class ForestMotherProperty : EnemyProperty<ForestMother>,
        IDataProvider<ForestMotherSO>, IStateMachine<ForestMotherStateMachine, E_ForestMotherState>, IPropertyBase
    {
        public ForestMotherProperty(ForestMother owner) : base(owner) { }

        public ForestMotherSO Data { get; private set; }
        public ForestMotherStateMachine StateMachine { get; private set; }
        public ForestMotherAnimationController AnimController { get; private set; }
        public Animator Animator { get; private set; }
        public NavMeshAgent NavMeshAgent { get; private set; }
        public Material CopyMaterial { get; set; }
        public IPattern CurrentPattern { get; set; }
        public int VineHealth { get; set; }
        public float LiftTime { get; set; }       
        public int Health { get; set; }
        public bool IsSpawn { get; set; }

        protected override void InitializeProperty(ForestMother owner)
        {
            StateMachine = owner.GetComponent<ForestMotherStateMachine>();
            Animator = owner.GetComponent<Animator>();
            Data = owner._data;

            NavMeshAgent = owner.GetComponent<NavMeshAgent>();
            NavMeshAgent.updateRotation = false;

            AnimController = owner.GetComponent<ForestMotherAnimationController>();

            Health = Data.Health;
            VineHealth = Data.VineHealth;

            LiftTime = Data.LiftTime;
        }
    }

    public class BatProperty : EnemyProperty<Bat>,
        IDataProvider<BatSO>, IStateMachine<BatStateMachine, E_BatState>, IPropertyBase
    {
        public BatProperty(Bat owner) : base(owner) { }

        public BatSO Data { get; private set; }
        public Transform TargetTransform { get; set; }
        public BatStateMachine StateMachine { get; private set; }
        public NavMeshAgent NavMeshAgent { get; private set; }
        public Animator Animator { get; private set; }
        public Material CopyMaterial { get; set; }
        public int Health { get; set; }
        public bool IsSpawn { get; set; }

        protected override void InitializeProperty(Bat owner)
        {
            StateMachine = owner.GetComponent<BatStateMachine>();
            NavMeshAgent = owner.GetComponent<NavMeshAgent>();
            Animator = owner.GetComponent<Animator>();
            Data = owner._batSO; //TODO 완성되면 GetData로 변경.

            NavMeshAgent.speed = Data.Speed;
            NavMeshAgent.stoppingDistance = Data.AgentStopDistance;
            Health = Data.Health;
        }
    }

    public class GhoulProperty : EnemyProperty<Ghoul>,
        IDataProvider<GhoulSO>, IStateMachine<GhoulStateMachine, E_GhoulState>, IPropertyBase
    {
        public GhoulProperty(Ghoul owner) : base(owner) { }
        
        public GhoulSO Data { get; private set; }
        public GhoulStateMachine StateMachine { get; private set; }
        public NavMeshAgent NavMeshAgent { get; private set; }
        public Animator Animator { get; private set; }
        public Material CopyMaterial { get; set; }
        public int Health { get; set; }
        public bool IsSpawn { get; set; }

        protected override void InitializeProperty(Ghoul owner)
        {
            StateMachine = owner.GetComponent<GhoulStateMachine>();
            NavMeshAgent = owner.GetComponent<NavMeshAgent>();
            Animator = owner.GetComponent<Animator>();
            Data = owner._testData; //TODO 완성되면 GetData로 변경.

            NavMeshAgent.speed = Data.Speed;
            NavMeshAgent.stoppingDistance = Data.AgentStopDistance;

            Health = Data.Health;
        }
    }

    public class MageProperty : EnemyProperty<Mage>,
        IDataProvider<MageSO>, IStateMachine<MageStateMachine, E_MageState>, IPropertyBase
    {
        public MageProperty(Mage owner) : base(owner) { }
        
        public MageStateMachine StateMachine{ get; private set; }
        public NavMeshAgent NavMeshAgent { get; private set; }
        public Animator Animator { get; private set; }
        public Material CopyMaterial { get; set; }
        public MageSO Data { get; private set; }
        public bool IsSpawn { get; set; }
        public int Health { get; set; }

        protected override void InitializeProperty(Mage owner)
        {
            StateMachine = owner.GetComponent<MageStateMachine>();
            NavMeshAgent = owner.GetComponent<NavMeshAgent>();
            Animator = owner.GetComponent<Animator>();
            Data = owner._testData; //TODO 완성되면 GetData로 변경.

            NavMeshAgent.speed = Data.Speed;
            NavMeshAgent.stoppingDistance = Data.AgentStopDistance;

            Health = Data.Health;
        }
    }




}

