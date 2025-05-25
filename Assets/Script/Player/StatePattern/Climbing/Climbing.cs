using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace PlayerComponent
{
    public class Climbing : PlayerMoveState, ICharacterState<Climbing>
    {
        public Climbing(Player player) : base(player) { }
        
        private readonly int _isClimb = Animator.StringToHash("IsClimb");
        private readonly int _climbTop = Animator.StringToHash("ClimbTop");
        private readonly int _climbValue = Animator.StringToHash("ClimbValue");

        private float _lowPointY;
        private float _highPointY;

        private bool _isClimbing;

        public void OnStateEnter()
        {
            _isClimbing = true;
            _rigidBody.useGravity = false;

            _animator.SetBool(_isClimb, true);
        }

        public void OnStateFixedUpdate()
        {
            CheckLadder();
        }

        public void OnStateExit()
        {
            _rigidBody.useGravity = true;
        }

        private void CheckLadder()
        {
            if (!_isClimbing)
                return;

            float playerPositionY = _playerTransform.position.y;

            bool isMove = playerPositionY > _lowPointY && playerPositionY < _highPointY;

            if (isMove)
            {
                ClimbMovement();
                return;
            }

            _isClimbing = false;

            float exitDirection = playerPositionY <= _lowPointY ? -1f : 1f;
            _player.StartCoroutine(ClimbEndMovement(exitDirection));
        }

        private void ClimbMovement()
        {
            var moveDirection = new Vector3(0f, _stateHandler.MoveVector.y, 0f);
            var moveVector = _rigidBody.position + moveDirection * _data.LadderSpeed * Time.fixedDeltaTime;

            _rigidBody.MovePosition(moveVector);
            _animator.SetFloat(_climbValue, moveDirection.y);
        }

        private IEnumerator ClimbEndMovement(float direction)
        {
            _animator.SetTrigger(_climbTop);

            yield return new WaitUntil(() =>
            {
                var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
                return stateInfo.IsName("Climbing_off_ladder_top");
            });

            _animator.SetBool(_isClimb, false);

            var waitForFixedUpdate = new WaitForFixedUpdate();
            while (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 0.85)  
            {
                Vector3 moveDirection = new Vector3(0f, 0f, direction).normalized;
                Vector3 moveVector = moveDirection * _data.LadderSpeed * Time.fixedDeltaTime;    
               
                _rigidBody.MovePosition(_rigidBody.position + moveVector);
                yield return waitForFixedUpdate;
            }

            _stateHandler.ChangeIdleORMoveState();
        }

        public void SetLadderSize((float lowPoint, float highPoint) ladderSize)
        {
            _lowPointY = ladderSize.lowPoint;
            _highPointY = ladderSize.highPoint;
        }
    }
}

