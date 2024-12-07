using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    // Singleton instance
    public static ObjectPoolManager Instance { get; private set; }

    [Serializable]
    public class PoolConfig
    {
        public GameObject prefab;
        public int initialPoolSize = 20;
        public int maxPoolSize = 50; 
        public bool isDynamic = true;
        public float cleanupInterval = 5f;  
        public float inactiveDuration = 2f;  
    }

    [SerializeField] private List<PoolConfig> poolConfigs;

    private Dictionary<string, Queue<GameObject>> poolDictionary = new Dictionary<string, Queue<GameObject>>();

    private Dictionary<GameObject, float> objectCreationTimes = new Dictionary<GameObject, float>();

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializePools();

            // Start periodic cleanup
            InvokeRepeating(nameof(PerformGarbageCollection), 0f, 5f * 60f);  // Every 5 minutes
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializePools()
    {
        foreach (var config in poolConfigs)
        {
            CreatePool(config.prefab, config.initialPoolSize);
        }
    }

    public void CreatePool(GameObject prefab, int initialSize)
    {
        string poolKey = prefab.name;

        if (!poolDictionary.ContainsKey(poolKey))
        {
            poolDictionary[poolKey] = new Queue<GameObject>();
        }

        for (int i = 0; i < initialSize; i++)
        {
            GameObject obj = CreateNewPoolObject(prefab);
            ReturnToPool(obj);
        }
    }

    // Create a new object for the pool
    private GameObject CreateNewPoolObject(GameObject prefab)
    {
        GameObject obj = Instantiate(prefab);
        obj.name = prefab.name;

        PooledObject pooledObject = obj.AddComponent<PooledObject>();
        pooledObject.poolName = prefab.name;
        obj.SetActive(false);

        return obj;
    }

    // Get an object from the pool
    public GameObject GetPooledObject(GameObject prefab)
    {
        string poolKey = prefab.name;

        PoolConfig config = poolConfigs.FirstOrDefault(c => c.prefab == prefab);

        if (!poolDictionary.ContainsKey(poolKey))
        {
            CreatePool(prefab, config?.initialPoolSize ?? 10);
        }

        Queue<GameObject> pool = poolDictionary[poolKey];
        GameObject obj;

        if (pool.Count == 0)
        {
            obj = CreateNewPoolObject(prefab);
        }
        else
        {
            obj = pool.Dequeue();
        }

        objectCreationTimes[obj] = Time.time;

        obj.SetActive(true);
        return obj;
    }

    // Return an object to the pool
    public void ReturnToPool(GameObject obj)
    {
        PooledObject pooledObject = obj.GetComponent<PooledObject>();

        if (pooledObject == null)
        {
            Destroy(obj);
            return;
        }

        obj.SetActive(false);

        string poolKey = pooledObject.poolName;
        if (poolDictionary.ContainsKey(poolKey))
        {
            Queue<GameObject> pool = poolDictionary[poolKey];

            PoolConfig config = poolConfigs.FirstOrDefault(c => c.prefab.name == poolKey);
            if (config != null && pool.Count < config.maxPoolSize)
            {
                pool.Enqueue(obj);
            }
            else
            {
                Destroy(obj);
            }
        }
    }

    // Perform garbage collection on object pools
    private void PerformGarbageCollection()
    {
        foreach (var config in poolConfigs)
        {
            string poolKey = config.prefab.name;

            if (poolDictionary.TryGetValue(poolKey, out Queue<GameObject> pool))
            {
                List<GameObject> objectsToRemove = new List<GameObject>();

                foreach (GameObject obj in pool)
                {
                    if (objectCreationTimes.TryGetValue(obj, out float creationTime))
                    {
                        if (Time.time - creationTime > config.inactiveDuration * 60f)
                        {
                            objectsToRemove.Add(obj);
                        }
                    }
                }

                foreach (GameObject obj in objectsToRemove)
                {
                    pool = new Queue<GameObject>(pool.Where(o => o != obj));
                    Destroy(obj);
                }

                poolDictionary[poolKey] = pool;
            }
        }

        objectCreationTimes = objectCreationTimes
            .Where(kvp => poolDictionary.Values.Any(pool => pool.Contains(kvp.Key)))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }
}


public class PooledObject : MonoBehaviour
{
    public string poolName;

   public void ReturnToPool()
    {
        ObjectPoolManager.Instance.ReturnToPool(gameObject);
    }
}