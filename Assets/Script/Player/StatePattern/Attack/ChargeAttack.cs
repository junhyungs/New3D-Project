using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using System;

namespace PlayerComponent
{
    public class ChargeAttack : PlayerAttackState, ICharacterState<ChargeAttack>
    {
        public ChargeAttack(Player player) : base(player)
        {
            _capsuleCollider = player.GetComponent<CapsuleCollider>();
        }

        private WaitForFixedUpdate _waitForFixedUpdate;
        private CapsuleCollider _capsuleCollider;

        private readonly int _chargeAttack = Animator.StringToHash("ChargeAttack");
        private readonly int _chargeEquals = Animator.StringToHash("ChargeEquals");

        private const string _charge_max_L = "Charge_max_L";
        private const string _charge_max_R = "Charge_max_R";
        private const string _first_Slash = "First_Slash";
        private const string _second_Slash = "Second_Slash";

        public bool Pressed { get; set; }
        private bool _attackDirection = true;

        public void OnStateEnter()
        {
            SetWeaponController();

            _animator.SetBool(_chargeAttack, Pressed);

            var equals = Convert.ToInt32(_attackDirection);
            var hand = equals == 1 ?
                PlayerHand.Charge_R : PlayerHand.Charge_L;

            SwitchWeapon(hand);
            _animator.SetInteger(_chargeEquals, equals);
        }

        public void OnStateUpdate()
        {
            if (!Pressed)
            {
                var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                bool isMax = stateInfo.IsName(_charge_max_L)
                    || stateInfo.IsName(_charge_max_R);

                if (isMax)
                {
                    LookAtCursor();
                    _monobehaviour.StartCoroutine(DashMovement());
                }

                _animator.SetBool(_chargeAttack , Pressed);
            }
        }

        public void OnStateExit()
        {
            SwitchWeapon(PlayerHand.Idle);
        }

        private IEnumerator DashMovement()
        {
            yield return new WaitUntil(() =>
            {
                var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                return stateInfo.IsName(_first_Slash) || stateInfo.IsName(_second_Slash);
            });

            var speed = _constantData.DashSpeed / 2f;
            var maxDistance = 5f;
            var startPosition = _rigidbody.position;

            while (!_animator.IsInTransition(0))//상태전환이 일어나기 전까지 반복.
            {
                var currentPosition = _rigidbody.position;
                var moveDistance = Vector3.Distance(startPosition, currentPosition);

                if (moveDistance > maxDistance)
                    break;

                var dashSpeed = speed * Time.fixedDeltaTime;
                var moveVector = _playerTransform.forward * dashSpeed;

                _rigidbody.MovePosition(_rigidbody.position + moveVector);
                yield return _waitForFixedUpdate;
            }

            var endPosition = _rigidbody.position;
            FindTarget(startPosition, endPosition);

            _attackDirection = !_attackDirection;
            _stateHandler.ChangeIdleORMoveState();
        }

        private void FindTarget(Vector3 startPos, Vector3 endPos)
        {
            var weapon = GetCurrentWeapon();
            var weaponData = weapon.GetWeaponData();
            var distance = Vector3.Distance(startPos, endPos);

            var boxPosition = (startPos + endPos) / 2 + Vector3.up * 0.7f;
            var boxSize = new Vector3(weaponData.Range.x, _capsuleCollider.height, distance);
            var targetLayer = LayerMask.GetMask("Enemy");

            Collider[] colliders = Physics.OverlapBox(boxPosition, 
                boxSize / 2f, _playerTransform.rotation, targetLayer);
            if (colliders.Length <= 0)
                return;

            var damage = weaponData.Damage;
            foreach (var target in colliders)
                HitTarget(target, damage);
        }

        private void HitTarget(Collider collider, int damage)
        {
            if (!collider.TryGetComponent(out ITakeDamage iTakeDamage))
                return;

            iTakeDamage.TakeDamage(damage);
        }
    }
}

