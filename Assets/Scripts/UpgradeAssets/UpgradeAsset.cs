using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrade Objects")]
[System.Serializable]
public class UpgradeAsset : ScriptableObject
{
    public enum StateUpgrade {Health, Stamina}
    public StateUpgrade type;
    public int upgradeNumber;
    public UpgradeAsset preRequisites;
    public Sprite upgradeSprite;
    public float upgradeStat;
    public int cost;
    public bool isPurchased;
}