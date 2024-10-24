using UnityEngine;

public enum PoolType { Obstacle, Collectable, FinishLine,}


[CreateAssetMenu(fileName = "SpawnableObject")]
public class SpawnableObject : ScriptableObject
{
    public PoolType type;
    public float speed;
    public int size; // spawn count
    public int collectableValue;
}