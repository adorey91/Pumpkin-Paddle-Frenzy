using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int healthAmount;
    public float staminaDrain;
    public int appleCount;
    public int attemptsMade;

    public List<string> purchasedUpgrades = new List<string>(); // saving names of upgrades
}