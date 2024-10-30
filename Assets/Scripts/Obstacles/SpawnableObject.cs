using UnityEngine;

public enum PoolType { Obstacle, Collectable, FinishLine, Kayak,}

[CreateAssetMenu(fileName = "SpawnableObject")]
public class SpawnableObject : ScriptableObject
{
    public PoolType type;
    public float speed;
    public int size; // spawn count
    public int collectableValue;
}
