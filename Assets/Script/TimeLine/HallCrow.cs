using Cinemachine;
using EnumCollection;
using GameData;
using MapComponent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace TimeLineComponent
{
    public class HallCrow : TimeLine
    {
        private readonly HashSet<string> _playableTrakNames = new HashSet<string>()
        {
            "PlayerWalkAnimationTrack"
        };

        [Header("Camera"), SerializeField]
        private CinemachineVirtualCamera _lastCamera;

        [Header("TransformObject"), SerializeField]
        private Transform _dummyTransform;

        private BoxCollider _collider;
        private Action _afterAction;

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
            BindPlayer();
        }

        private void BindPlayer()
        {
            var uiKey = EnableUI.PlayerUI.ToString();
            UIManager.Instance.DisableUI(uiKey);

            var playerManager = PlayerManager.Instance;
            playerManager.LockPlayer(true);

            var playerObject = playerManager.PlayerObject;
            playerObject.transform.position = _dummyTransform.position;
            playerObject.transform.rotation = _dummyTransform.rotation;  
            playerObject.transform.SetParent(_dummyTransform);

            var playerRigid = playerObject.GetComponent<Rigidbody>();
            playerRigid.isKinematic = true;

            Action afterAction = null;
            afterAction = () =>
            {
                UIManager.Instance.EnableUI(uiKey);
                playerObject.transform.SetParent(null, true);
                playerRigid.isKinematic = false;
                playerManager.LockPlayer(false);

                _afterAction -= afterAction;
            };
            _afterAction += afterAction;

            var playableAsset = _playableDirector.playableAsset;
            foreach (var output in playableAsset.outputs)
                if (_playableTrakNames.Contains(output.streamName))
                    _playableDirector.SetGenericBinding(output.sourceObject, playerObject);

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

            var mapComponent = GetMapComponent();
            if (mapComponent != null)
                mapComponent.MapLevelData.OpenDoor.Add(LinkedDoor.Level_0_Level_1);

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
            Action<PlayableDirector> onStopped = null;
            onStopped = (playableDirector) =>
            {
                _afterAction?.Invoke();
                _playableDirector.stopped -= onStopped;
            };
            _playableDirector.stopped += onStopped;

            yield return StartCoroutine(StartHallCrowDialog(key));

            var mapComponent = GetMapComponent();
            if (mapComponent != null)
            {
                var mapEventDic = mapComponent.MapLevelData.MapEventDictionary;
                mapEventDic.TryAdd(GameEvent.HallCrow, true);
            }
        }

        private Level_0 GetMapComponent()
        {
            var parentObject = transform.root.gameObject;
            var mapComponent = parentObject.GetComponent<Level_0>();
            return mapComponent;
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

