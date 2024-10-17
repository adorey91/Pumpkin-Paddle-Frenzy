using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeChange : MonoBehaviour
{
    [SerializeField] private GameObject staminaObjects;


    private void OnEnable()
    {
        Actions.OnGameplay += ChangeObjects;
    }

    private void OnDisable()
    {
        Actions.OnGameplay -= ChangeObjects;
    }


    private void ChangeObjects()
    {
        if (GameManager.instance.isEndless)
            staminaObjects.SetActive(false);
        else
            staminaObjects.SetActive(true);
    }
}
