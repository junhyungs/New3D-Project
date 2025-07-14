using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using EnumCollection;
using GameData;
using Cinemachine;

namespace TimeLineComponent
{
    public class Intro : TimeLine
    {
        [Header("IntroObject")]
        [SerializeField] private GameObject _dummyPlayer;
        [SerializeField] private GameObject _introCamera;

        [Header("DialogKey")]
        [SerializeField] private ScriptableDataKey _key;

        public override void PlayTimeLine()
        {
            if (_playableDirector == null)
                return;

            _playableDirector.Play();
        }

        public void Signal_IntroDialog()
        {
            StartCoroutine(StartIntroDialog());
        }

        private IEnumerator StartIntroDialog()
        {
            _playableDirector.Pause();

            var dialogDataSO = DataManager.Instance.GetScriptableData(_key) as DialogDataSO;
            if (dialogDataSO != null)
            {
                var npcName = dialogDataSO.NpcName;
                var dialogList = dialogDataSO.GetMainDialogList();

                yield return DialogManager.Instance.StartDialog(npcName, dialogList);
            }

            _playableDirector.Play();
        }

        public void Signal_MovePlayer()
        {
            var targetPosition = _dummyPlayer.transform.position;
            var targetRotation = _dummyPlayer.transform.rotation;
            PlayerManager.Instance.EnablePlayer(targetPosition, targetRotation);

            IntroCameraSetting();
        }

        private void IntroCameraSetting()
        {
            var introCameraComponent = _introCamera.GetComponent<CinemachineVirtualCamera>();
            var introCameraTransposer = introCameraComponent.GetCinemachineComponent<CinemachineTransposer>();

            var playerCameraSetting = new PlayerCameraSetting
            {
                Position = _introCamera.transform.position,
                FollowOffset = introCameraTransposer.m_FollowOffset,
                FieldOfView = introCameraComponent.m_Lens.FieldOfView,
            };

            PlayerManager.Instance.EnablePlayerCamera(playerCameraSetting, true);
        }
    }
}

