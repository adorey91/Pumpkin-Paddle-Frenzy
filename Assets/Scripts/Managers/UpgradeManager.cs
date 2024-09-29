using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{

    [Header("Upgrades")]
    public UpgradeAsset[] allUpgrades;
    public UpgradeAsset[] healthUpgrades;
    public UpgradeAsset[] staminaUpgrades;

    [Header("Upgrade Buttons")]
    [SerializeField] private Button[] healthButtons;
    [SerializeField] private Button[] staminaButtons;

    [Header("Checkmark Images")]
    [SerializeField] private GameObject[] healthChecks;
    [SerializeField] private GameObject[] staminaChecks;

    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private HealthSystem healthSystem;

    public List<UpgradeAsset> purchasedUpgrades = new List<UpgradeAsset>(); // All purchased upgrades should go to this list

    private void Start()
    {
        UpdateAllButtons();
    }


    public void PurchaseUpgrade(UpgradeAsset upgradeAsset)
    {
        if (scoreManager.appleCount >= upgradeAsset.cost && !upgradeAsset.isPurchased)
        {
            scoreManager.appleCount -= upgradeAsset.cost;
            scoreManager.UpdateText();

            // Add to the list of purchased upgrades if not already in it
            if (!purchasedUpgrades.Contains(upgradeAsset))
                purchasedUpgrades.Add(upgradeAsset);

            ApplyUpgradeToPlayer(upgradeAsset);
        }
    }

    #region PlayerUpgrade
    public void ApplyUpgradeToPlayer(UpgradeAsset upgradeAsset)
    {
        if (upgradeAsset != null)
        {
            switch (upgradeAsset.type)
            {
                case UpgradeAsset.StateUpgrade.Health:
                    healthSystem.curHealthUpgrade = upgradeAsset;
                    healthSystem.maxHealth = (int)upgradeAsset.upgradeStat;
                    upgradeAsset.isPurchased = true;
                    Debug.Log("Health: " + healthSystem.maxHealth);
                    break;
                case UpgradeAsset.StateUpgrade.Stamina:
                    healthSystem.curStaminaUpgrade = upgradeAsset;
                    healthSystem.staminaDrain = upgradeAsset.upgradeStat;
                    upgradeAsset.isPurchased = true;
                    Debug.Log("Stamina: " + healthSystem.staminaDrain);
                    break;
            }
            Debug.Log("Applied upgrade");
        }
        UpdateAllButtons();
    }

    #endregion

    #region ButtonUpdates
    public void UpdateAllButtons()
    {
        UpdateUpgradeButtons(healthUpgrades, healthButtons, healthChecks);
        UpdateUpgradeButtons(staminaUpgrades, staminaButtons, staminaChecks);
    }

    private void UpdateUpgradeButtons(UpgradeAsset[] upgrades, Button[] buttons, GameObject[] checkMarks)
    {
        for (int i = 0; i < upgrades.Length; i++)
        {
            UpgradeAsset upgrade = upgrades[i];
            Button button = buttons[i];
            GameObject checkMark = checkMarks[i];

            if (upgrade.isPurchased)
            {
                button.interactable = false; // Disable button if purchased
                checkMark.SetActive(true); // Show check mark if purchased
            }
            else
            {
                // Check if player can afford the upgrade
                if (scoreManager.appleCount >= upgrade.cost)
                {
                    // Only check prerequisites if they exist
                    if (upgrade.preRequisites == null || upgrade.preRequisites.isPurchased)
                        button.interactable = true; // Enable button if prerequisites are met or none
                    else
                        button.interactable = false; // Disable button if prerequisites are not met
                }
                else
                    button.interactable = false; // Disable button if not enough currency
                checkMark.SetActive(false); // Hide check mark if not purchased
            }
        }
    }
    #endregion
}
