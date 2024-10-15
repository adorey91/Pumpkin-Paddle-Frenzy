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
        data.appleCount = ScoreManager.totalAppleCount;
        data.attemptsMade = ScoreManager.attemptNumber;
        data.onScreenControls = uiManager.activeControls;
        data.onScreenPause = uiManager.activePause;

        bf.Serialize(file, data);
        file.Close();
    }

    /// <summary>
    /// Loads gamestats
    /// </summary>
    internal void Load()
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
                    foundUpgrade.isPurchased = true;
                    upgradeManager.purchasedUpgrades.Add(foundUpgrade);

                    upgradeManager.ApplyUpgradeToPlayer(foundUpgrade);
                }
            }

            // Load Stats
            healthSystem.maxHealth = data.healthAmount;
            healthSystem.staminaDrain = data.staminaDrain;
            ScoreManager.totalAppleCount = data.appleCount;
            ScoreManager.attemptNumber = data.attemptsMade;
            uiManager.activePause = data.onScreenPause;
            uiManager.activeControls = data.onScreenControls;

            // Triggers the load settings action
            Actions.LoadSettings();

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