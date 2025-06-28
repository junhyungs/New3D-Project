using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCameraOffset : MonoBehaviour
{
    [Header("CameraOffset")]
    [SerializeField] private CameraOffset _nextOffset;
    [SerializeField] private CameraOffset _previousOffset;
    private Coroutine _coroutine;
    private bool _hasTriggered;
    
    private void OnTriggerEnter(Collider other)
    {
        bool isPlayer = other.gameObject.layer == LayerMask.NameToLayer("Player");
        if(!isPlayer)
            return;
        
        if (_coroutine != null)
        {
            StopCoroutine(_coroutine);
            _coroutine = null;
            _hasTriggered = !_hasTriggered;
        }
           
        var targetOffset = _hasTriggered ? _previousOffset : _nextOffset;
        _coroutine = StartCoroutine(StartChange(targetOffset));
    }

    private IEnumerator StartChange(CameraOffset targetOffset)
    {
        if(targetOffset == null)
            yield break;

        var playerManager = PlayerManager.Instance;
        var transposer = playerManager.VirtualCameraTransposer;
        var virtualCamera = playerManager.VirtualCameraComponent;

        var startOffset = transposer.m_FollowOffset;
        var startFOV = virtualCamera.m_Lens.FieldOfView;

        var distance = Vector3.Distance(startOffset, targetOffset.FollowOffset);
        if(distance < 0.01f)
            yield break;

        var duration = distance / targetOffset.BlendSpeed;
        var elapsed = 0f;

        while(elapsed < duration)
        {
            elapsed += Time.deltaTime;
            var t = Mathf.Clamp01(elapsed / duration);

            transposer.m_FollowOffset = Vector3.Lerp(startOffset, targetOffset.FollowOffset, t);
            virtualCamera.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetOffset.FieldOfView, t);
            yield return null;
        }

        transposer.m_FollowOffset = targetOffset.FollowOffset;
        virtualCamera.m_Lens.FieldOfView = targetOffset.FieldOfView;
        yield return null;

        _coroutine = null;
        _hasTriggered = !_hasTriggered;
    }
}
