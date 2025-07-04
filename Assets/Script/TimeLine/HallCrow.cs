using Cinemachine;
using EnumCollection;
using GameData;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TimeLineComponent
{
    public class HallCrow : TimeLine
    {
        [Header("Camera"), SerializeField]
        private CinemachineVirtualCamera _lastCamera;

        [Header("DummyPlayer"), SerializeField]
        private GameObject _dummyPlayer;
        private BoxCollider _collider;

        protected override void Awake()
        {
            base.Awake();
            _collider = GetComponent<BoxCollider>();
        }

        public override void PlayTimeLine()
        {
            if(_collider != null)
                _collider.enabled = false;

            _playableDirector.Play();
        }

        private void OnTriggerEnter(Collider other)
        {
            bool canTimeLine = other.gameObject.layer == LayerMask.NameToLayer("Player")
                || _playableDirector != null;

            if (!canTimeLine)
                return;

            var playerManager = PlayerManager.Instance;
            playerManager.PlayerObject.SetActive(false);
            PlayTimeLine();
        }

        public void HallCrow_Dialog_1()
        {
            var npcName = Npc.HallCrow_1.ToString();
            StartCoroutine(StartHallCrowDialog(npcName));
        }

        public void HallCrow_Dialog_2()
        {
            CameraBlend(0f);

            var npcName = Npc.HallCrow_2.ToString();
            StartCoroutine(StartHallCrowDialog(npcName));
        }

        public void HallCrow_Dialog_3()
        {
            var npcName = Npc.HallCrow_3.ToString();
            StartCoroutine(StartHallCrowDialog(npcName));
            StartCoroutine(ChangeFieldOfView(_lastCamera, 20f, 5f));
        }

        public void HallCrow_Dialog_4()
        {
            CameraBlend(2f);

            var npcName = Npc.HallCrow_4.ToString();
            StartCoroutine(LastDialog(npcName));
        }

        public void HallCrow_Dialog_5()
        {
            var npcName = Npc.HallCorw_5.ToString();
            StartCoroutine(StartHallCrowDialog(npcName));
        }

        private IEnumerator LastDialog(string npcName)
        {
            var playerManager = PlayerManager.Instance;

            var playerObject = playerManager.PlayerObject;
            playerObject.transform.position = _dummyPlayer.transform.position;
            playerObject.transform.rotation = _dummyPlayer.transform.rotation;
            playerManager.PlayerObject.SetActive(true);
            playerManager.LockPlayer(true);

            yield return StartCoroutine(StartHallCrowDialog(npcName));
            playerManager.LockPlayer(false);
        }

        private IEnumerator StartHallCrowDialog(string npcName)
        {
            _playableDirector.Pause();

            var dataKey = DataKey.DialogData.ToString();
            var dialogData = DataManager.Instance.GetData(dataKey) as DialogData;
            if(dialogData == null)
                yield break;

            var myDialog = dialogData.GetMyDialog(npcName);
            var dialogManager = DialogManager.Instance;
            yield return dialogManager.StartDialog(myDialog.Name, myDialog.StoryList);

            _playableDirector.Play();
        }

        private void CameraBlend(float blendValue)
        {
            var mainCamera = Camera.main;
            var cinemachineBrain = mainCamera.GetComponent<CinemachineBrain>();
            if (cinemachineBrain != null)
                cinemachineBrain.m_DefaultBlend.m_Time = blendValue;
        }

        private IEnumerator ChangeFieldOfView(CinemachineVirtualCamera targetCamera,
            float targetView, float duration)
        {
            var elapsed = 0f;
            var startView = targetCamera.m_Lens.FieldOfView;
            while (elapsed < duration)
            {
                elapsed += Time.deltaTime;
                var t = elapsed / duration;

                targetCamera.m_Lens.FieldOfView = Mathf.Lerp(startView, targetView, t);
                yield return null;
            }

            targetCamera.m_Lens.FieldOfView = targetView;
        }
    }
}

