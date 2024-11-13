using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Save Data", menuName = "Save/NewData")]
public class SaveSO : ScriptableObject
{
    public bool onScreenControls;
    public bool onScreenPause;
    public int attemptsMade;
    public int totalAppleCount;

    public List<UpgradeAsset> purchasedUpgrades;

    public bool IsThereData()
    {
        //Debug.Log(attemptsMade);
        return attemptsMade > 0;
    }


    // Delete the data only needs to reset has saved data, attempts made & purchased upgrades as the rest will just be written over with data
    public void DeleteData()
    {
        attemptsMade = 0;
        purchasedUpgrades.Clear();
    }
}
