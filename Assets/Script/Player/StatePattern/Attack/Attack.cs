using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace PlayerComponent
{
    public class Attack : PlayerAttackState, ICharacterState<Attack>,
        IAttackStateEventReceiver, IEnableObject
    {
        public Attack(Player player) : base(player)
        {
            InitializeBehaviour();
        }

        private readonly int _attackHash = Animator.StringToHash("Attack");
        private readonly int _isAttackHash = Animator.StringToHash("IsAttack");

        private const string TAG_COMBO = "Combo";

        private int _comboCount;
        private float _lastClickTime;
        private float _moveTimer;
        private const float DEACTIVETIME = 0.45f;

        private bool _enableMovement;
        private bool _isNextClick;
        private bool _isTransitioning;

        private Vector3 _moveDirection;
        private Coroutine _transitionCoroutine;

        public float NextComboDelay { get; set; } = 0.4f;

        public void OnStateEnter()
        {
            SetWeaponController();

            _animator.SetTrigger(_attackHash);
            _animator.SetBool(_isAttackHash, true);

            LookAtCursor();
            _lastClickTime = Time.time;            
        }

        public void OnStateFixedUpdate()
        {
            if (!_enableMovement)
                return;

            _moveTimer += Time.fixedDeltaTime;
            if (_moveTimer >= 0.2f)
                return;

            var movement = _moveDirection * _constantData.DashSpeed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(_rigidbody.position + movement);
        }

        public void OnStateUpdate()
        {
            if (_transitionCoroutine != null)
                return;

            if (_comboCount >= 3)
            {
                _transitionCoroutine = _monobehaviour.StartCoroutine(EndComboAfterAnimation());
                return;
            }

            bool comboDelay = Time.time - _lastClickTime > NextComboDelay;
            if (_isNextClick && comboDelay)
            {
                _animator.SetTrigger(_attackHash);
                LookAtCursor();
                _lastClickTime = Time.time;

                return;
            }

            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            bool finishedCombo = stateInfo.IsTag(TAG_COMBO) && stateInfo.normalizedTime >= 1f;
            if (finishedCombo && !_isTransitioning)
            {
                _animator.SetBool(_isAttackHash, false);
                _transitionCoroutine = _monobehaviour.StartCoroutine(WaitForNextComboORTransition());
            }    
        }
        
        public void OnStateExit()
        {
            ResetState();
            _weaponObjectController.SetWeaponActive(PlayerHand.Idle);
        }

        private void InitializeBehaviour()
        {
            var attackStateBehaviours = _animator.GetBehaviours<AttackStateBehaviour>();
            foreach (var behaviour in attackStateBehaviours)
                behaviour.IAttack = this;
        }

        private IEnumerator EndComboAfterAnimation()
        {
            _animator.SetBool(_isAttackHash, false);

            yield return new WaitWhile(() =>
            {
                var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                return stateInfo.IsTag(TAG_COMBO);
            });

            _stateHandler.ChangeIdleORMoveState();
            _transitionCoroutine = null;
        }

        private IEnumerator WaitForNextComboORTransition()
        {
            yield return null;
            _isTransitioning = true;

            while (_animator.IsInTransition(0))
            {
                if (_isNextClick)
                {
                    var nextAnim = NextComboName();
                    if (nextAnim != null)
                    {
                        _isNextClick = false;
                        _lastClickTime = Time.time;
                        _isTransitioning = false;
                        _transitionCoroutine = null;

                        _animator.SetBool(_isAttackHash, true);
                        _animator.Play(nextAnim, 0, 0);
                        yield break;
                    }
                }

                yield return null;
            }

            _transitionCoroutine = null;
            _stateHandler.ChangeIdleORMoveState();
        }

        private string NextComboName()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.IsName("First_Slash"))
                return "Second_Slash";
            if (stateInfo.IsName("Second_Slash"))
                return "Third_Slash";

            return null;
        }

        private void ResetState()
        {
            _isTransitioning = false;
            _isNextClick = false;
            _lastClickTime = 0f;
            _comboCount = 0;
        }

        public void SetClick(bool isClick)
        {
            _isNextClick = isClick; 
        }

        public void OnAttackAnimEnter(ref bool isDeactive,PlayerHand playerHand)
        {
            StartComboMovement();

            isDeactive = false;
            SwitchWeapon(playerHand);

            _comboCount++;
        }

        private void StartComboMovement()
        {
            _moveDirection = _playerTransform.forward;
            _enableMovement = true;
            _moveTimer = 0f;
        }

        public void OnAttackAnimUpdate(ref bool isDeactive, AnimatorStateInfo stateInfo)
        {
            bool deactiveCurrentWeapon = !isDeactive
            && stateInfo.normalizedTime >= DEACTIVETIME;
            if (deactiveCurrentWeapon)
            {
                isDeactive = true;
                _weaponObjectController.DeActiveCurrentWeapon();
            }
        }

        public void OnAttackAnimExit()
        {
            _enableMovement = false;
            _isNextClick = false;
        }

        public void OnEnableObject()
        {
            InitializeBehaviour();
        }
    }

    public interface IAttackStateEventReceiver
    {
        public void OnAttackAnimEnter(ref bool isDeactive, PlayerHand playerHand);
        public void OnAttackAnimUpdate(ref bool isDeactive, AnimatorStateInfo stateInfo);
        public void OnAttackAnimExit();
    }
}

