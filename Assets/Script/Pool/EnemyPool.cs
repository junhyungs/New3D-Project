using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : ObjectPool<EnemyPool>, IObjectPool
{
    public void CreatePool(string address, int count = 1)
    {
        if (_poolDictionary.ContainsKey(address))
            return;

    }

    public GameObject DequeueGameObject(string addressablesKey)
    {
        return new GameObject(addressablesKey);
    }

    public void EnqueueGameObject(string addressablesKey, GameObject gameObject)
    {
        
    }

    public void AllDisableGameObject(string address)
    {
        
    }
}
