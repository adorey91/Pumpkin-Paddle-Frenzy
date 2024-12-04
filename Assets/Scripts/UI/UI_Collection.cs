using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Collection : MonoBehaviour
{
    [SerializeField] private TMP_Text collectionText;
    [SerializeField] private TMP_Text energyCollection;

    public void UpdateCollectionText(string collectionAmount)
    {
        collectionText.text = collectionAmount;
    }

    public void UpdateEnergyText(string collectionAmount)
    {
        energyCollection.text = collectionAmount;
    }
}
