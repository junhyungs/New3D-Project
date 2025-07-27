using EnemyComponent;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectContainer
{
    public ObjectContainer(Transform containerTransform)
    {
        _objectQueue = new Queue<GameObject>();
        ContainerTransform = containerTransform;
    }

    protected Queue<GameObject> _objectQueue;
    public Transform ContainerTransform { get; private set; }
    public GameObject SaveItem { get; set; }
    public int QueueCount => _objectQueue.Count;

    public virtual GameObject Dequeue()
    {
        var gameObject = _objectQueue.Dequeue();
        return gameObject;
    }

    public virtual void Enqueue(GameObject gameObject)
    {
        _objectQueue.Enqueue(gameObject);
    }
}

public class EnemyProjectileContainer : ObjectContainer
{
    public EnemyProjectileContainer(Transform poolTransform) : base(poolTransform)
    {
        _disableProjectiles = new List<IDisableProjectile>();
    }

    private List<IDisableProjectile> _disableProjectiles;

    public void AddDisableProjectile(IDisableProjectile projectile) =>
        _disableProjectiles.Add(projectile);

    public override GameObject Dequeue()
    {
        if(_objectQueue.Count != 0)
            return _objectQueue.Dequeue();
        return null;
    }

    public void AllDisable()
    {
        for (int i = 0; i < _disableProjectiles.Count; i++)
            _disableProjectiles[i].Disable();
    }
}

public class PlayerWeaponObjectContainer
{
    public PlayerWeaponObjectContainer(Transform poolTransform, int arrayLength)
    {
        ObjectArray = new GameObject[arrayLength];
        PoolTransform = poolTransform;
    }

    public GameObject[] ObjectArray { get; private set; }
    public Transform PoolTransform { get; private set; }
}
