using EnumCollection;
using System.Collections;
using System.Collections.Generic;
using TimeLineComponent;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;
using System;
using UnityEngine.UI;

public class ShortCutDoor : TimeLineComponent.TimeLine, IInteractionGameObject
{
    private readonly HashSet<string> _playableTrackNames = new HashSet<string>()
    {
        "PlayerObjectTrack", "PlayerWalkAnimationTrack", "PlayerMoveTrack"
    };

    [Header("Link")] 
    [SerializeField] private LinkedDoor _destination;
    [SerializeField] private LinkedMap _linkedMap;

    [Header("Transform")]
    [SerializeField] private Transform _inTransform;
    [SerializeField] private Transform _outTransform;

    [Header("TimelineAsset")]
    [SerializeField] private TimelineAsset _in;
    [SerializeField] private TimelineAsset _out;

    [Header("FadeImage")]
    [SerializeField] private Image _fadeImage;

    private void OnDisable()
    {
        if (_fadeImage == null)
            return;

        var color = _fadeImage.color;
        color.a = 0f;
        _fadeImage.color = color;
    }

    public bool IsWeaponInteractable { get; set; } = false;
    public void Interact() => PlayInTimeLine();
    private void PlayInTimeLine()
    {
        if (!TrySetPlayerTransform(_outTransform, -transform.forward, out GameObject playerObject))
            return;

        var key = EnableUI.PlayerUI.ToString();
        UIManager.Instance.DisableUI(key);

        LockPlayer(true);

        var addressableKey = "Map/" + _linkedMap.ToString();
        PlayTimeLine(playerObject, _in,
            () => MapManager.Instance.ChangeMapAsync(addressableKey, _destination));
    }

    public void PlayOutTimeLine()
    {
        if (!TrySetPlayerTransform(_outTransform, transform.forward, out GameObject playerObject))
            return;

        Action afterAction = () =>
        {
            var key = EnableUI.PlayerUI.ToString();
            UIManager.Instance.EnableUI(key);

            LockPlayer(false);
        };
        
        PlayTimeLine(playerObject, _out, afterAction);
    }

    private bool TrySetPlayerTransform(Transform target, Vector3 forward, out GameObject playerObject)
    {
        playerObject = PlayerManager.Instance?.PlayerObject;
        if (playerObject == null)
        {
            LockPlayer(false);
            return false;
        }

        playerObject.transform.position = target.position;
        playerObject.transform.rotation = Quaternion.LookRotation(forward);
        return true;
    }

    private void PlayTimeLine(GameObject playerObject, TimelineAsset asset, Action afterAction)
    {
        if(_playableDirector == null)
            Init();

        _playableDirector.playableAsset = asset;

        var playerRigid = playerObject.GetComponent<Rigidbody>();
        playerRigid.isKinematic = true;

        StartCoroutine(Delay(afterAction, playerRigid));
        BindTrack(playerObject);
    }

    private void BindTrack(GameObject playerObject)
    {
        var playableAsset = _playableDirector.playableAsset;
        foreach(var output in playableAsset.outputs)
        {
            if (_playableTrackNames.Contains(output.streamName))
                _playableDirector.SetGenericBinding(output.sourceObject, playerObject);
        }

        PlayTimeLine();
    }

    private IEnumerator Delay(Action action, Rigidbody playerRigid)
    {
        bool isDone = false;

        void OnStopped(PlayableDirector playableDirector)
        {
            isDone = true;
            playerRigid.isKinematic = false;
        }
        
        _playableDirector.stopped += OnStopped;

        yield return new WaitUntil(() => isDone);
        _playableDirector.stopped -= OnStopped;

        action?.Invoke();
    }

    private void LockPlayer(bool isLock)
    {
        PlayerManager.Instance.LockPlayer(isLock);
    }

    public override void PlayTimeLine()
    {
        _playableDirector.Play();
    }

    public void Signal_DisablePlayerCamera()
    {
        PlayerManager.Instance?.EnablePlayerCamera(new PlayerCameraSetting(), false);
    }

    public void Signal_EnablePlayerCamera()
    {
        PlayerManager.Instance?.EnablePlayerCamera(new PlayerCameraSetting(), true);
    }
}
