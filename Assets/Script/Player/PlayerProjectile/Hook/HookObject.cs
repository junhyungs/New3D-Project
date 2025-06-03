using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PlayerComponent
{
    public class HookObject : PlayerProjectile
    {
        [Header("AnchorObject"), SerializeField] private GameObject _anchorObject;
        public Stack<GameObject> EnableChains => _enableChains;
        private Stack<GameObject> _enableChains = new Stack<GameObject>();
        private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();        
        private HookChain _hookChain;
        private Action<Vector3, HookObject> _oneTimeAction;

        private int _chainIndex;

        private float _distance;
        private float _intervalDistance;
        private float _maxDistance;
        private bool _isCollision;

        protected override void Awake()
        {
            base.Awake();

            _hookChain = GetComponentInChildren<HookChain>();
            CarculateMaxDistance();
        }

        private void CarculateMaxDistance()
        {
            var mySizeZ = GetComponent<BoxCollider>().size.z;
            var chainSize = _hookChain.GetChainSize();

            _intervalDistance = chainSize / _hookChain.Chains.Length;
            _maxDistance = mySizeZ + chainSize;
        }

        protected override void OnEnable()
        {
            if (!_anchorObject.activeSelf)
                _anchorObject.SetActive(true);
        }

        protected override void OnDisable()
        {
            Initialize();
        }

        public override void Fire()
        {
            StartCoroutine(HookStart());
        }

        private void Initialize()
        {
            _enableChains.Clear();
            _isCollision = false;
            _distance = 0f;
            _chainIndex = 0;
        }

        private IEnumerator HookStart()
        {
            yield return StartCoroutine(ExtendHook());

            if (!_isCollision)
            {
                yield return StartCoroutine(ReturnHook());
                _oneTimeAction?.Invoke(Vector3.zero, this);
            }
            else
                _anchorObject.SetActive(false);
        }

        private IEnumerator ExtendHook()
        {
            while(_distance < _maxDistance && !_isCollision)
            {
                _distance += _speed * Time.fixedDeltaTime;
                if(_distance > _maxDistance)
                {
                    _distance = _maxDistance;
                }

                Movement(1);

                int theoreticalChainCount = Mathf.FloorToInt(_distance / _intervalDistance);
                while(_chainIndex < _hookChain.Chains.Length && _chainIndex < theoreticalChainCount)
                {
                    var chain = _hookChain.Chains[_chainIndex].gameObject;
                    chain.SetActive(true);

                    _enableChains.Push(chain);
                    _chainIndex++;
                }

                yield return _waitForFixedUpdate;
            }

            while(_chainIndex < _hookChain.Chains.Length && !_isCollision)
            {
                var chain = _hookChain.Chains[_chainIndex].gameObject;
                chain.SetActive(true);

                _enableChains.Push(chain);
                _chainIndex++;
                yield return null;
            }
        }

        private IEnumerator ReturnHook()
        {
            var intervalDistance = _intervalDistance - 0.05f;

            while (_distance > 0.05f)
            {
                _distance -= _speed * Time.fixedDeltaTime;
                if(_distance < 0)
                {
                    _distance = 0;
                }

                Movement(-1);

                int theoreticalChainCount = Mathf.FloorToInt(_distance / intervalDistance);

                while(_enableChains.Count > theoreticalChainCount)
                {
                    var chain = _enableChains.Pop();
                    chain.SetActive(false);
                }

                yield return _waitForFixedUpdate;
            }
        }

        private void Movement(int direction)
        {
            Vector3 moveDirection = transform.forward * direction;
            Vector3 moveVector = moveDirection * _speed * Time.fixedDeltaTime;

            _rigidBody.MovePosition(_rigidBody.position +  moveVector);
        }

        public void CallBackCollisionVector3(Action<Vector3, HookObject> callBack)
        {
            Action<Vector3, HookObject> oneTimeAction = null;
            oneTimeAction = (transform, gameObject) =>
            {
                callBack?.Invoke(transform, this);
                _oneTimeAction -= oneTimeAction;
            };
            _oneTimeAction += oneTimeAction;
        }

        protected override void OnTriggerEnter(Collider other)
        {
            bool isTarget = other.gameObject.tag == "HookAnchor" ||
                other.gameObject.layer == LayerMask.NameToLayer("Enemy");
            if (isTarget)
            {
                _isCollision = true;

                Hit(other);

                var hitPoint = other.ClosestPoint(transform.position);
                var movePosition = hitPoint - transform.forward * 1.2f;
                _oneTimeAction?.Invoke(movePosition, this);
            }
        }
    }
}


