using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner_door : MonoBehaviour
{
    [Header("SpawnPoint")]
    [SerializeField] private Transform _spawnPoint;
    private DoorMaterialController _doorMaterialController;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        _doorMaterialController = GetComponentInChildren<DoorMaterialController>();
    }

    public void SpawnEnemy(Spawner spawner, int count)
    {

    }
}
