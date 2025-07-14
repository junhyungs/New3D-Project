using EnumCollection;
using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NPC_Component
{
    public class Banker : NPC
    {
        private Animator _animator;
        private readonly int _write = Animator.StringToHash("Write");
        private float _writeTime;
        private bool _hasWritten;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            var stateInfo = _animator.GetCurrentAnimatorStateInfo(0);
            bool canWrite = stateInfo.IsTag("PausedLoop") && stateInfo.normalizedTime >= 0.4f && !_hasWritten;
            if (canWrite)
            {
                _animator.SetBool(_write, true);
                _writeTime = 0f;
                _hasWritten = true;
            }
            else
            {
                _writeTime += Time.deltaTime;
                if(_writeTime >= 15f)
                {
                    _animator.SetBool(_write, false);
                    _hasWritten = false;
                }
            }
        }

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

