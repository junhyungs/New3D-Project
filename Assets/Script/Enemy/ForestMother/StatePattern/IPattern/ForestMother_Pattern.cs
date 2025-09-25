using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using EnumCollection;

namespace EnemyComponent
{
    public abstract class ForestMother_Pattern : IPattern
    {
        protected ForestMotherProperty _property;
        protected ForestMother _owner;
        protected WaitForSeconds _delay;

        public bool IsRunning { get; protected set; }
        public virtual void Init(ForestMother owner)
        {
            _owner = owner;
            _property = owner.Property;
            _delay = new WaitForSeconds(0.5f);
        }
        public virtual void Enable() { }
        public virtual void Start() { }
        public virtual void Update() { }
        public virtual void Exit() { }
        public virtual IEnumerator WaitForAnimation() { yield return null; }
        public virtual void OnTriggerEnter(Collider other) { }
        protected void PlayAnimation(MotherParameterKey key)
        {
            var animKey = key.ToString();
            _property.AnimController.PlayAnimation(animKey);
        }
        protected void SetIsTrigger(bool isTrigger) =>
            _property.CapsuleCollider.isTrigger = isTrigger;

    }
}

