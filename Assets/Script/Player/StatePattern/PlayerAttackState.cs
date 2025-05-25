using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PlayerComponent
{
    public class PlayerAttackState : PlayerState
    {
        public PlayerAttackState(Player player) : base(player)
        {
            Initialize(player);
        }

        protected Animator _animator;
        protected Transform _playerTransform;

        private void Initialize(Player player)
        {
            _animator = player.GetComponentInChildren<Animator>();
            _playerTransform = player.transform;
        }

        private void LookAtCursor()
        {
            Vector3 lookPos = new Vector3(_plane.Point.x,
                _playerTransform.position.y, _plane.Point.z);

            var distance = Vector3.Distance(_playerTransform.position, lookPos);
            if(distance > 0.1f)
                _playerTransform.LookAt(lookPos);
        }

        protected void ClickTriggerAttack(int parameter)
        {
            LookAtCursor();
            _animator.SetTrigger(parameter);
        }
    }
}

