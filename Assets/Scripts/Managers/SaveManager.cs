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
    [SerializeField] private OnScreenButtonControl onScreenButtonControl;

    [SerializeField] private SaveSO saveObject;

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

        //if (File.Exists(GetSavePath() + "/playerInfo.dat"))
        //    uiManager.Confirmation_UI("save");
        if(saveObject.IsThereData())
        {
            uiManager.Confirmation_UI("save");
        }
        else
        {
            uiManager.Instructions_UI();
        }
    }

    public void SaveData()
    {
        if(GameManager.instance.gameIsEndless)
            SaveRunData();
        else
            SavePlayerData();
    }

    #region SaveData
    /// Saves game player stats.
    private void SavePlayerData()
    {
        if(!GameManager.instance.gameIsEndless)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(GetSavePath() + "/playerInfo.dat");
            PlayerData data = new PlayerData();

            //// SavePlayerData the names of all purchased upgrades
            //foreach (UpgradeAsset purchasedUpgrade in upgradeManager.PurchasedUpgrades)
            //{
            //    data.purchasedUpgrades.Add(purchasedUpgrade.name); // Add all purchased upgrade names
            //}

            //// SavePlayerData other stats
            //data.appleCount = scoreManager.GetTotalAppleCount();
            //data.attemptsMade = scoreManager.GetAttemptCount();
            //data.onScreenControls = onScreenButtonControl.activeControls;
            //data.onScreenPause = onScreenButtonControl.activePause;

            saveObject.totalAppleCount = scoreManager.GetTotalAppleCount();
            saveObject.attemptsMade = scoreManager.GetAttemptCount();
            saveObject.onScreenControls = onScreenButtonControl.activeControls;
            saveObject.onScreenPause = onScreenButtonControl.activePause;

            foreach(UpgradeAsset purchased in upgradeManager.PurchasedUpgrades)
            {
                if(!saveObject.purchasedUpgrades.Contains(purchased))
                    saveObject.purchasedUpgrades.Add(purchased);
            }

            bf.Serialize(file, data);
            file.Close();
        }
    }

    // Saves Best Run Data
    private void SaveRunData()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(GetSavePath() + "/bestRunInfo.dat");
        RunData runData = new RunData();

        runData.bestTime = scoreManager.GetBestRun();

        bf.Serialize(file, runData);
        file.Close();
    }
    #endregion

    #region LoadData
    // Loads Best Run Data before menu UI
    public void LoadRunData()
    {
        if (File.Exists(GetSavePath() + "/bestRunInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(GetSavePath() + "/bestRunInfo.dat", FileMode.Open);
            RunData runData = (RunData)bf.Deserialize(file);
            file.Close();

            //Debug.Log("Set best run " +  runData.bestTime);
            scoreManager.SetBestRun(runData.bestTime);
        }
    }

    // Loads Player Stats
    internal void Load()
    {
        if (File.Exists(GetSavePath() + "/playerInfo.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(GetSavePath() + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();

            // Clear the list of purchased upgrades before reloading them from saved data
            upgradeManager.ClearPurchasedUpgrades();


            foreach(UpgradeAsset purchased in saveObject.purchasedUpgrades)
            {
                upgradeManager.AddPurchasedUpgrades(purchased);
                upgradeManager.ApplyUpgradeToPlayer(purchased);
            }

            scoreManager.SetTotalAppleCount(saveObject.totalAppleCount);
            scoreManager.SetAttempt(saveObject.attemptsMade);
            onScreenButtonControl.activeControls = saveObject.onScreenControls;
            onScreenButtonControl.activePause = saveObject.onScreenPause;

            //// Find and apply all upgrades by name
            //foreach (string upgradeName in data.purchasedUpgrades)
            //{
            //    UpgradeAsset foundUpgrade = upgradeManager.FindUpgradeByName(upgradeName);
            //    if (foundUpgrade != null)
            //    {
            //        foundUpgrade.isPurchased = true;
            //        upgradeManager.AddPurchasedUpgrades(foundUpgrade);
            //        upgradeManager.ApplyUpgradeToPlayer(foundUpgrade);
            //    }
            //}

            //// Load Stats
            //scoreManager.SetTotalAppleCount(data.appleCount);
            //scoreManager.SetAttempt(data.attemptsMade);
            //onScreenButtonControl.activePause = data.onScreenPause;
            //onScreenButtonControl.activeControls = data.onScreenControls;

            // Triggers the load settings action
            Actions.ApplySettings();

            // Load the gameplay scene after applying player data
            Actions.LoadScene("Gameplay");
        }
    }
    #endregion

    // Returns the save path
    private static string GetSavePath()
    {
        return Application.persistentDataPath;
    }

    // Deletes SavePlayerData
    internal void DeleteSave()
    {
        //if (File.Exists(GetSavePath() + "/playerInfo.dat"))
        //{
        //    File.Delete(GetSavePath() + "/playerInfo.dat");
        //    Debug.Log("File Deleted");
        //}

        if (saveObject.IsThereData())
        {
            saveObject.DeleteData();
            Debug.Log("File Deleted");
        }
    }
}