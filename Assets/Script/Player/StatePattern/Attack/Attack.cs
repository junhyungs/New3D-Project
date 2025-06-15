using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PlayerComponent
{
    public class Attack : PlayerAttackState, ICharacterState<Attack>, IAttackStateEventReceiver
    {
        public Attack(Player player) : base(player)
        {
            var attackStateBehaviours = _animator.GetBehaviours<AttackStateBehaviour>();
            foreach (var behaviour in attackStateBehaviours)
                behaviour.IAttack = this;

            _stateBehaviours = attackStateBehaviours;
        }
        
        private readonly int _attack = Animator.StringToHash("Attack");
        private readonly int _isAttack = Animator.StringToHash("IsAttack");
        private int _comboCount;

        public float NextCombo { get; set; } = 0.4f;
        private float _lastClickTime;
        private float _changeTime;
        private float _moveTimer;

        private bool _animationMovement;
        private bool _isNextClick;
        private bool _noInputTransition;

        private Vector3 _movePos;
        private AttackStateBehaviour[] _stateBehaviours;
        private Coroutine _transitionCoroutine;

        public void OnStateEnter()
        {
            var iWeapon = WeaponManager.Instance.CurrentWeapon;
            if(iWeapon != null)
            {
                var weaponController = iWeapon.GetWeaponController();
                foreach (var behaviour in _stateBehaviours)
                    behaviour.WeaponController = weaponController;
            }

            _animator.SetTrigger(_attack);
            _animator.SetBool(_isAttack, true);

            LookAtCursor();
            _lastClickTime = Time.time;            
        }

        public void OnStateFixedUpdate()
        {
            if (!_animationMovement)
                return;

            _moveTimer += Time.fixedDeltaTime;
            if (_moveTimer >= 0.2f)
                return;

            var moveVector = _movePos * _constantData.DashSpeed * Time.fixedDeltaTime;
            _rigidbody.MovePosition(_rigidbody.position + moveVector);
        }

        private IEnumerator Reservation()
        {
            _animator.SetBool(_isAttack, false);

            yield return new WaitWhile(() =>
            {
                var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            
                return stateInfo.IsTag("Combo");
            });
            
            _stateHandler.ChangeIdleORMoveState();
            _transitionCoroutine = null;
        }

        public void OnStateUpdate()
        {
            if (_transitionCoroutine != null)
                return;

            if (_comboCount >= 3)
            {
                _transitionCoroutine = _monobehaviour.StartCoroutine(Reservation());
                return;
            }

            if (_isNextClick)
            {
                bool isNextCombo = Time.time - _lastClickTime > NextCombo;
                if (isNextCombo)
                {
                    _animator.SetTrigger(_attack);
                    LookAtCursor();

                    _lastClickTime = Time.time;
                }

                return;
            }

            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            bool noInput = stateInfo.IsTag("Combo") && stateInfo.normalizedTime >= 1f;
            if (noInput && !_noInputTransition)
            {
                _animator.SetBool(_isAttack, false);
                _transitionCoroutine = _monobehaviour.StartCoroutine(NoInputTransition());
            }    
        }
        
        private IEnumerator NoInputTransition()
        {
            yield return null;
            
            _noInputTransition = true;
            while (_animator.IsInTransition(0))
            {
                if (_isNextClick)
                {
                    _lastClickTime = Time.time;
                    _transitionCoroutine = null;
                    _noInputTransition = false;
                    _isNextClick = false;
                    _comboCount = 0;
                    _animator.SetBool(_isAttack, true);
                    _animator.Play("First_Slash", 0, 0);

                    
                    yield break;
                }

                yield return null;
            }

            _transitionCoroutine = null;
            _stateHandler.ChangeIdleORMoveState();
        }

        public void OnStateExit()
        {
            Init();
        }

        private void Init()
        {
            _noInputTransition = false;
            _isNextClick = false;
            _lastClickTime = 0f;
            _comboCount = 0;
        }

        public void SetClick(bool isClick)
        {
            _isNextClick = isClick; 
        }

        public void OnAttackAnimEnter()
        {
            AttackMovementInitialize();
            _comboCount++;
        }

        private void AttackMovementInitialize()
        {
            _movePos = _playerTransform.forward;
            _animationMovement = true;
            _moveTimer = 0f;
        }

        public void OnAttackAnimExit()
        {
            _animationMovement = false;
            _isNextClick = false;
        }
    }

    public interface IAttackStateEventReceiver
    {
        public void OnAttackAnimEnter();
        public void OnAttackAnimExit();
    }
}

