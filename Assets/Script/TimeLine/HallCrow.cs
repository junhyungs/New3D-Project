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

        protected override void Init()
        {
            base.Init();
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
            var key = ScriptableDataKey.HallCrow_1_DialogSO;
            StartCoroutine(StartHallCrowDialog(key));
        }

        public void HallCrow_Dialog_2()
        {
            CameraBlend(0f);

            var key = ScriptableDataKey.HallCrow_2_DialogSO;
            StartCoroutine(StartHallCrowDialog(key));
        }

        public void HallCrow_Dialog_3()
        {
            var key = ScriptableDataKey.HallCrow_3_DialogSO;
            StartCoroutine(StartHallCrowDialog(key));
            StartCoroutine(ChangeFieldOfView(_lastCamera, 20f, 5f));
        }

        public void HallCrow_Dialog_4()
        {
            CameraBlend(2f);
            var key = ScriptableDataKey.HallCrow_4_DialogSO;
            StartCoroutine(LastDialog(key));
        }

        public void HallCrow_Dialog_5()
        {
            var key = ScriptableDataKey.HallCrow_5_DialogSO;
            StartCoroutine(StartHallCrowDialog(key));
        }

        private IEnumerator LastDialog(ScriptableDataKey key)
        {
            var playerManager = PlayerManager.Instance;

            var playerObject = playerManager.PlayerObject;
            playerObject.transform.position = _dummyPlayer.transform.position;
            playerObject.transform.rotation = _dummyPlayer.transform.rotation;
            playerManager.PlayerObject.SetActive(true);
            playerManager.LockPlayer(true);

            yield return StartCoroutine(StartHallCrowDialog(key));
            playerManager.LockPlayer(false);
        }

        private IEnumerator StartHallCrowDialog(ScriptableDataKey key)
        {
            _playableDirector.Pause();

            var dialogDataSO = DataManager.Instance.GetScriptableData(key) as DialogDataSO;
            if(dialogDataSO != null)
            {
                var npcName = dialogDataSO.NpcName;
                var dialogList = dialogDataSO.GetMainDialogList();
                yield return DialogManager.Instance.StartDialog(npcName, dialogList);
            }

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

