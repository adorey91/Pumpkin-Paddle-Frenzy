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
    public GameObject spawnPrefab;
    public float spawnWeight;
    public int collectableValue;
}
