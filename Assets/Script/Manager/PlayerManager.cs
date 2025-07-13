using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;
using Cinemachine;
using EnumCollection;

public class PlayerManager : Singleton_MonoBehaviour<PlayerManager>
{
    [Header("PlayerPrefab")]
    [SerializeField] private GameObject _playerPrefab;
    [Header("VirtualCamera")]
    [SerializeField] private GameObject _cameraPrefab;

    public Player PlayerComponent { get; private set; }
    public CinemachineVirtualCamera VirtualCameraComponent { get; private set; }    
    public CinemachineTransposer VirtualCameraTransposer { get; private set; }
    public GameObject PlayerObject => PlayerComponent.gameObject;
    public GameObject VirtualCameraObject => VirtualCameraComponent.gameObject;

    private void Start()
    {
        CreatePlayer();
        CreateVirtualCamera();
    }

    private void CreatePlayer() //TODO 나중에 게임 매니저에서 일괄적으로 관리.
    {
        var playerObject = Instantiate(_playerPrefab);
        PlayerComponent = playerObject.GetComponent<Player>();

        InventoryManager.Instance.InitializeInventory();
        playerObject.SetActive(false);
    }

    private void CreateVirtualCamera()
    {
        var virtualCameraObject = Instantiate(_cameraPrefab);
        //virtualCameraObject.transform.rotation = Quaternion.Euler(51f, 0f, 0f);

        VirtualCameraComponent = virtualCameraObject.GetComponent<CinemachineVirtualCamera>();
        VirtualCameraComponent.Follow = PlayerObject.transform;
        VirtualCameraComponent.LookAt = PlayerObject.transform;

        VirtualCameraTransposer = VirtualCameraComponent.GetCinemachineComponent<CinemachineTransposer>();
        VirtualCameraTransposer.m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;
        VirtualCameraTransposer.m_FollowOffset = new Vector3(0f, 10f, -8f);
        VirtualCameraTransposer.m_XDamping = 0f;
        VirtualCameraTransposer.m_YDamping = 0f;
        VirtualCameraTransposer.m_ZDamping = 0f;

        virtualCameraObject.SetActive(false);
    }

    public void LockPlayer(bool enable)
    {
        PlayerComponent.InputHandler.LockPlayer(!enable);
    }

    public void EnablePlayer(Vector3 position, Quaternion rotation)
    {
        PlayerObject.transform.position = position;
        PlayerObject.transform.rotation = rotation;
        PlayerObject.SetActive(true);
    }

    public void EnablePlayerCamera(PlayerCameraSetting playerCameraSetting, bool active)
    {
        bool equals = playerCameraSetting.Equals(default(PlayerCameraSetting));
        if (!equals)
        {
            VirtualCameraObject.transform.position = playerCameraSetting.Position;
            VirtualCameraComponent.m_Lens.FieldOfView = playerCameraSetting.FieldOfView;
            VirtualCameraTransposer.m_FollowOffset = playerCameraSetting.FollowOffset;
        }

        VirtualCameraObject.SetActive(active);
    }
}

public struct PlayerCameraSetting
{
    public Vector3 Position;
    public Vector3 FollowOffset;
    public float FieldOfView;    
}