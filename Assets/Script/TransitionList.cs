using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;
using System.Linq;

public abstract class TransitionList<T> where T : Enum
{
    public TransitionList(IStateController<T> state)
    {
        _transitionDictionary = new Dictionary<T, HashSet<T>>();
        _getState = state;

        Initialize();
    }

    protected Dictionary<T, HashSet<T>> _transitionDictionary;
    protected IStateController<T> _getState;
    protected abstract void Initialize();
    protected void AddDictionary(T key, params T[] states)
    {
        if (!_transitionDictionary.ContainsKey(key))
            _transitionDictionary.Add(key, states.ToHashSet());
    }

    public bool CanChange(T next)
    {
        var key = _getState.GetCurrentStateType();
        if (_transitionDictionary.TryGetValue(key, out HashSet<T> stateSet))
            return stateSet.Contains(next);

        Debug.Log("TransitionList TryGetValue Error");
        return false;
    }
    
}

namespace PlayerComponent
{
    public class PlayerTransitionList : TransitionList<E_PlayerState>
    {
        public PlayerTransitionList(IStateController<E_PlayerState> state) : base(state) { }
        
        protected override void Initialize()
        {
            AddDictionary(E_PlayerState.Idle,
                E_PlayerState.Move,
                E_PlayerState.Roll,
                E_PlayerState.RollSlash,
                E_PlayerState.Attack,
                E_PlayerState.Climbing,
                E_PlayerState.Falling,
                E_PlayerState.ChargeAttack,
                E_PlayerState.Skill);

            AddDictionary(E_PlayerState.Move,
                E_PlayerState.Idle,
                E_PlayerState.Roll,
                E_PlayerState.RollSlash,
                E_PlayerState.Attack,
                E_PlayerState.Falling,
                E_PlayerState.Climbing,
                E_PlayerState.ChargeAttack,
                E_PlayerState.Skill);

            AddDictionary(E_PlayerState.Roll,
                E_PlayerState.Idle,
                E_PlayerState.Move);

            AddDictionary(E_PlayerState.RollSlash,
                E_PlayerState.Idle,
                E_PlayerState.Move);

            AddDictionary(E_PlayerState.Falling,
                E_PlayerState.Idle,
                E_PlayerState.Move);

            AddDictionary(E_PlayerState.Climbing,
                E_PlayerState.Idle,
                E_PlayerState.Move);

            AddDictionary(E_PlayerState.Attack,
                E_PlayerState.Idle,
                E_PlayerState.Move,
                E_PlayerState.Roll,
                E_PlayerState.Attack);

            AddDictionary(E_PlayerState.ChargeAttack,
                E_PlayerState.Idle,
                E_PlayerState.Move,
                E_PlayerState.Roll);

            AddDictionary(E_PlayerState.Skill,
                E_PlayerState.Idle,
                E_PlayerState.Move,
                E_PlayerState.Roll);
        }
    }
}
