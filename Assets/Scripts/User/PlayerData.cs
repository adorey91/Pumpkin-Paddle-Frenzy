using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerData : MonoBehaviour
{
    public UpgradeAsset currentHealthUpgrade;
    public UpgradeAsset currentStaminaUpgrade;
    public int healthAmount;
    public int staminaAmount;
    public int appleCount;
}