using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;

namespace EnemyComponent
{
    public class ForestMotherState : EnemyState<ForestMotherProperty, ForestMother,
        ForestMotherStateMachine, E_ForestMotherState>
    {
        public ForestMotherState(ForestMother owner) : base(owner) { }
    }

    public class ForestMotherPatternManager
    {
        public ForestMotherPatternManager(ForestMother owner)
        {
            _patternArray = new MotherPattern[]
            {
                //MotherPattern.Slam,
                //MotherPattern.Slam,
                //MotherPattern.Slam_Slow,
                //MotherPattern.Hyper,
                MotherPattern.Lift
            };

            _patternDictionary = new Dictionary<MotherPattern, IPattern>();

            foreach(var item in _patternArray)
            {
                var pattern = CreatePattern(item);
                pattern.Init(owner);

                if(!_patternDictionary.ContainsKey(item))
                    _patternDictionary.Add(item, pattern);
            }
        }

        private Dictionary<MotherPattern, IPattern> _patternDictionary;
        private MotherPattern[] _patternArray;
        private int _index;

        public IPattern GetPattern
        {
            get
            {
                var next = _patternArray[_index];
                _index = (_index + 1) % _patternArray.Length;

                if (!_patternDictionary.ContainsKey(next))
                {
                    var pattern = CreatePattern(next);
                    _patternDictionary[next] = pattern;
                }

                return _patternDictionary[next];
            }
        }

        public void Enable()
        {
            _index = 0;
            foreach (var item in _patternDictionary.Values)
                item.Enable();
        }

        private IPattern CreatePattern(MotherPattern pattern)
        {
            switch (pattern)
            {
                case MotherPattern.Slam:
                    return new Slam();
                case MotherPattern.Slam_Slow:
                    return new SlamSlow();
                case MotherPattern.Hyper:
                    return new Hyper();
                case MotherPattern.Lift:
                    return new Lift();
                default:
                    throw new ArgumentException();
            }
        }
    }
}

