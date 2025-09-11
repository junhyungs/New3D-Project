using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class ForestMotherState : EnemyState<ForestMotherProperty, ForestMother,
        ForestMotherStateMachine, E_ForestMotherState>
    {
        public ForestMotherState(ForestMother owner) : base(owner) { }
    }

    public interface IPattern
    {
        void Start() { }
        void Update() { }
        void Exit() { }
        bool IsRunning { get; }
    }
}

