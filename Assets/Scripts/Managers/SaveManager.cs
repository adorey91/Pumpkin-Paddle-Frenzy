using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    [SerializeField] private Button saveButton;

   

    public void CheckForSave()
    {
        if(File.Exists(GetSavePath() + "/playerInfo.dat"))
        {
            // should ask if player wants to play this save
        }
        else
        {
            // should continue to instructions
        }
    }

    public void Save()
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(GetSavePath() + "/playerInfo.dat");
        Debug.Log(Application.persistentDataPath);

        PlayerData data = new PlayerData();
        //if(CharacterController.instance.currentHealthUpgrade != null)
        //    data.currentHealthUpgrade = CharacterController.instance.currentHealthUpgrade;
        //if(CharacterController.instance.currentStaminaUpgrade != null)
        //    data.currentStaminaUpgrade = CharacterController.instance.currentStaminaUpgrade;
        data.healthAmount = CharacterController.instance.healthAmount;
        data.staminaDrain = CharacterController.instance.staminaDrain;
        data.appleCount = CharacterController.instance.appleCount;

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
            FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file);
            file.Close();
            //if(data.currentHealthUpgrade != null)
            //    CharacterController.instance.currentHealthUpgrade = data.currentHealthUpgrade;
            //if (data.currentStaminaUpgrade != null)
            //    CharacterController.instance.currentStaminaUpgrade = data.currentStaminaUpgrade;
            CharacterController.instance.healthAmount = data.healthAmount;
            CharacterController.instance.staminaDrain = data.staminaDrain;
            CharacterController.instance.appleCount = data.appleCount;
        }
    }

    private static string GetSavePath()
    {
        return Application.persistentDataPath;
    }
}
