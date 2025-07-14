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
            _mage.StartCoroutine(CheckTarget());
        }

        private IEnumerator CheckTarget()
        {
            _targetLayer = LayerMask.GetMask("Player");
            var range = GetRange();
            while (true)
            {
                bool check = Physics.CheckSphere(_mageTransform.position, range, _targetLayer);
                if (check)
                {
                    break;
                }

                yield return _intervalTime;
            }

            _stateMachine.ChangeState(E_MageState.Move);
        }
    }
}

