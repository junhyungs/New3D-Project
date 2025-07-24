using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class MageState
    {
        public MageState(Mage mage)
        {
            _property = mage.Property;
        }

        protected MageProperty _property;
        protected Transform _mageTransform;
        protected Transform _targetTransform;
        protected const string PROPERTYNAME = "_NoiseValue";

        protected float GetRange()
        {
            var data = _property.Data;
            return _property.IsSpawn ? 
                data.Spawn_DetectionRange : data.DetectionRange;
        }

        protected Transform FindTarget()
        {
            var targetLayer = LayerMask.GetMask("Player");
            var range = GetRange(); 
            var results = new Collider[1];

            var count = Physics.OverlapSphereNonAlloc(_property.Owner.transform.position,
                range, results, targetLayer);
            if(count > 0)
            {
                var targetTransform = results[0].transform;
                return targetTransform;
            }
            else
                return null;
        }
    }
}
