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

            var dataKey = DataKey.DialogData.ToString();
            var dialogData = DataManager.Instance.GetData(dataKey) as DialogData;
            if (dialogData == null)
                yield break;

            var npcName = Npc.BusNPC.ToString();
            var myDialog = dialogData.GetMyDialog(npcName);

            var dialogManager = DialogManager.Instance;
            yield return dialogManager.StartDialog(myDialog.Name, myDialog.StoryList);

            _playableDirector.Play();
        }

        public void Signal_MovePlayer()
        {
            var playerObject = PlayerManager.Instance.PlayerObject;
            playerObject.transform.position = _dummyPlayer.transform.position;
            playerObject.transform.rotation = _dummyPlayer.transform.rotation;
            playerObject.SetActive(true);

            IntroCameraSetting();
        }

        private void IntroCameraSetting()
        {
            var introCameraComponent = _introCamera.GetComponent<CinemachineVirtualCamera>();
            var introCameraTransposer = introCameraComponent.GetCinemachineComponent<CinemachineTransposer>();

            var playerCameraObject = PlayerManager.Instance.VirtualCameraObject;
            playerCameraObject.transform.position = _introCamera.transform.position;

            var playerCameraComponent = PlayerManager.Instance.VirtualCameraComponent;
            playerCameraComponent.m_Lens.FieldOfView = introCameraComponent.m_Lens.FieldOfView;
            
            var playerCameraTransposer = PlayerManager.Instance.VirtualCameraTransposer;
            playerCameraTransposer.m_FollowOffset = introCameraTransposer.m_FollowOffset;

            playerCameraObject.SetActive(true);
        }
    }
}

