using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace PlayerComponent
{
    public class PlayerHook : MonoBehaviour
    {
        private Stack<GameObject> _enableChains = new Stack<GameObject>();
        private WaitForFixedUpdate _waitForFixedUpdate = new WaitForFixedUpdate();
        private Coroutine _moveCoroutine;
        private HookChain _hookChain;
        private Rigidbody _rigidBody;

        private int _chainIndex;

        private float _maxDistance;
        private float _distance;
        private float _intervalDistance;
        private float _speed = 8f;

        private Vector3 _startPos;
        private Vector3 _nextPos;

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
            _startPos = transform.localPosition;
            _moveCoroutine = StartCoroutine(HookStart());
        }

        private void OnDisable()
        {
            _chainIndex = 0;
        }

        private IEnumerator HookStart()
        {
            yield return StartCoroutine(ExtendHook());

            if (!_isCollision)
            {
                yield return StartCoroutine(ReturnHook());
            }

            if(_enableChains.Count > 0)
            {
                foreach (var chain in _enableChains)
                    chain.SetActive(false);
            }

            _enableChains.Clear();
            gameObject.SetActive(false);
        }

        private IEnumerator ExtendHook()
        {
            _nextPos = _startPos;

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
                    _nextPos = transform.localPosition;
                    _chainIndex++;
                }

                yield return _waitForFixedUpdate;
            }

            while(_chainIndex < _hookChain.Chains.Length)
            {
                var chain = _hookChain.Chains[_chainIndex].gameObject;
                chain.SetActive(true);
                _enableChains.Push(chain);
                _nextPos = transform.localPosition;
                _chainIndex++;

                yield return null;
            }
        }

        private IEnumerator ReturnHook()
        {
            while(_distance > 0.05f)
            {
                _distance -= _speed * Time.fixedDeltaTime;
                if(_distance < 0)
                {
                    _distance = 0;
                }

                Movement(-1);

                int theoreticalChainCount = Mathf.FloorToInt(_distance / _intervalDistance);

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

        public void CallBackCollisionTransform()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            
        }
    }
}


