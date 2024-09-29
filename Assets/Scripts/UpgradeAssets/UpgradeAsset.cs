using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrade Objects")]
[Serializable]
public class UpgradeAsset : ScriptableObject
{
    public enum StateUpgrade {Health, Stamina}
    public StateUpgrade upgrade;
    public int upgradeNumber;
    public UpgradeAsset preRequisites;
    public Sprite upgradeSprite;
    public float upgradeStat;
    public int cost;
    public bool isPurchased;
}