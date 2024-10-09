using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

[CreateAssetMenu(fileName = "Upgrade", menuName = "Upgrade Objects")]
[System.Serializable]
public class UpgradeAsset : ScriptableObject
{
    public enum StateUpgrade {Health, Stamina}
    public StateUpgrade type;
    public int number;
    public UpgradeAsset preRequisites;
    public Sprite newSprite;
    public AnimationClip newAnimation;
    public float newStats;
    public int cost;
    public bool isPurchased;
}