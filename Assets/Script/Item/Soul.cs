using EnumCollection;
using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemComponent
{
    public class Soul : Item, ICurrencyItem
    {
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
            InventoryManager.Instance.SetItem(this);
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
            var soulDataSO = ItemDataSO as SoulItemDataSO;
            if (soulDataSO == null)
                yield break;
            
            var time = 0f;
            var maxTime = soulDataSO.MaxTime;
            var moveDirection = Vector3.up;

            while(time < maxTime)
            {
                var moveVector = moveDirection * soulDataSO.MoveSpeed * Time.deltaTime;
                transform.Translate(moveVector);

                time += Time.deltaTime;
                yield return null;
            }

            var playerObject = PlayerManager.Instance.PlayerObject;
            var playerTransform = playerObject.transform;
            var speed = soulDataSO.MoveSpeed * 2;

            while (_isMove)
            {
                var targetPosition = playerTransform.position + Vector3.up * 0.7f;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
                yield return null;
            }
        }

        public int GetValue()
        {
            var soulDataSO = ItemDataSO as SoulItemDataSO;
            if (soulDataSO == null)
                return 1;

            return soulDataSO.SoulValue;
        }
    }
}

