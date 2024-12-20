using System;
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
    [SerializeField] private float energySpeedMultiplier = 1.5f;

    [Header("Timer Reference")]
    [SerializeField] private CustomTimerSO energyTimer;

    private int energyAmount;
    private bool canUseEnergy;
    private bool isUsingEnergy = false;


    private void Awake()
    {
        energyAmount = initialEnergyAmount;
        UpdateEnergyUI();
    }

    private void OnEnable()
    {
        Actions.EnergyCollection += OnEnergyCollected;
        Actions.OnUseEnergy += OnEnergyUsed;
        Actions.OnGameplay += OnGameplayReset;
    }

    private void OnDisable()
    {
        Actions.EnergyCollection -= OnEnergyCollected;
        Actions.OnUseEnergy -= OnEnergyUsed;
        Actions.OnGameplay -= OnGameplayReset;
    }

    private void Update()
    {
        if (isUsingEnergy)
        {
            UpdateEnergyTimerUI();

            // if the timer completes
            if (energyTimer.UpdateTimer(Time.deltaTime))
            {
                StopUsingEnergy();
            }
        }
    }

    #region GettersSetters
    public int GetEnergyAmount()
    {
        return energyAmount;
    }

    public void SetEnergyAmount(int amount)
    {
        energyAmount = amount;
    }
    #endregion

    // Event Handlers
    private void OnEnergyCollected()
    {
        energyAmount++;
        Actions.OnPlaySFX("EnergyCollection");
        UpdateEnergyUI();

        if (energyAmount > 0 && !isUsingEnergy)
        {
            SetEnergyButtonState(true);
        }
    }

    // Sets state of the energy button
    private void SetEnergyButtonState(bool state)
    {
        canUseEnergy = state;
        energyButton.interactable = state;
    }

    // Event Handler for using energy
    public void OnEnergyUsed()
    {
        if (isUsingEnergy || energyAmount <= 0)
        {
            SetEnergyButtonState(false);
            return;
        }

        if (energyAmount > 0 && GameManager.instance.isPlaying)
        {
            StartUsingEnergy();
        }

    }

    // Start using energy
    private void StartUsingEnergy()
    {
        isUsingEnergy = true;
        energyAmount--;
        Actions.OnPlaySFX("EnergyUsed");

        UpdateEnergyUI();
        SetEnergyButtonState(false);

        energyTimer.StartTimer(energyDuration);
        //Debug.Log($"Timer Started: {energyTimer.isRunning}, Duration: {energyTimer.duration}");
    }

    // Stop using energy, sets everything back to normal
    private void StopUsingEnergy()
    {
        isUsingEnergy = false;
        Time.timeScale = 1;

        if (energyAmount > 0)
        {
            SetEnergyButtonState(true);
        }

        ResetTimerUI();
    }

    // UI Updates
    private void UpdateEnergyUI()
    {
        energyTextAmnt.text = $"x {energyAmount}";
    }

    private void UpdateEnergyTimerUI()
    {
        Time.timeScale = energySpeedMultiplier;
        energyButton.image.fillAmount = energyTimer.GetRemainingTime() / energyTimer.duration;
    }

    private void ResetTimerUI()
    {
        energyButton.image.fillAmount = 1;
    }
     
    private void OnGameplayReset()
    {
        energyTextAmnt.text = $"x {energyAmount}";

        if (energyAmount > 0)
        {
            energyButton.interactable = true;
            canUseEnergy = true;
        }
        else
            energyButton.interactable = false;

        energyButton.image.fillAmount = 1;
        energyTimer.ResetTimer();
        canUseEnergy = false;
        isUsingEnergy = false;
    }
}
