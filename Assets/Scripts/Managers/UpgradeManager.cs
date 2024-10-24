using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradeManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private HealthSystem healthSystem;

    [Header("Health & Stamina Upgrades")]
    public UpgradeUI[] upgradeAssets;

    [Header("Player Components")]
    [SerializeField] private CircleCollider2D playerCollider;
    [SerializeField] private Animator paddleAnimator;
    [SerializeField] SpriteChanger healthSprite;
    [SerializeField] SpriteChanger staminaSprite;

    


    private List<UpgradeAsset> purchasedUpgrades = new List<UpgradeAsset>(); // All purchased upgrades should go to this list
    public IReadOnlyList<UpgradeAsset> PurchasedUpgrades => purchasedUpgrades.AsReadOnly();


    private void OnEnable()
    {
        Actions.ApplySettings += UpdateAllButtons;
        Actions.OnGameOver += UpdateAllButtons;
        Actions.OnGameWin += UpdateAllButtons;
    }

    private void OnDisable()
    {
        Actions.ApplySettings -= UpdateAllButtons;
        Actions.OnGameOver -= UpdateAllButtons;
        Actions.OnGameWin -= UpdateAllButtons;
    }

    public void ClearPurchasedUpgrades() => purchasedUpgrades.Clear();
    public void AddPurchasedUpgrades(UpgradeAsset asset) => purchasedUpgrades.Add(asset);

    public bool CanPurchaseUpgrade(UpgradeAsset upgradeAsset) => !upgradeAsset.isPurchased && scoreManager.GetTotalAppleCount() >= upgradeAsset.cost && (upgradeAsset.preRequisites == null || upgradeAsset.preRequisites.isPurchased);


    
    // Function used to purchase upgrades from upgrade "store"
    public void PurchaseUpgrade(UpgradeAsset upgradeAsset)
    {
        if (CanPurchaseUpgrade(upgradeAsset))
        {
            scoreManager.BuyUpgrade(upgradeAsset.cost);

            // Add to the list of purchased upgrades
            if (!purchasedUpgrades.Contains(upgradeAsset))
                purchasedUpgrades.Add(upgradeAsset);

            ApplyUpgradeToPlayer(upgradeAsset);
        }
        UpdateAllButtons();
    }

    /// <summary>
    /// Resets all upgrade assets to not purchased
    /// </summary>
    public void ResetUpgrades()
    {
        foreach(UpgradeUI upgrade in upgradeAssets)
        {
            upgrade.upgradeAsset.isPurchased = false;
        }

        paddleAnimator.SetInteger("Upgrade", 0);
        Actions.ResetHealth();
    }

    /// <summary>
    /// Applies upgrade upgradeAsset back to player when upgradeAsset isnt purchased
    /// </summary>
    /// <param name="upgradeAsset"></param>
    public void ApplyUpgradeToPlayer(UpgradeAsset upgradeAsset)
    {
        if (upgradeAsset == null) return;

        switch (upgradeAsset.type)
        {
            case UpgradeAsset.StateUpgrade.Health:
                healthSystem.SetUpgrade(upgradeAsset, isHealth: true);
                UpdateSprite(healthSprite, upgradeAsset.newSprite);
                playerCollider.radius = upgradeAsset.colliderRadius; break;
            case UpgradeAsset.StateUpgrade.Stamina:
                healthSystem.SetUpgrade(upgradeAsset, isHealth: false);
                UpdateSprite(staminaSprite, upgradeAsset.newSprite);
                paddleAnimator.SetInteger("Upgrade", upgradeAsset.number);
                break;
        }
        //Debug.Log("Applied upgrade");
        upgradeAsset.isPurchased = true;
    }

    private void UpdateSprite(SpriteChanger spriteToChange, Sprite newSprite)
    {
        spriteToChange.playerSprite.sprite = newSprite;
        spriteToChange.upgradePageImage.sprite = newSprite;
    }

    /// <summary>
    /// Used to find an upgrade in the all upgrade list by name
    /// </summary>
    /// <param name="upgradeName"></param>
    /// <returns></returns>
    public UpgradeAsset FindUpgradeByName(string upgradeName)
    {
        foreach(UpgradeUI upgrade in upgradeAssets)
        {
            if(upgrade.upgradeAsset.name ==  upgradeName) return upgrade.upgradeAsset;
        }

        Debug.LogWarning($"Upgrade not found: {upgradeName}");
        return null;
    }


    /// <summary>
    /// Updates all upgrade buttons based on a private function, that takes the upgrades array, their buttons and the checkmarks
    /// </summary>
    public void UpdateAllButtons()
    {
        foreach(UpgradeUI upgrade in upgradeAssets)
        {
            UpdateUpgradeButtons(upgrade);
        }
    }

    private void UpdateUpgradeButtons(UpgradeUI upgrade)
    {
        UpgradeAsset upgradeAsset = upgrade.upgradeAsset;

        upgrade.button.interactable = CanPurchaseUpgrade(upgradeAsset);
        upgrade.checkMark.SetActive(upgradeAsset.isPurchased);

        if(GameManager.instance.gameIsEndless)
            upgradeAsset.cost = upgradeAsset.baseCost * 2;
        else
            upgradeAsset.cost = upgradeAsset.baseCost;

        upgrade.costText.text = upgradeAsset.cost.ToString();  
    }

    private void UpdateUpgradeCost()
    {
        foreach(UpgradeUI upgrade in upgradeAssets)
        {
            upgrade.costText.text = upgrade.upgradeAsset.cost.ToString();
        }
    }
}
