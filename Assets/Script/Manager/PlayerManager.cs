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

    private void CreatePlayer()
    {
        var playerObject = Instantiate(_playerPrefab);
        PlayerComponent = playerObject.GetComponent<Player>();

        WeaponManager.Instance.SetWeapon(ItemType.Sword);
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
        //VirtualCameraTransposer.m_FollowOffset = new Vector3(0f, 10f, -8f);
        VirtualCameraTransposer.m_XDamping = 0f;
        VirtualCameraTransposer.m_YDamping = 0f;
        VirtualCameraTransposer.m_ZDamping = 0f;

        virtualCameraObject.SetActive(false);
    }

    public void LockPlayer(bool enable)
    {
        PlayerComponent.InputHandler.LockPlayer(!enable);
    }
}
