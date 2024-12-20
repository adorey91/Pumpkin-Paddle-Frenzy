using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    [Header("Obstacle Base Values")]
    private float objectBaseSpeed = 1.5f;
    [Range(0f, 1f)] public float obstacleSpeedFactor = 0.15f;

    [SerializeField] private List<GameObject> prefabObjects; // List of all prefab objects
    private Dictionary<PoolType, Queue<GameObject>> poolDictionary;
    [SerializeField] private SpawnableObject[] spawnableObject;

    [Header("Settings For Apple Collection")]
    [SerializeField] private GameObject splitAppleObject;
    [SerializeField] private Transform appleTargetPos;
    private Queue<GameObject> splitApplePool;
    [SerializeField] private ScoreManager scoreManager;

    [Header("Settings for Energy Collection")]
    [SerializeField] private GameObject energyObject;
    [SerializeField] private Transform energyTargetPos;
    private Queue<GameObject> energyPool;


    [Header("Settings for Speed Transition")]
    [SerializeField] private float smoothSpeedTransitionTime = 10f;
    private float currentSpeed;
    private float newSpeed;
    private float speedStepProgress = 0f;
    private bool speedIncreasing = false;

    bool isCollected = false;
    bool forceSpawn = false;
    private float playerX_Pos;

    private void Awake()
    {
        splitApplePool = new Queue<GameObject>();
        energyPool = new Queue<GameObject>();

        // Populate the pool for split apples
        for (int i = 0; i < 12; i++) // Adjust the number as needed
        {
            GameObject splitApple = Instantiate(splitAppleObject, transform);
            splitApple.SetActive(false);
            splitApplePool.Enqueue(splitApple);
        }

        // Populate the pool for energy
        for (int i = 0; i < 12; i++) // Adjust the number as needed
        {
            GameObject energy = Instantiate(energyObject, transform);
            energy.SetActive(false);
            energyPool.Enqueue(energy);
        }
    }

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

    private void Update()
    {
        if (speedIncreasing)
        {
            IncreasingGradualSpeed();

            foreach (SpawnableObject spawnable in spawnableObject)
            {
                spawnable.speed = currentSpeed;
            }
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
        Actions.ForceSpawn += ForceSpawn;
    }

    private void OnDisable()
    {
        Actions.OnSpawn -= HandleSpawnEvent;
        Actions.OnReturn -= ReturnToPool;
        Actions.SpeedChange -= UpdatePoolSpeed;
        Actions.ReturnAllToPool -= ReturnAllToPool;
        Actions.OnGameplay -= ShufflePools;
        Actions.ForceSpawn -= ForceSpawn;
    }
    #endregion

    #region Spawning
    private void ForceSpawn(float playerPosition)
    {
        forceSpawn = true;
        playerX_Pos = playerPosition;
        HandleSpawnEvent(PoolType.Obstacle);
    }

    // handles the spawning event
    private void HandleSpawnEvent(PoolType type)
    {
        SpawnFromPool(type);
    }


    // Spawns an inactive object from a designated pool, sets the rotation and position
    private GameObject SpawnFromPool(PoolType type)
    {
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
        SpawnableObject spawnable = objectToSpawn.GetComponent<SpawnableBehaviour>().GetSpawnableObject();
        float randomX = Random.Range(spawnable.minXspawn, spawnable.maxXspawn);

        switch (type)
        {
            case PoolType.FinishLine:
                spawnPosition = transform.position;
                break;
            case PoolType.Kayak:
                spawnPosition = new Vector2((randomX <= 0 ? spawnable.kayakXminSpawn : spawnable.kayakXmaxSpawn), transform.position.y);
                spawnRotation = randomX <= 0 ? Quaternion.Euler(0, 180, 0) : Quaternion.identity;
                break;
            case PoolType.Collectable:
                objectToSpawn.transform.localScale = new Vector3(0.4f, 0.4f, 0.5f);
                spawnPosition = new Vector2(randomX, spawnPosition.y);
                break;
            default:
                spawnPosition = new Vector2(randomX, spawnPosition.y);
                break;
        }

        objectToSpawn.transform.rotation = spawnRotation;
        if (forceSpawn)
            objectToSpawn.transform.position = new Vector2(playerX_Pos, spawnPosition.y);
        else
            objectToSpawn.transform.position = spawnPosition;


        forceSpawn = false;

        return objectToSpawn;
    }
    #endregion

    #region Return
    // Returns all children under this to their pools and sets them as inactive.
    private void ReturnAllToPool()
    {
        // Loop through the immediate children of this transform
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i); // Get immediate child
            GameObject childObject = child.gameObject; // turn off child before returning

            if (childObject.activeInHierarchy)
            {
                SpawnableBehaviour spawnableBehaviour = child.GetComponent<SpawnableBehaviour>();
                childObject.SetActive(false);

                // Check if the component exists
                if (spawnableBehaviour != null)
                {
                    SpawnableObject spawnObj = spawnableBehaviour.GetSpawnableObject();

                    // Check if the SpawnableObject is valid
                    if (spawnObj != null)
                    {
                        PoolType poolObject = spawnObj.type; // Access the PoolType from the SpawnableObject
                        ReturnToPool(poolObject, child.gameObject, false); // Return the child to the appropriate pool
                    }
                }
            }
        }
    }

    // Returns the gameObject back to the pool and sets it inactive
    private void ReturnToPool(PoolType type, GameObject objectSpawned, bool collected)
    {
        isCollected = collected;

        if (objectSpawned.activeInHierarchy)
        {
            if (type == PoolType.Collectable & isCollected)
            {
                if (objectSpawned.name == "Apple(Clone)" || objectSpawned.name == "GoldenApple(Clone)")
                    CollectAppleMovement(objectSpawned);
                else
                    CollectEnergyMovement(objectSpawned);
            }
            else
                objectSpawned.SetActive(false);

            poolDictionary[type].Enqueue(objectSpawned);
        }
    }
    #endregion

    #region Shuffle
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
    #endregion

    #region SplitApple_Collection
    private GameObject GetSplitAppleFromPool()
    {
        if (splitApplePool.Count > 0)
        {
            GameObject apple = splitApplePool.Dequeue();
            apple.transform.localScale = new Vector3(0.4f, 0.4f, 0.5f);
            apple.SetActive(true);
            //Debug.Log("Retrieved energyCan from pool: " + energyCan.name); // Debugging message
            return apple;
        }
        //Debug.LogWarning("No apples available in split pool!"); // Debugging message
        return null;
    }

    private void ReturnSplitAppleToPool(GameObject apple)
    {
        apple.SetActive(false);
        splitApplePool.Enqueue(apple);
    }

    private void CollectAppleMovement(GameObject apple)
    {
        apple.SetActive(false);
        Transform applePos = apple.transform;

        if (apple.name == "Apple(Clone)")
        {
            GameObject singleApple = GetSplitAppleFromPool();
            singleApple.transform.position = applePos.position + new Vector3(0.4f, 0, 0);

            // Start moving each energyCan towards the target
            StartCoroutine(MoveAndScaleToTarget(singleApple));
        }
        else
        {
            GameObject apple1 = GetSplitAppleFromPool();
            GameObject apple2 = GetSplitAppleFromPool();
            GameObject apple3 = GetSplitAppleFromPool();

            apple1.transform.position = applePos.position + new Vector3(-0.4f, 0, 0);
            apple2.transform.position = applePos.position + new Vector3(0.4f, 0, 0);
            apple3.transform.position = applePos.position + new Vector3(0, 0.4f, 0);

            // Start moving each energyCan towards the target
            StartCoroutine(MoveAndScaleToTarget(apple1));
            StartCoroutine(MoveAndScaleToTarget(apple2));
            StartCoroutine(MoveAndScaleToTarget(apple3));
        }
    }


    private IEnumerator MoveAndScaleToTarget(GameObject smallApple)
    {
        Vector3 startPosition = smallApple.transform.position;
        Vector3 endPosition = appleTargetPos.position;

        float moveDuration = 1f; // Adjust based on your desired speed
        float elapsedTime = 0f;
        float arcHeight = 2f; // Adjust for the height of the curve

        Vector3 targetScale = Vector3.zero;
        float scaleSpeed = 2f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float t = elapsedTime / moveDuration;
            t = Mathf.Clamp01(t);

            // Interpolate position along a parabolic path
            float height = Mathf.Sin(t * Mathf.PI) * arcHeight; // Sin curve for the height
            Vector3 nextPosition = Vector3.Lerp(startPosition, endPosition, t);
            nextPosition.y += height;

            smallApple.transform.position = nextPosition;

            // Scale down the energyCan
            smallApple.transform.localScale = Vector3.Lerp(smallApple.transform.localScale, targetScale, Time.unscaledDeltaTime * scaleSpeed);

            yield return null;
        }

        // Ensure the energyCan reaches the exact end position
        smallApple.transform.position = endPosition;
        smallApple.transform.localScale = targetScale;

        // Return the small energyCan to its pool
        ReturnSplitAppleToPool(smallApple);
    }
    #endregion

    #region Energy_Collection
    private GameObject GetEnergyFromPool()
    {
        if (energyPool.Count > 0)
        {
            GameObject energy = energyPool.Dequeue();
            energy.transform.localScale = new Vector3(0.4f, 0.4f, 0.5f);
            energy.SetActive(true);
            //Debug.Log("Retrieved energyCan from pool: " + energyCan.name); // Debugging message
            return energy;
        }
        //Debug.LogWarning("No energy available in pool!"); // Debugging message
        return null;
    }

    private void ReturnEnergyToPool(GameObject energyCan)
    {
        energyCan.SetActive(false);
        energyPool.Enqueue(energyCan);
    }

    private void CollectEnergyMovement(GameObject energyCan)
    {
        energyCan.SetActive(false);
        Transform energyPos = energyCan.transform;

        GameObject energy = GetEnergyFromPool();
        energy.transform.position = energyPos.position + new Vector3(0.4f, 0, 0);

        // Start moving each energyCan towards the target
        StartCoroutine(MoveAndScaleEnergyToTarget(energy));
    }


    private IEnumerator MoveAndScaleEnergyToTarget(GameObject energyCan)
    {
        Vector3 startPosition = energyCan.transform.position;
        Vector3 endPosition = energyTargetPos.position;

        float moveDuration = 1f; // Adjust based on your desired speed
        float elapsedTime = 0f;
        float arcHeight = 2f; // Adjust for the height of the curve

        Vector3 targetScale = Vector3.zero;
        float scaleSpeed = 2f;

        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;

            float t = elapsedTime / moveDuration;
            t = Mathf.Clamp01(t);

            // Interpolate position along a parabolic path
            float height = Mathf.Sin(t * Mathf.PI) * arcHeight; // Sin curve for the height
            Vector3 nextPosition = Vector3.Lerp(startPosition, endPosition, t);
            nextPosition.y += height;

            energyCan.transform.position = nextPosition;

            // Scale down the energyCan
            energyCan.transform.localScale = Vector3.Lerp(energyCan.transform.localScale, targetScale, Time.unscaledDeltaTime * scaleSpeed);

            yield return null;
        }

        // Ensure the energyCan reaches the exact end position
        energyCan.transform.position = endPosition;
        energyCan.transform.localScale = targetScale;

        // Return the small energyCan to its pool
        ReturnEnergyToPool(energyCan);
    }

    #endregion

    #region PoolSpeedUpdate
    // Updates pool currentSpeed based on timeAlive value
    private void UpdatePoolSpeed(float timeAlive)
    {
        newSpeed = objectBaseSpeed * Mathf.Pow(timeAlive, obstacleSpeedFactor);

        // OnGameplayReset interpolation progress
        speedStepProgress = 0f;
        speedIncreasing = true;
    }

    private void IncreasingGradualSpeed()
    {
        // Increment progress proportionally based on smoothSpeedTransitionTime
        speedStepProgress += Time.deltaTime / smoothSpeedTransitionTime;
        speedStepProgress = Mathf.Clamp01(speedStepProgress);

        // Smoothly interpolate between currentSpeed and newSpeed
        currentSpeed = Mathf.SmoothStep(currentSpeed, newSpeed, speedStepProgress);

        // Stop increasing when close enough to newSpeed
        if (Mathf.Abs(currentSpeed - newSpeed) < 0.001f)
        {
            currentSpeed = newSpeed; // Snap to target
            speedIncreasing = false;
        }
    }
    #endregion
}
