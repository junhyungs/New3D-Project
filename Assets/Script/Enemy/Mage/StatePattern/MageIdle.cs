using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class MageIdle : MageState, ICharacterState<MageIdle>
    {
        public MageIdle(Mage mage) : base(mage) { }
        private WaitForSeconds _intervalTime = new WaitForSeconds(0.5f);
        private LayerMask _targetLayer;

        public void OnStateEnter()
        {
            if (_property == null)
                Debug.Log("Property Null");
            else if (_property.Owner == null)
                Debug.Log("Owner Null");
                _property.Owner.StartCoroutine(CheckTarget());
        }

        private IEnumerator CheckTarget()
        {
            _targetLayer = LayerMask.GetMask("Player");
            var range = GetRange();
            while (true)
            {
                bool check = Physics.CheckSphere(_property.Owner.transform.position, range, _targetLayer);
                if (check)
                {
                    break;
                }

                yield return _intervalTime;
            }

            _property.StateMachine.ChangeState(E_MageState.Teleport);
        }
    }
}

