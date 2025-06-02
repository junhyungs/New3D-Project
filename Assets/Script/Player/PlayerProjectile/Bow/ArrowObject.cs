using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PlayerComponent
{
    public class ArrowObject : PlayerProjectile
    {
        protected override void Awake()
        {
            base.Awake();
            _objectKey = ObjectKey.PlayerArrowPrefab;
        }
    }
}

