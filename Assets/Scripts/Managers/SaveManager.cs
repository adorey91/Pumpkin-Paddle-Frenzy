using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private LevelManager levelManager;


    /// <summary>
    /// Resets all upgrades to not purchased. Then checks for save, if no save, will load instructions. If there is a save, the player can choose to use it or delete it.
    /// </summary>
    public void CheckForSave()
    {
        upgradeManager.ResetUpgrades();

        if (File.Exists(GetSavePath() + "/playerInfo.dat"))
            uiManager.Confirmation_UI("save");
        else
            uiManager.Instructions_UI();
    }

    /// <summary>
    /// Saves game stats.
    /// </summary>
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
        data.onScreenControls = uiManager.activeControls;
        data.onScreenPause = uiManager.activePause;

        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game Saved");

    }

    /// <summary>
    /// Loads gamestats
    /// </summary>
    public void Load()
    {
        if (File.Exists(GetSavePath() + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(GetSavePath() + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            // Clear the list of purchased upgrades before reloading them from saved data
            upgradeManager.purchasedUpgrades.Clear();

            // Find and apply all upgrades by name
            foreach (string upgradeName in data.purchasedUpgrades)
            {
                UpgradeAsset foundUpgrade = upgradeManager.FindUpgradeByName(upgradeName);
                if (foundUpgrade != null)
                {
                    foundUpgrade.isPurchased = true; // Ensure it's marked as purchased
                    upgradeManager.purchasedUpgrades.Add(foundUpgrade); // Add to purchased list

                    // Apply the upgrade (to update any relevant player stats)
                    upgradeManager.ApplyUpgradeToPlayer(foundUpgrade);
                }
            }

            // Load other stats
            healthSystem.maxHealth = data.healthAmount;
            healthSystem.staminaDrain = data.staminaDrain;
            scoreManager.appleCount = data.appleCount;
            scoreManager.attemptNumber = data.attemptsMade;
            uiManager.activePause = data.onScreenPause;
            uiManager.activeControls = data.onScreenControls;

            // Make sure to update health system and buttons after loading all data
            healthSystem.UpdateHealthStats();
            upgradeManager.UpdateAllButtons(); // Update the buttons once the data is fully loaded
            uiManager.LoadButtons();

            // Load the gameplay scene
            levelManager.LoadScene("Gameplay");
        }
    }


    /// <summary>
    /// Returns the save path
    /// </summary>
    /// <returns></returns>
    private static string GetSavePath()
    {
        return Application.persistentDataPath;
    }

    /// <summary>
    /// Deletes Save
    /// </summary>
    internal void DeleteSave()
    {
        if (File.Exists(GetSavePath() + "/playerInfo.dat"))
        {
            File.Delete(GetSavePath() + "/playerInfo.dat");
            Debug.Log("File Deleted");
        }
    }
}