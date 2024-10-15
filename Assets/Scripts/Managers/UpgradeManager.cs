using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private HealthSystem healthSystem;

    [Header("Upgrades")]
    public UpgradeAsset[] healthUpgrades;
    public UpgradeAsset[] staminaUpgrades;

    [Header("Upgrade Buttons")]
    [SerializeField] private Button[] healthButtons;
    [SerializeField] private Button[] staminaButtons;

    [Header("Checkmark Images")]
    [SerializeField] private GameObject[] healthChecks;
    [SerializeField] private GameObject[] staminaChecks;

    [Header("Player Collider")]
    [SerializeField] private CircleCollider2D playerCollider;

    [Header("Health Upgrade Components")]
    [SerializeField] private SpriteRenderer healthSprite;
    [SerializeField] private Image healthSpriteUpgrade;

    [Header("Stamina Upgrade Components")]
    [SerializeField] private SpriteRenderer staminaSprite;
    [SerializeField] private Image staminaSpriteUpgrade;
    [SerializeField] private Animator paddleAnimator;



    internal List<UpgradeAsset> purchasedUpgrades = new List<UpgradeAsset>(); // All purchased upgrades should go to this list

    private void OnEnable()
    {
        Actions.LoadSettings += UpdateAllButtons;
    }

    private void OnDisable()
    {
        Actions.LoadSettings -= UpdateAllButtons;
    }

    /// <summary>
    /// Function used to purchase upgrades from upgrade "store"
    /// </summary>
    /// <param name="upgradeAsset"></param>
    public void PurchaseUpgrade(UpgradeAsset upgradeAsset)
    {
        if (ScoreManager.totalAppleCount >= upgradeAsset.cost && !upgradeAsset.isPurchased)
        {
            ScoreManager.totalAppleCount -= upgradeAsset.cost;
            Actions.UpdateAppleText();

            // Add to the list of purchased upgrades if not already in it
            if (!purchasedUpgrades.Contains(upgradeAsset))
                purchasedUpgrades.Add(upgradeAsset);

            ApplyUpgradeToPlayer(upgradeAsset);
        }
    }

    /// <summary>
    /// Resets all upgrade assets
    /// </summary>
    public void ResetUpgrades()
    {
        foreach (var upgradeAsset in healthUpgrades)
        {
            upgradeAsset.isPurchased = false;
        }

        foreach (var upgradeAsset in staminaUpgrades)
        {
            upgradeAsset.isPurchased = false;
        }
    }

    /// <summary>
    /// Applies upgrade asset back to player
    /// </summary>
    /// <param name="upgradeAsset"></param>
    public void ApplyUpgradeToPlayer(UpgradeAsset upgradeAsset)
    {
        if (upgradeAsset != null)
        {
            switch (upgradeAsset.type)
            {
                case UpgradeAsset.StateUpgrade.Health:
                    healthSystem.curHealthUpgrade = upgradeAsset;
                    healthSystem.maxHealth = (int)upgradeAsset.newStats;
                    healthSprite.sprite = upgradeAsset.newSprite;
                    healthSpriteUpgrade.sprite = upgradeAsset.newSprite;
                    playerCollider.radius = upgradeAsset.colliderRadius;
                    upgradeAsset.isPurchased = true;
                    break;

                case UpgradeAsset.StateUpgrade.Stamina:
                    healthSystem.curStaminaUpgrade = upgradeAsset;
                    healthSystem.staminaDrain = upgradeAsset.newStats;
                    staminaSprite.sprite = upgradeAsset.newSprite;
                    staminaSpriteUpgrade.sprite = upgradeAsset.newSprite;
                    paddleAnimator.SetInteger("Upgrade", upgradeAsset.number);
                    upgradeAsset.isPurchased = true;
                    break;
            }
            Debug.Log("Applied upgrade");
        }
        UpdateAllButtons();
    }

    /// <summary>
    /// Used to find an upgrade in the all upgrade list by name
    /// </summary>
    /// <param name="upgradeName"></param>
    /// <returns></returns>
    public UpgradeAsset FindUpgradeByName(string upgradeName)
    {
        foreach (UpgradeAsset upgradeAsset in healthUpgrades)
        {
            if (upgradeAsset.name == upgradeName)
                return upgradeAsset;
        }

        foreach(UpgradeAsset upgradeAsset in staminaUpgrades)
        {
            if (upgradeAsset.name == upgradeName)
                    return upgradeAsset;
        }
        Debug.LogWarning($"Upgrade not found: {upgradeName}");
        return null;
    }

    /// <summary>
    /// Updates all upgrade buttons based on a private function, that takes the upgrades array, their buttons and the checkmarks
    /// </summary>
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
                Debug.Log(button.name + "is disabled");
                checkMark.SetActive(true); // Show check mark if purchased
            }
            else
            {
                // Check if player can afford the upgrade
                if (ScoreManager.totalAppleCount >= upgrade.cost)
                {
                    // Only check prerequisites if they exist
                    if (upgrade.preRequisites == null || upgrade.preRequisites.isPurchased)
                        button.interactable = true; // Enable button if prerequisites are met or none
                    else
                        button.interactable = false; // Disable button if prerequisites are not met
                }
                else
                {
                    button.interactable = false; // Disable button if not enough currency
                }
                checkMark.SetActive(false); // Hide check mark if not purchased
            }
        }
    }
}
