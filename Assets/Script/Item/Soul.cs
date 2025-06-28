using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemComponent
{
    public class Soul : CurrencyItem
    {
        [Header("Movement")]
        [SerializeField] private float _maxTime;
        [SerializeField] private float _speed;
        [Header("SoulValue"), SerializeField]
        private int _soulValue;
        private bool _isMove;

        public override ItemType SlotName => ItemType.Soul;

        private void OnEnable()
        {
            ColliderANDMoveControl(true);
        }

        private void OnTriggerEnter(Collider other)
        {
            bool collision = other.gameObject.layer == LayerMask.NameToLayer("Player");
            if (!collision)
                return;

            ColliderANDMoveControl(false);
            InventoryManager.Instance.SetGameItem(this);
            gameObject.SetActive(false);
        }

        private void ColliderANDMoveControl(bool isMove)
        {
            _isMove = isMove;
            _collider.enabled = isMove;
        }

        public override void Interact()
        {
            StartCoroutine(StartMovement());
        }

        private IEnumerator StartMovement()
        {
            var time = 0f;
            var moveDirection = Vector3.up;
            while(time < _maxTime)
            {
                var moveVector = moveDirection * _speed * Time.deltaTime;
                transform.Translate(moveVector);

                time += Time.deltaTime;
                yield return null;
            }

            var playerObject = PlayerManager.Instance.PlayerObject;
            var playerTransform = playerObject.transform;
            var speed = _speed * 2;
            while (_isMove)
            {
                var targetPosition = playerTransform.position + Vector3.up * 0.7f;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
        }

        public override int GetValue()
        {
            return _soulValue;
        }
    }
}

