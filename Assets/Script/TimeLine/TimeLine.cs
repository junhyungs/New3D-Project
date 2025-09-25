using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace TimeLineComponent
{
    public abstract class TimeLine : MonoBehaviour
    {
        protected PlayableDirector _playableDirector;
        public abstract void PlayTimeLine();
        private void Awake()
        {
            Init();
        }

        protected virtual void Init()
        {
            _playableDirector = GetComponent<PlayableDirector>();
        }

        public void LockPlayer(string value)
        {
            var playerManager = PlayerManager.Instance;
            if (playerManager == null)
                return;

            if (!string.IsNullOrEmpty(value) &&
                bool.TryParse(value, out bool result))
                playerManager.LockPlayer(result);
        }

        public void BlendTimeSetting(float blendTime)
        {
            var cinemachineBrain = Camera.main.GetComponent<CinemachineBrain>();
            if(cinemachineBrain == null)
                return;

            var blend = cinemachineBrain.m_DefaultBlend;
            blend.m_Time = blendTime;
            cinemachineBrain.m_DefaultBlend = blend;
        }

        public virtual void Signal_DisablePlayerCamera()
        {
            PlayerCameraSetting(new PlayerCameraSetting(), false);
        }

        public virtual void Signal_EnablePlayerCamera()
        {
            PlayerCameraSetting(new PlayerCameraSetting(), true);
        }

        protected void PlayerCameraSetting(PlayerCameraSetting setting, bool value)
        {
            PlayerManager.Instance?.EnablePlayerCamera(setting, value);
        }
    }
}

