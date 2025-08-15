using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace EnemyComponent
{
    public class BatAnimationEvent : MonoBehaviour
    {
        public event Action AttackEvent;

        public void InvokeAttack()
        {
            AttackEvent?.Invoke();
        }
    }
}

