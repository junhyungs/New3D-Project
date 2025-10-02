using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayerComponent;
using Cinemachine;
using EnumCollection;
using GameData;

public class PlayerManager : Singleton_MonoBehaviour<PlayerManager>
{
    [Header("VirtualCamera")]
    [SerializeField] private GameObject _cameraPrefab;

    public Player PlayerComponent { get; private set; }
    public CinemachineVirtualCamera VirtualCameraComponent { get; private set; }    
    public CinemachineTransposer VirtualCameraTransposer { get; private set; }
    public GameObject PlayerObject => PlayerComponent.gameObject;
    public GameObject VirtualCameraObject => VirtualCameraComponent.gameObject;
    public PlayerCameraSetting CurrentCameraSetting
    {
        get
        {
            var cameraSetting = new PlayerCameraSetting()
            {
                Position = VirtualCameraObject.transform.position,
                FollowOffset = VirtualCameraTransposer.m_FollowOffset,
                FieldOfView = VirtualCameraComponent.m_Lens.FieldOfView
            };
            return cameraSetting;
        }
    }

    public void SetPlayer(GameObject playerObject)
    {
        PlayerComponent = playerObject.GetComponent<Player>();
        CreateVirtualCamera();
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

    public void LockPlayer(bool isLocked)
    {
        PlayerComponent.InputHandler.LockPlayer(!isLocked);
    }

    public void LoadPlayer(MapData mapData)
    {
        if (!mapData.SaveData)
            return;

        var savePosition = mapData.SerializeVector3.ToVector3();
        var saveRotation = mapData.SerializeQuaternion.ToQuaternion();
        EnablePlayer(savePosition, saveRotation);

        var savePlayerData = DataManager.Instance.GetData<PlayerSaveData>(DataKey.Player);
        if(savePlayerData != null)
        {
            var saveSetting = savePlayerData.SavePlayerCameraSetting;
            var cameraSetting = new PlayerCameraSetting()
            {
                Position = saveSetting.GetPosition(),
                FollowOffset = saveSetting.GetFollowOffset(),
                FieldOfView = saveSetting.FieldOfView
            };

            EnablePlayerCamera(cameraSetting, true);
        }
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

public struct SavePlayerCameraSetting
{
    public float positionX, positionY, positionZ;
    public float followOffsetX, followOffsetY, followOffsetZ;
    public float FieldOfView;
   
    public SavePlayerCameraSetting(Vector3 position, Vector3 Offset, float fieldOfView)
    {
        positionX = position.x;
        positionY = position.y;
        positionZ = position.z;

        followOffsetX = Offset.x;
        followOffsetY = Offset.y;
        followOffsetZ = Offset.z;

        FieldOfView = fieldOfView;
    }

    public Vector3 GetPosition() => new Vector3(positionX, positionY, positionZ);
    public Vector3 GetFollowOffset() => new Vector3(followOffsetX, followOffsetY, followOffsetZ);
}