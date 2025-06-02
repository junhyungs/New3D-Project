using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PlayerComponent
{
    public class PlayerHook : MonoBehaviour
    {
        [Header("AnchorObject"), SerializeField] private GameObject _anchorObject;
        public Stack<GameObject> EnableChains => _enableChains;
        private Stack<GameObject> _enableChains = new Stack<GameObject>();
        private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();        
        private HookChain _hookChain;
        private Rigidbody _rigidBody;
        private Action<Vector3, PlayerHook> _oneTimeAction;

        private int _chainIndex;

        private float _maxDistance;
        private float _distance;
        private float _intervalDistance;
        private float _speed = 8f;

        private bool _isCollision;

        private void Awake()
        {
            _hookChain = GetComponentInChildren<HookChain>();
            _rigidBody = GetComponent<Rigidbody>();

            CarculateMaxDistance();
        }

        private void CarculateMaxDistance()
        {
            var mySizeZ = GetComponent<BoxCollider>().size.z;
            var chainSize = _hookChain.GetChainSize();

            _intervalDistance = chainSize / _hookChain.Chains.Length;
            _maxDistance = mySizeZ + chainSize;
        }

        private void OnEnable()
        {
            if (!_anchorObject.activeSelf)
                _anchorObject.SetActive(true);
        }

        private void OnDisable()
        {
            Initialize();
        }

        public void FireHook()
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

        public void CallBackCollisionVector3(Action<Vector3, PlayerHook> callBack)
        {
            Action<Vector3, PlayerHook> oneTimeAction = null;
            oneTimeAction = (transform, gameObject) =>
            {
                callBack?.Invoke(transform, this);
                _oneTimeAction -= oneTimeAction;
            };
            _oneTimeAction += oneTimeAction;
        }

        private void OnTriggerEnter(Collider other)
        {
            bool isTarget = other.gameObject.tag == "HookAnchor";
            if (isTarget)
            {
                _isCollision = true;

                var hitPoint = other.ClosestPoint(transform.position);
                var movePosition = hitPoint - transform.forward * 1.2f;
                _oneTimeAction?.Invoke(movePosition, this);
            }
        }
    }
}


