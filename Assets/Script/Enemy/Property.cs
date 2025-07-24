using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EnumCollection;

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

    public interface IStateMachine<TStateMachine>
    {
        TStateMachine StateMachine { get; }
    }

    public interface IDataProvider<T> where T : EnemyDataSO
    {
        T Data { get; }
    }

    public abstract class EnemyProperty<T>
        where T : MonoBehaviour
    {
        protected EnemyProperty(T owner)
        {
            Owner = owner;
            InitializeProperty(owner);
        }

        public T Owner { get; private set; }
        protected abstract void InitializeProperty(T owner);
        protected TDataSO GetData<TDataSO>(ScriptableDataKey key) where TDataSO : EnemyDataSO
        {
            return DataManager.Instance.GetScriptableData(key) as TDataSO;
        }
    }

    public class MageProperty : EnemyProperty<Mage>, IDataProvider<MageSO>, IStateMachine<MageStateMachine>, IPropertyBase
    {
        public MageProperty(Mage reference) : base(reference) { }
        
        public MageStateMachine StateMachine{ get; private set; }
        public NavMeshAgent NavMeshAgent { get; private set; }
        public Animator Animator { get; private set; }
        public Material CopyMaterial { get; set; }
        public MageSO Data { get; private set; }
        public bool IsSpawn { get; set; }
        public int Health { get; set; }

        protected override void InitializeProperty(Mage reference)
        {
            StateMachine = reference.GetComponent<MageStateMachine>();
            NavMeshAgent = reference.GetComponent<NavMeshAgent>();
            Animator = reference.GetComponent<Animator>();
            Data = reference._testData; //TODO 완성되면 GetData로 변경.

            NavMeshAgent.speed = Data.Speed;
            NavMeshAgent.stoppingDistance = Data.AgentStopDistance;

            Health = Data.Health;
        }
    }




}

