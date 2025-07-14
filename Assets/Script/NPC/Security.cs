using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC_Component
{
    public class Security : NPC
    {
        public override void Interact()
        {
            if (!CanDialog(out var dialogDataSO))
                return;

            var npcName = dialogDataSO.NpcName;
            var dialogList = dialogDataSO.GetLoopDialogList();

            _dialogCoroutine = StartCoroutine(StartDialog(npcName, dialogList));
        }

        protected override IEnumerator StartDialog(string npcName, List<string> dialogList)
        {
            LockPlayer(true);
            yield return DialogManager.Instance.StartDialog(npcName, dialogList);
            LockPlayer(false);

            _dialogCoroutine = null;
        }
    }
}

