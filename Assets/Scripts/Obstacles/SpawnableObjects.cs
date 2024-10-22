using UnityEngine;


[CreateAssetMenu(fileName = "SpawnableObject")]
public class SpawnableObjects : ScriptableObject
{
    public enum ObjectType
    {
        Obstacle,
        Collectable,
        FinishLine,
    }
    public ObjectType type;
    public GameObject[] spawnPrefab;
    [Range(0f, 1f)]
    public float minWeight;
    [Range(0f, 1f)]
    public float maxWeight;

    public int collectableValue;

    public GameObject GetSpawn()
    {
        int random = Random.Range(0, spawnPrefab.Length);
        return spawnPrefab[random];
    }

    public float GetWeight()
    {
        return Random.Range(minWeight, maxWeight);
    }
}
