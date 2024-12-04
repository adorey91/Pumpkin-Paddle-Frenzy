using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    [Header("Managers")]
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private ScoreManager scoreManager;
    [SerializeField] private UpgradeManager upgradeManager;
    [SerializeField] private UiManager uiManager;
    [SerializeField] private ToggleManager toggleManager;
    [SerializeField] private InventoryManager inventoryManager;

    private string savePath;
    private string saveFileName = "playerInfo.json";

    public void Start()
    {
        savePath = Path.Combine(GetSavePath(), saveFileName);
    }

    private void OnEnable()
    {
        Actions.LoadBestRun += LoadRunData;
        Actions.LoadSave += Load;
        Actions.DeleteSave += DeleteSave;
    }

    private void OnDisable()
    {
        Actions.LoadBestRun -= LoadRunData;
        Actions.LoadSave -= Load;
        Actions.DeleteSave -= DeleteSave;
    }


    /// <summary>
    /// Resets all upgrades to not purchased. Then checks for save, if no save, will load instructions. If there is a save, the player can choose to use it or delete it.
    /// </summary>
    public void CheckForSave()
    {
        Actions.ResetStats();

        if (File.Exists(savePath))
            uiManager.Confirmation_UI("save");
        else
            uiManager.Instructions_UI();
    }

    public void SaveData()
    {
        if (GameManager.instance.gameIsEndless)
            SaveRunData();
        else
            SavePlayerData();
    }

    #region SaveData
    /// Saves game player stats.
    private void SavePlayerData()
    {
        if (!GameManager.instance.gameIsEndless)
        {
            string filePath = Path.Combine(GetSavePath(), saveFileName);
            PlayerData data = new PlayerData();

            // SavePlayerData the names of all purchased upgrades
            foreach (UpgradeAsset purchasedUpgrade in upgradeManager.PurchasedUpgrades)
            {
                data.purchasedUpgrades.Add(purchasedUpgrade.name); // Add all purchased upgrade names
            }

            // SavePlayerData other stats
            data.availableAppleCount = scoreManager.GetTotalAppleCount();
            data.lifetimeAppleCount = scoreManager.GetLifetimeAppleCount();
            data.attemptsMade = scoreManager.GetAttemptCount();
            data.onScreenControls = toggleManager.GetOnScreenControlsState();
            data.onScreenPause = toggleManager.GetPauseButtonState();
            data.energyAmount = inventoryManager.GetEnergyAmount();

            string json = JsonUtility.ToJson(data, true);
            File.WriteAllText(filePath, json);
        }
    }

    // Saves Best Run Data
    private void SaveRunData()
    {
        string runFilePath = Path.Combine(GetSavePath(), "bestRunInfo.dat");
        RunData runData = new RunData();

        runData.bestTime = scoreManager.GetBestRun();

        string json = JsonUtility.ToJson(runData, true);
        File.WriteAllText(runFilePath, json);
    }
    #endregion

    #region LoadData
    // Loads Best Run Data before menu UI
    public void LoadRunData()
    {
        string runFilePath = Path.Combine(GetSavePath(), "bestRunInfo.dat");
        if (File.Exists(runFilePath))
        {
            try
            {
                string json = File.ReadAllText(runFilePath);
                RunData runData = JsonUtility.FromJson<RunData>(json);

                scoreManager.SetBestRun(runData.bestTime);
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Failed to load save data: " + ex.Message);
                // Optionally delete corrupted save
                File.Delete(runFilePath);
            }
        }
    }

    // Loads Player Stats
    private void Load()
    {
        if (File.Exists(savePath))
        {
            try
            {
                string json = File.ReadAllText(savePath);
                PlayerData data = JsonUtility.FromJson<PlayerData>(json);

                upgradeManager.ClearPurchasedUpgrades();
                foreach (string upgradeName in data.purchasedUpgrades)
                {
                    UpgradeAsset foundUpgrade = upgradeManager.FindUpgradeByName(upgradeName);
                    if (foundUpgrade != null)
                    {
                        foundUpgrade.isPurchased = true;
                        upgradeManager.AddPurchasedUpgrades(foundUpgrade);
                        upgradeManager.ApplyUpgradeToPlayer(foundUpgrade);
                    }
                }

                // Load other stats
                scoreManager.SetTotalAppleCount(data.availableAppleCount, data.lifetimeAppleCount);
                scoreManager.SetAttempt(data.attemptsMade);
                toggleManager.SetOnScreenControlsState(data.onScreenControls);
                toggleManager.SetPauseButtonState(data.onScreenPause);
                inventoryManager.SetEnergyAmount(data.energyAmount);

                Ui_TextUpdater._AppleTotalEndGame = upgradeManager.GetTotalCost();

                Actions.ApplySettings();

                GameManager.instance.loadUpgrade = true;
                Actions.LoadScene("Gameplay");
            }
            catch (System.Exception ex)
            {
                Debug.LogError("Failed to load save data: " + ex.Message);
                // Optionally delete corrupted save
                File.Delete(savePath);
            }
        }
    }
    #endregion

    // Returns the save path
    private static string GetSavePath()
    {
        string path = Application.persistentDataPath;
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
        return path;
    }

    // Deletes SavePlayerData
    internal void DeleteSave()
    {
        if (File.Exists(savePath))
        {
            File.Delete(savePath);
            Debug.Log("File Deleted");
        }

        toggleManager.CheckForKeyboard(); 
        scoreManager.SetAttempt(0);
        scoreManager.SetTotalAppleCount(0, 0);
        inventoryManager.SetEnergyAmount(0);
    }
}