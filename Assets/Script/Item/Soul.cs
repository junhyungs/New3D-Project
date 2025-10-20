using EnumCollection;
using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ItemComponent
{
    public class Soul : MonoBehaviour, ICurrencyItem
    {
        private Coroutine _coroutine;

        public ItemType SlotName => ItemType.Soul;
        public ItemDataSO ItemDataSO => 
            DataManager.Instance.GetScriptableData(ScriptableDataKey.SoulItemDataSO) as ItemDataSO;

        private void OnEnable()
        {
            var playerObject = PlayerManager.Instance.PlayerObject;
            if(playerObject != null)
            {
                var targetTransform = playerObject.transform;
                _coroutine = StartCoroutine(MovementCoroutine(targetTransform));
            }
        }

        private void OnDisable()
        {
            _coroutine = null;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player"))
                return;

            StopCoroutine(_coroutine);
            InventoryManager.Instance.SetItem(this);

            gameObject.SetActive(false);
            transform.localPosition = Vector3.zero;
        }

        private IEnumerator MovementCoroutine(Transform targetTransform)
        {
            var soulData = ItemDataSO as SoulItemDataSO;
            if (soulData == null)
                yield break;

            var time = 0f;
            var maxTime = soulData.MaxTime;
            var moveSpeed = soulData.MoveSpeed;
            while(time < maxTime)
            {
                var translation = Vector3.up * moveSpeed * Time.deltaTime;
                transform.Translate(translation);
                time += Time.deltaTime;
                yield return null;
            }

            moveSpeed *= 2f;
            while (true)
            {
                var targetVector = targetTransform.position + Vector3.up * 0.7f;
                transform.position = Vector3.MoveTowards(transform.position, targetVector, moveSpeed * Time.deltaTime);
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

