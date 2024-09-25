using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrade Objects")]
public class UpgradeAsset : ScriptableObject
{
    public enum StateUpgrade {Health, Stamina}
    public StateUpgrade upgrade;
    public Sprite upgradeSprite;
    public float upgradeStatTo;
}
