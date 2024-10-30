using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Pool;

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
            Debug.Log(poolType);foreach (GameObject prefab in group)
            {
                SpawnableBehaviour obstacle = prefab.GetComponent<SpawnableBehaviour>();
                for (int i = 0; i < obstacle.GetSpawnableObject().size; i++)
                {
                    GameObject obj = Instantiate(prefab, transform);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }
            }

            // Shuffle the pool
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
        Actions.LevelChange += ShufflePools;
    }

    private void OnDisable()
    {
        Actions.OnSpawn -= HandleSpawnEvent;
        Actions.OnReturn -= ReturnToPool;
        Actions.SpeedChange -= UpdatePoolSpeed;
        Actions.ReturnAllToPool -= ReturnAllToPool;
        Actions.LevelChange -= ShufflePools;
    }
    #endregion

    private void HandleSpawnEvent(PoolType type)
    {
        SpawnFromPool(type);
    }

    private GameObject SpawnFromPool(PoolType type)
    {
        if (!poolDictionary.ContainsKey(type))
        {
            Debug.LogWarning("Pool with tag " + type + " doesn't exist");
            return null;
        }
        // The range on the x axis for where the obstacles can spawn
        float randomX = Random.Range(-6.7f, 6.7f);


        //THIS IS THE PROBLEM. IT'S DEQUEUING THE PREVIOUS VALUE SOMETIMES.
        GameObject objectToSpawn = poolDictionary[type].Dequeue();
        objectToSpawn.SetActive(true);

        if (poolDictionary[type] == poolDictionary[PoolType.FinishLine])
        {
            objectToSpawn.transform.position = new Vector2(0, 7f);
            objectToSpawn.transform.rotation = Quaternion.identity;

        }
        else if (poolDictionary[type] == poolDictionary[PoolType.Kayak])
        {
            if (randomX <= 0)
            {
                randomX = -6f;
                objectToSpawn.transform.rotation *= Quaternion.Euler(0, 180f, 0);
            }
            else
                randomX = 6f;
        }
        else
        {
            objectToSpawn.transform.position = new Vector2(randomX, 7f);

        }

        if (poolDictionary[type] != poolDictionary[PoolType.Kayak] || randomX == 6f)
            objectToSpawn.transform.rotation = Quaternion.identity;

        Debug.Log(objectToSpawn);
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
                // Get the SpawnableObject
                SpawnableObject spawnObj = spawnableBehaviour.GetSpawnableObject();

                // Check if the SpawnableObject is valid
                if (spawnObj != null)
                {
                    PoolType poolObject = spawnObj.type; // Access the PoolType from the SpawnableObject
                    ReturnToPool(poolObject, child.gameObject); // Return the child to the appropriate pool
                }
                else
                    Debug.LogWarning($"SpawnableObject is null for {child.name}. Ensure it is assigned correctly.");
            }
            else
                Debug.LogWarning($"SpawnableBehaviour component not found on {child.name}.");
        }
    }

    private void ReturnToPool(PoolType type, GameObject objectSpawned)
    {
        objectSpawned.SetActive(false);
        poolDictionary[type].Enqueue(objectSpawned);
        Debug.Log(objectSpawned + "returned to pool");
    }

    // Shuffles each pool of queues
    private void ShufflePools(int level)
    {
        foreach (var pool in poolDictionary)
        {
            Queue<GameObject> queue = pool.Value;
            ShuffleQueue(queue);
        }
    }

    // Method to shuffle Queue with active objects at the end
    private void ShuffleQueue(Queue<GameObject> queue)
    {
        // Separate active and inactive objects
        List<GameObject> inactiveList = new List<GameObject>();
        List<GameObject> activeList = new List<GameObject>();

        // Sorts objects into active and inactive lists
        foreach (GameObject obj in queue)
        {
            if (obj.activeSelf)  
                activeList.Add(obj);
            else
                inactiveList.Add(obj);// Collect inactive objects
        }

        // Shuffle only the inactive list
        for (int i = 0; i < inactiveList.Count; i++)
        {
            int randomIndex = Random.Range(i, inactiveList.Count);
            GameObject temp = inactiveList[i];
            inactiveList[i] = inactiveList[randomIndex];
            inactiveList[randomIndex] = temp;
        }

        // Clear the original queue and re-enqueue shuffled inactive objects, then active objects
        queue.Clear();
        foreach (GameObject obj in inactiveList)
        {
            queue.Enqueue(obj);
        }
        foreach (GameObject obj in activeList)
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
