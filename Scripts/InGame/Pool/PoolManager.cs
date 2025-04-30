using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager
{
    public static PoolManager Instance;

    public Dictionary<string, Pool<PoolableMono>> _pools = new Dictionary<string, Pool<PoolableMono>>();
    private Transform _trmParent;

    public PoolManager(Transform trm)
    {
        _trmParent = trm;
    }

    public void CreatePool(PoolableMono prefab, int count = 10)
    {
        Pool<PoolableMono> pool = new Pool<PoolableMono>(prefab, _trmParent, count);
        _pools.Add(prefab.gameObject.name, pool);
    }

    public PoolableMono Pop(string name, Vector3 pos)
    {
        if (!_pools.ContainsKey(name))
        {
            Debug.LogError($"Prefab dose not exist on pool : {name}");  //說除儀 煎斜
            return null;
        }

        PoolableMono item = _pools[name].Pop();
        item.Init();
        item.gameObject.transform.position = pos;
        return item;
    }
    public PoolableMono Pop(string name, Vector3 pos, Vector2 force, float rotation)
    {
        if (!_pools.ContainsKey(name))
        {
            Debug.LogError($"Prefab dose not exist on pool : {name}");  //說除儀 煎斜
            return null;
        }

        PoolableMono item = _pools[name].Pop();
        item.Init();
        item.gameObject.transform.position = pos;
        item.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        item.GetComponent<Rigidbody2D>().velocity = force;
        return item;
    }
    public PoolableMono Pop(string name, Vector3 pos, float rotation, float damege, float knockBack)
    {
        if (!_pools.ContainsKey(name))
        {
            Debug.LogError($"Prefab dose not exist on pool : {name}");  //說除儀 煎斜
            return null;
        }

        PoolableMono item = _pools[name].Pop();
        item.Init();
        item.gameObject.transform.position = pos;
        item.gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, 0, rotation));
        item.GetComponent<BulletParticle>().Set(knockBack, damege);
        return item;
    }


    public void Push(PoolableMono obj)
    {
        _pools[obj.name].Push(obj);
    }
}
