using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private Button saveButton;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private LevelManager levelManager;


    public void CheckForSave()
    {
        if (File.Exists(GetSavePath() + "/playerInfo.dat"))
            uiManager.Confirmation_UI("save");
        else
            uiManager.Instructions_UI();
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(GetSavePath() + "/playerInfo.dat");

        PlayerData data = new PlayerData();

        // Save the names of all purchased upgrades
        foreach (UpgradeAsset purchasedUpgrade in upgradeManager.purchasedUpgrades)
        {
            data.purchasedUpgrades.Add(purchasedUpgrade.name); // Add all purchased upgrade names
        }

        // Save other stats
        data.healthAmount = healthSystem.maxHealth;
        data.staminaDrain = healthSystem.staminaDrain;
        data.appleCount = scoreManager.appleCount;
        data.attemptsMade = scoreManager.attemptNumber;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game Saved");

        saveButton.interactable = false;
    }



    public void Load()
    {
        if (File.Exists(GetSavePath() + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(GetSavePath() + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            // Find and apply all upgrades by name
            foreach (string upgradeName in data.purchasedUpgrades)
            {
                UpgradeAsset foundUpgrade = FindUpgradeByName(upgradeName);
                if (foundUpgrade != null)
                {
                    upgradeManager.ApplyUpgradeToPlayer(foundUpgrade);
                    // Add it back to the purchased upgrades list
                    upgradeManager.purchasedUpgrades.Add(foundUpgrade); // Rebuild purchased list after loading
                }
            }

            // Load other stats
            healthSystem.maxHealth = data.healthAmount;
            healthSystem.staminaDrain = data.staminaDrain;
            scoreManager.appleCount = data.appleCount;
            scoreManager.attemptNumber = data.attemptsMade;

            healthSystem.UpdateHealthStats();
            upgradeManager.UpdateAllButtons();
            levelManager.LoadScene("Gameplay");
        }
    }


    private static string GetSavePath()
    {
        return Application.persistentDataPath;
    }


    private UpgradeAsset FindUpgradeByName(string upgradeName)
    {
        // Search for the newUpgrade by name from the UpgradeManager's list
        foreach (UpgradeAsset upgrade in upgradeManager.allUpgrades)
        {
            if (upgrade.name == upgradeName)
                return upgrade;
        }
        Debug.LogWarning($"Upgrade not found: {upgradeName}");
        return null;
    }
}