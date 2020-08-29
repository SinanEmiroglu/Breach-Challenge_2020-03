using System.Collections.Generic;
using UnityEngine;

public class Pool : MonoBehaviour
{
    public static Dictionary<PooledMonoBehaviour, Pool> Pools = new Dictionary<PooledMonoBehaviour, Pool>();

    private Queue<PooledMonoBehaviour> _objects = new Queue<PooledMonoBehaviour>();
    private PooledMonoBehaviour _prefab;

    public static Pool GetPool(PooledMonoBehaviour prefab)
    {
        //if that specific prefab included in the dictionary, then just return.
        if (Pools.ContainsKey(prefab))
        {
            return Pools[prefab];
        }
        //Otherwise, create a new GameObject and add it to the dictionary
        var poolGameObject = new GameObject("Pool - " + prefab.name);
        var pool = poolGameObject.AddComponent<Pool>();
        pool._prefab = prefab;

        Pools.Add(prefab, pool);
        return pool;
    }

    public T Get<T>() where T : PooledMonoBehaviour
    {
        if (_objects.Count == 0)
        {
            GrowPool();
        }

        var pooledObject = _objects.Dequeue();

        return pooledObject as T;
    }

    private void GrowPool()
    {
        for (int i = 0; i < _prefab.InitialPoolSize; i++)
        {
            var pooledObject = Instantiate(_prefab) as PooledMonoBehaviour;

            pooledObject.gameObject.name += " " + i;

            pooledObject.OnReturnToPool += AddObjectToAvailableQueue;

            pooledObject.transform.SetParent(transform);
            pooledObject.gameObject.SetActive(false);
        }
    }

    private void AddObjectToAvailableQueue(PooledMonoBehaviour pooledObject)
    {
        pooledObject.transform.SetParent(transform);
        _objects.Enqueue(pooledObject);
    }
}