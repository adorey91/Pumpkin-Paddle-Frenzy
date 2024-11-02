using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [Header("Obstacle Base Values")]
    private float objectBaseSpeed = 1f;
    [Range(0f, 1f)] public float obstacleSpeedFactor = 0.15f;

    [SerializeField] private List<GameObject> prefabObjects; // List of all prefab objects
    private Dictionary<PoolType, Queue<GameObject>> poolDictionary;
    [SerializeField] private SpawnableObject[] spawnableObject;


    private void Start()
    {
        poolDictionary = new Dictionary<PoolType, Queue<GameObject>>();

        // Group prefabs by their PoolType
        var groupedPrefabs = prefabObjects.GroupBy(prefab => prefab.GetComponent<SpawnableBehaviour>().GetSpawnableObject().type);

        foreach (var group in groupedPrefabs)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();
            PoolType poolType = group.Key;

            foreach (GameObject prefab in group)
            {
                SpawnableBehaviour obstacle = prefab.GetComponent<SpawnableBehaviour>();
                for (int i = 0; i < obstacle.GetSpawnableObject().size; i++)
                {
                    GameObject obj = Instantiate(prefab, transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
            }

            ShuffleQueue(objectPool);

            // Add the pool to the dictionary with PoolType as key
            poolDictionary.Add(poolType, objectPool);
        }
    }

    #region ActionsEnableDisable
    private void OnEnable()
    {
        Actions.OnSpawn += HandleSpawnEvent;
        Actions.OnReturn += ReturnToPool;
        Actions.SpeedChange += UpdatePoolSpeed;
        Actions.ReturnAllToPool += ReturnAllToPool;
        Actions.OnGameplay += ShufflePools;
    }

    private void OnDisable()
    {
        Actions.OnSpawn -= HandleSpawnEvent;
        Actions.OnReturn -= ReturnToPool;
        Actions.SpeedChange -= UpdatePoolSpeed;
        Actions.ReturnAllToPool -= ReturnAllToPool;
        Actions.OnGameplay -= ShufflePools;
    }
    #endregion

    // handles the spawning event
    private void HandleSpawnEvent(PoolType type)
    {
        SpawnFromPool(type);
    }

    // Spawns an INACTIVE object from a designated pool, sets the rotation and position
    private GameObject SpawnFromPool(PoolType type)
    {
        float leftSpawnX = -6f;
        float rightSpawnX = 6f;
        float randomXSpawnMin = -6.7f;
        float randomXSpawnMax = 6.7f;
        float randomX = Random.Range(randomXSpawnMin, randomXSpawnMax);

        if (!poolDictionary.ContainsKey(type))
        {
            Debug.LogWarning("Pool with tag " + type + " doesn't exist");
            return null;
        }

        Queue<GameObject> pool = poolDictionary[type];
        GameObject objectToSpawn = null;

        // Find an inactive object in the pool
        for (int i = 0; i < pool.Count; i++)
        {
            GameObject obj = pool.Dequeue();
            if (!obj.activeInHierarchy)
            {
                objectToSpawn = obj;
                break;
            }
            else
            {
                // Re-enqueue the active object to check later
                pool.Enqueue(obj);
            }
        }

        // If no inactive object was found, return null or handle this case
        if (objectToSpawn == null)
        {
            Debug.LogWarning("No inactive objects available in pool for type " + type);
            return null;
        }

        // Configure object settings and spawn position
        objectToSpawn.SetActive(true);
        Vector2 spawnPosition = transform.position;
        Quaternion spawnRotation = Quaternion.identity;

        switch (type)
        {
            case PoolType.FinishLine:
                spawnPosition = transform.position;
                break;
            case PoolType.Kayak:
                spawnPosition = new Vector2((randomX <= 0 ? leftSpawnX : rightSpawnX), transform.position.y);
                spawnRotation = randomX <= 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
                break;
            default:
                spawnPosition = new Vector2(randomX, spawnPosition.y);
                break;
        }

        objectToSpawn.transform.position = spawnPosition;
        objectToSpawn.transform.rotation = spawnRotation;

        return objectToSpawn;
    }

    // Returns all children under this to their pools and sets them as inactive.
    private void ReturnAllToPool()
    {
        // Loop through the immediate children of this transform
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i); // Get immediate child

            SpawnableBehaviour spawnableBehaviour = child.GetComponent<SpawnableBehaviour>();

            // Check if the component exists
            if (spawnableBehaviour != null)
            {
                SpawnableObject spawnObj = spawnableBehaviour.GetSpawnableObject();

                // Check if the SpawnableObject is valid
                if (spawnObj != null)
                {
                    PoolType poolObject = spawnObj.type; // Access the PoolType from the SpawnableObject
                    ReturnToPool(poolObject, child.gameObject); // Return the child to the appropriate pool
                }
            }
        }
    }

    // Returns the gameObject back to the pool and sets it inactive
    private void ReturnToPool(PoolType type, GameObject objectSpawned)
    {
        objectSpawned.SetActive(false);
        poolDictionary[type].Enqueue(objectSpawned);
        //Debug.Log(objectSpawned + "returned to pool");
    }

    // Shuffles each pool of queues
    private void ShufflePools()
    {
        if (poolDictionary == null)
        {
            //Debug.LogWarning("poolDictionary is not initialized.");
            return;
        }

        foreach (var pool in poolDictionary)
        {
            Queue<GameObject> queue = pool.Value;
            if (queue == null)
            {
                //Debug.LogWarning($"Queue for {pool.Key} is null.");
                continue;
            }
            ShuffleQueue(queue);
        }
    }


    // Method to shuffle Queue with active objects at the end
    private void ShuffleQueue(Queue<GameObject> queue)
    {
        List<GameObject> list = queue.ToList();

        // Shuffle the list
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            GameObject temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }

        // Clear the original queue and re-enqueue the shuffled list
        queue.Clear();
        foreach (GameObject obj in list)
        {
            queue.Enqueue(obj);
        }
    }

    // Updates pool speed based on timeAlive value
    private void UpdatePoolSpeed(float timeAlive)
    {
        float newSpeed = objectBaseSpeed * Mathf.Pow(timeAlive, obstacleSpeedFactor);

        foreach (SpawnableObject spawnable in spawnableObject)
        {
            spawnable.speed = newSpeed;
        }
    }
}
