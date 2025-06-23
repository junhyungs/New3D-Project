using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EnumCollection;
using GameData;

namespace NPC_Component
{
    public abstract class NPC : MonoBehaviour, IInteractionDialog
    {
        protected Dialog _myDialog;
        protected Coroutine _dialogCoroutine;
        protected Npc _npcName;
        protected readonly string _order_Stroy = "Story";
        protected readonly string _order_Loop = "Loop";
        protected readonly string _order_End = "End";
        protected readonly string _dataKey = DataKey.DialogData.ToString();

        private void Start()
        {
            var dialogData = DataManager.Instance.GetData(_dataKey) as DialogData;
            if (dialogData == null)
                return;

            _myDialog = dialogData.GetMyDialog(_npcName.ToString());
        }

        public abstract void Interact();
        protected abstract IEnumerator StartDialog();
        protected void LockPlayer(bool islock)
        {
            PlayerManager.Instance.LockPlayer(islock);
        }

        protected bool CanNotDialog()
        {
            return _myDialog == null || _dialogCoroutine != null;
        }
    }
}

