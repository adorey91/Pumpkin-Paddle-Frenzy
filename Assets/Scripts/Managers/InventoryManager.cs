using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{

    [Header("Ui References")]
    [SerializeField] private TMP_Text energyTextAmnt;
    [SerializeField] private Button energyButton;

    [Header("Energy Settings")]
    [SerializeField] private int initialEnergyAmount = 0;
    [SerializeField] private float energyDuration = 10f;

    [Header("Timer Reference")]
    [SerializeField] private CustomTimerSO energyTimer;

    private int energyAmount;
    private bool canUseEnergy;
    private bool usingEnergy = false;


    private void Start()
    {
        energyAmount = initialEnergyAmount;
    }

    private void OnEnable()
    {
        Actions.EnergyCollection += CollectEnergy;
        Actions.OnUseEnergy += UseEnergy;
        Actions.OnGameplay += Reset;
    }

    private void OnDisable()
    {
        Actions.EnergyCollection -= CollectEnergy;
        Actions.OnUseEnergy -= UseEnergy;
        Actions.OnGameplay -= Reset;
    }

    private void Update()
    {
        if (usingEnergy)
        {
            Time.timeScale = 2;

            energyButton.image.fillAmount = energyTimer.GetRemainingTime() / energyTimer.duration;

            if (energyTimer.UpdateTimer(Time.deltaTime))
            {
                usingEnergy = false;
                Time.timeScale = 1;
                energyButton.image.fillAmount = 1;

                if (energyAmount > 0)
                {
                    canUseEnergy = true;
                    energyButton.interactable = true;
                }
            }
        }

    }

    public int GetEnergyAmount()
    {
        return energyAmount;
    }

    public void SetEnergyAmount(int amount)
    {
        energyAmount = amount;
    }

    private void CollectEnergy()
    {
        energyAmount++;
        energyTextAmnt.text = $"x {energyAmount}";

        if (energyAmount > 0 && !usingEnergy)
        {
            energyButton.interactable = true;
            canUseEnergy = true;
        }
    }

    public void UseEnergy()
    {
        if (usingEnergy || energyAmount == 0)
        {
            canUseEnergy = false;
            energyButton.interactable = false;
            return;
        }

        if (energyAmount > 0 && GameManager.instance.isPlaying)
        {

            usingEnergy = true;
            canUseEnergy = false;
            energyButton.interactable = false;

            energyAmount--;
            energyTextAmnt.text = $"x {energyAmount}";

            energyTimer.StartTimer(energyDuration);
            Debug.Log($"Timer Started: {energyTimer.isRunning}, Duration: {energyTimer.duration}");

            if (energyAmount == 0)
                energyButton.interactable = false;
        }

    }

    private void UpdateEnergyUI()
    {
        energyTextAmnt.text = $"x {energyAmount}";
    }

    private void UpdateEnergyTimerUI()
    {
        Time.timeScale = 2;
        energyButton.image.fillAmount = energyTimer.GetRemainingTime() / energyTimer.duration;
    }


    private void Reset()
    {
        energyTextAmnt.text = $"x {energyAmount}";

        if (energyAmount > 0)
        {
            energyButton.interactable = true;
            canUseEnergy = true;
        }
        else
            energyButton.interactable = false;

        energyTimer.ResetTimer();
        canUseEnergy = false;
        usingEnergy = false;
    }
}
