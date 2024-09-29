using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{

    [Header("Upgrades")]
    [SerializeField] private UpgradeAsset[] healthUpgrades;
    [SerializeField] private UpgradeAsset[] staminaUpgrades;

    [Header("Upgrade Buttons")]
    [SerializeField] private Button[] healthButtons;
    [SerializeField] private Button[] staminaButtons;

    [Header("Checkmark Images")]
    [SerializeField] private GameObject[] healthChecks;
    [SerializeField] private GameObject[] staminaChecks;

    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private HealthSystem healthSystem;


    private void Start()
    {
        for(int i = 0; i < healthUpgrades.Length; i++)
        {
            healthUpgrades[i].isPurchased = false;
            healthButtons[i].interactable = false;
        }
        for (int i = 0;i < staminaUpgrades.Length;i++)
        {
            staminaUpgrades[i].isPurchased = false;
            staminaButtons[i].interactable = false;
        }
    }

    public void PurchaseUpgrade(UpgradeAsset upgradeAsset)
    {
        if(scoreManager.appleCount >= upgradeAsset.cost && !upgradeAsset.isPurchased)
        {
            scoreManager.appleCount -= upgradeAsset.cost;
            scoreManager.UpdateText();
            upgradeAsset.isPurchased = true;

            if(upgradeAsset.upgrade == UpgradeAsset.StateUpgrade.Health)
            {
                healthSystem.curHealthUpgrade = upgradeAsset;
                healthSystem.maxHealth = (int)upgradeAsset.upgradeStat;
            }
            else
            {
                healthSystem.curStaminaUpgrade = upgradeAsset;
                healthSystem.staminaDrain = upgradeAsset.upgradeStat;
            }

            UpdateAllButtons();
        }
    }

    public void UpdateAllButtons()
    {
        // Update Health Upgrade Buttons
        for (int i = 0; i < healthUpgrades.Length; i++)
        {
            if (healthUpgrades[i].isPurchased)
            {
                healthButtons[i].interactable = false; // Disable button if purchased
                healthChecks[i].SetActive(true);
            }
            else
            {
                // Check if player can afford the upgrade
                if (scoreManager.appleCount >= healthUpgrades[i].cost)
                {
                    // Only check prerequisites if they exist
                    if (healthUpgrades[i].preRequisites == null || healthUpgrades[i].preRequisites.isPurchased)
                    {
                        healthButtons[i].interactable = true; // Enable button if prerequisites are met or none
                    }
                    else
                    {
                        healthButtons[i].interactable = false; // Disable button if prerequisites are not met
                    }
                }
                else
                {
                    healthButtons[i].interactable = false; // Disable button if not enough currency
                }
            }
        }

        // Update Stamina Upgrade Buttons
        for (int i = 0; i < staminaUpgrades.Length; i++)
        {
            if (staminaUpgrades[i].isPurchased)
            {
                staminaButtons[i].interactable = false; // Disable button if purchased
                staminaChecks[i].SetActive(true);
            }
            else
            {
                // Check if player can afford the upgrade
                if (scoreManager.appleCount >= staminaUpgrades[i].cost)
                {
                    // Only check prerequisites if they exist
                    if (staminaUpgrades[i].preRequisites == null || staminaUpgrades[i].preRequisites.isPurchased)
                    {
                        staminaButtons[i].interactable = true; // Enable button if prerequisites are met or none
                    }
                    else
                    {
                        staminaButtons[i].interactable = false; // Disable button if prerequisites are not met
                    }
                }
                else
                {
                    staminaButtons[i].interactable = false; // Disable button if not enough currency
                }
            }
        }
    }

}