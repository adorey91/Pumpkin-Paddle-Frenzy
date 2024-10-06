using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextUpdater : MonoBehaviour
{
    [SerializeField] private UpgradeAsset upgradeAsset;


    private void Start()
    {
        TMP_Text costText = GetComponent<TextMeshProUGUI>();
        costText.text = upgradeAsset.cost.ToString();
    }
}
