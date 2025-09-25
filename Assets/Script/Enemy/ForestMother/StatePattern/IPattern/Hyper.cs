using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;

namespace EnemyComponent
{
    public class Hyper : ForestMother_Pattern, IStateBehaviourController
    {
        private Coroutine _accelCoroutine;
        private Transform _targetTransform;

        private Vector3 _movePosition;

        private const int MaxWallHitCount = 3;
        private int _currentWallHitCount;

        private const float MaxAcceleration = 8f;
        private const float ReturnSpeed = 5f;
        private const float DashSpeed = 100f;

        private bool _return;

        public override void Init(ForestMother owner)
        {
            base.Init(owner);
            GetBehaviour(owner.Property);
        }

        public override void Start()
        {
            IsRunning = true;
            if (_targetTransform == null)
            {
                _targetTransform = GetTargetTransform();
                if (_targetTransform == null)
                {
                    IsRunning = false;
                    return;
                }
            }

            SetIsTrigger(true);
            _owner.StartCoroutine(WaitForAnimation());
        }

        public override void Update()
        {
            if (!_return)
                return;

            var distance = Vector3.Distance(_property.StartPosition, _owner.transform.position);
            if(distance <= 0.1f)
            {
                PlayAnimation(MotherParameterKey.StartHyper_Bool_false);
                _return = false;
            }
        }

        public override void Exit()
        {
            SetIsTrigger(false);
            _currentWallHitCount = 0;
        }

        public override void OnTriggerEnter(Collider other)
        {
            if(other.TryGetComponent(out ITakeDamage takeDamage))
                takeDamage.TakeDamage(_property.Data.Damage);

            if(other.gameObject.layer == LayerMask.NameToLayer("Wall"))
                _owner.StartCoroutine(HandleWallCollision());
        }

        private IEnumerator HandleWallCollision()
        {
            SetAgent(0f, MaxAcceleration, _owner.transform.position);
            yield return new WaitForSeconds(0.5f);

            _currentWallHitCount++;

            if (_currentWallHitCount < MaxWallHitCount)
            {
                _movePosition = GetMovePosition();
                SetAgent(DashSpeed, 1f, _movePosition);
                StartAccelerationCoroutine();
            }
            else
            {
                _movePosition = _property.StartPosition;
                _return = true;
                SetAgent(ReturnSpeed, MaxAcceleration, _movePosition);
            }
        }

        public override IEnumerator WaitForAnimation()
        {
            PlayAnimation(MotherParameterKey.Hyper_Trigger);
            yield return new WaitUntil(() =>
            {
                var stateInfo = _property.Animator.GetCurrentAnimatorStateInfo(1);
                return stateInfo.IsName("EndHyper") && stateInfo.normalizedTime >= 0.88f;
            });

            yield return _delay;
            IsRunning = false;
        }

        private void StartAccelerationCoroutine()
        {
            if (_accelCoroutine != null)
            {
                _owner.StopCoroutine(_accelCoroutine);
                _accelCoroutine = null;
            }

            _accelCoroutine = _owner.StartCoroutine(AccelerationCoroutine());
        }

        private void SetAgent(float speed, float accleration, Vector3 destination)
        {
            _property.NavMeshAgent.speed = speed;
            _property.NavMeshAgent.acceleration = accleration;
            _property.NavMeshAgent.SetDestination(destination);
        }

        private IEnumerator AccelerationCoroutine()
        {
            var currentValue = 0f;
            while(currentValue <= MaxAcceleration)
            {
                currentValue = _property.NavMeshAgent.acceleration;
                _property.NavMeshAgent.acceleration = Mathf.MoveTowards(
                    currentValue,
                    MaxAcceleration,
                    3f * Time.deltaTime);

                yield return null;
            }

            _accelCoroutine = null;
        }

        private Vector3 GetMovePosition()
        {
            var rayDirection = (_targetTransform.position - _owner.transform.position).normalized;
            var rayPosition = _owner.transform.position + (Vector3.up * 0.5f);

            RaycastHit[] raycastHits = Physics.RaycastAll(rayPosition, rayDirection, 80f, LayerMask.GetMask("Wall"));
            if (raycastHits.Length > 0)
            {
                var backDistance = 2f;
                return raycastHits[0].point - rayDirection * backDistance;
            }
            else
                return _targetTransform.position;
        }

        private Transform GetTargetTransform()
        {
            var playerLayer = LayerMask.GetMask("Player");
            var resultArray = new Collider[1];
            var hitCount = Physics.OverlapSphereNonAlloc(
                _owner.transform.position,
                _property.Data.DetectionRange,
                resultArray,
                playerLayer);

            if (hitCount > 0)
                return resultArray[0].transform;
            else
                return null;
        }

        public override void Enable()
        {
            GetBehaviour(_property);
        }

        private void GetBehaviour(ForestMotherProperty property)
        {
            var animator = property.Animator;
            var hyperBehaviour = animator.GetBehaviour<HyperBehaviour>();
            if(hyperBehaviour != null)
                hyperBehaviour.Controller = this;
        }

        public void OnEnter(Animator animator, AnimatorStateInfo stateInfo)
        {
            _property.AnimController.InfinitiRotation(Vector3.down, true, 150f);

            _movePosition = GetMovePosition();
            SetAgent(DashSpeed, 1f, _movePosition);
            StartAccelerationCoroutine();
        }

        public void OnExit(Animator animator, AnimatorStateInfo stateInfo)
        {
            _property.AnimController.InfinitiRotation(Vector3.down, false);
        }
    }
}

