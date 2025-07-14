using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using GameData;

namespace NPC_Component
{
    public abstract class NPC : MonoBehaviour, IInteractionDialog
    {
        [Header("DiaogSOKey")]
        [SerializeField] private ScriptableDataKey _key;
        protected Coroutine _dialogCoroutine;
        
        public abstract void Interact();
        protected abstract IEnumerator StartDialog(string npcName, List<string> dialogList);
        protected bool CanDialog(out DialogDataSO dialogDataSO)
        {
            dialogDataSO = DataManager.Instance.GetScriptableData(_key) as DialogDataSO;
            if (dialogDataSO != null && _dialogCoroutine == null)
                return true;

            return false;
        }
        protected void LockPlayer(bool isLocked)
        {
            var playerManager = PlayerManager.Instance;
            if (playerManager == null)
                return;

            playerManager.LockPlayer(isLocked);
        }
    }
}

