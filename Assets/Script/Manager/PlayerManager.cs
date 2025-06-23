using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;
using Cinemachine;

public class PlayerManager : Singleton_MonoBehaviour<PlayerManager>
{
    [Header("PlayerPrefab"), SerializeField] private GameObject _playerPrefab; //TODO Resources File
    [Header("PlayerVirtualCamera"), SerializeField] private GameObject _virtualCamera; //TODO Resources File
    public Player Player { get; private set; }
    public GameObject VirtualCamera { get; private set; }

    private void Awake()
    {
        CreatePlayer();
        CreateVirtualCamera();
    }

    private void CreatePlayer()
    {
        var playerObject = Instantiate(_playerPrefab);
        Player = playerObject.GetComponent<Player>();
    }

    private void CreateVirtualCamera()
    {
        VirtualCamera = Instantiate(_virtualCamera);
        VirtualCamera.transform.rotation = Quaternion.Euler(51f, 0f, 0f);

        CinemachineVirtualCamera virtualCameraComponent = VirtualCamera.GetComponent<CinemachineVirtualCamera>();
        virtualCameraComponent.Follow = Player.transform;
        virtualCameraComponent.LookAt = Player.transform;

        var doNothing = virtualCameraComponent.GetCinemachineComponent<CinemachineComposer>();
        if(doNothing != null)
            Destroy(doNothing);

        var transPoser = virtualCameraComponent.GetCinemachineComponent<CinemachineTransposer>();
        transPoser.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
        transPoser.m_FollowOffset = new Vector3(0f, 10f, -8f);
        transPoser.m_XDamping = 0f;
        transPoser.m_YDamping = 0f;
        transPoser.m_ZDamping = 0f;
    }

    public void ForceMovePlayer(Transform transform)
    {
        Player.transform.position = transform.position; 
    }

    public void ForceMoveVirtualCamera(Transform transform)
    {
        VirtualCamera.transform.position = transform.position;
    }

    public void LockPlayer(bool enable)
    {
        Player.InputHandler.LockPlayer(!enable);
    }
}
