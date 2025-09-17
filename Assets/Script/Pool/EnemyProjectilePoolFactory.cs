using Cysharp.Threading.Tasks;
using EnemyComponent;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class EnemyProjectilePoolFactory : Singleton_MonoBehaviour<EnemyProjectilePoolFactory>
{
    public async UniTask<EnemyProjectilePool> CreateEnemyProjectilePoolAsync(Transform parentTransform, string addressablesKey, int count = 5)
    {
        var name = TrimStart(addressablesKey);

        var poolObject = new GameObject(name + "Pool");
        poolObject.transform.SetParent(parentTransform);
        poolObject.transform.localPosition = Vector3.zero;

        var pool = poolObject.AddComponent<EnemyProjectilePool>();
        return await LoadProjectileAsync(pool, addressablesKey, name, count);
    }

    private async UniTask<EnemyProjectilePool> LoadProjectileAsync(EnemyProjectilePool pool, string addressablesKey,
        string name, int count)
    {
        var handle = Addressables.LoadAssetAsync<GameObject>(addressablesKey);
        await handle.ToUniTask();
        
        var prefab = handle.Result;
        prefab.name = name;

        var container = new EnemyProjectileContainer(pool.transform);
        container.SaveItem = prefab;

        var parentTransform = pool.transform;
        for(int i = 0; i < count; i++)
        {
            var projectile = Instantiate(prefab, parentTransform);

            var enemyProjectileComponent = projectile.GetComponent<EnemyProjectile>();
            if(enemyProjectileComponent != null)
            {
                container.AddDisableProjectile(enemyProjectileComponent);
                enemyProjectileComponent.SetEnemyProjectilePool(pool);
            }
                
            projectile.SetActive(false);
            container.Enqueue(projectile);
        }

        pool.Setcontainer(container);
        return pool;
    }

    private string TrimStart(string targetString)
    {
        var trim = "Prefab/";
        if (targetString.StartsWith(trim))
            return targetString.Substring(trim.Length);

        return targetString;
    }
}
