using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton_MonoBehaviour<GameManager>
{
    [Header("PlayerPrefab")]
    [SerializeField] private GameObject _playerPrefab;

    public bool PlayerDeath { get; private set; }

    private void Start()
    {
        InitializePlayerSetting();
    }

    private void InitializePlayerSetting()
    {
        var playerObject = Instantiate(_playerPrefab);
        PlayerManager.Instance.SetPlayer(playerObject);
        InventoryManager.Instance.InitializeInventory();
        playerObject.SetActive(false);
    }
}
