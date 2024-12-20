using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int healthAmount;
    public float staminaDrain;
    public int availableAppleCount;
    public int lifetimeAppleCount;
    public int attemptsMade;
    public bool onScreenPause;
    public bool onScreenControls;
    public int energyAmount;

    public List<string> purchasedUpgrades = new List<string>(); // saving names of upgrades
}