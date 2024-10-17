using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;

public class SoundManager : MonoBehaviour
{
    [Header("Audio Source / Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Audio Control / Mixer Groups")]
    [SerializeField] private AudioControl sfxControl;
    [SerializeField] private AudioControl musicControl;
    [SerializeField] private AudioControl masterControl;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip mainClip;
    [SerializeField] private AudioClip gameplayClip;
    [SerializeField] private AudioClip sfxCollectClip;
    [SerializeField] private AudioClip sfxCrashClip;
    [SerializeField] private AudioClip sfxVictoryClip;


    public void Start()
    {
        musicSource.loop = true;

        SetVolume(sfxControl.mixer, sfxControl.audioImage.fillAmount);
        SetVolume(musicControl.mixer, musicControl.audioImage.fillAmount);
        SetVolume(masterControl.mixer, masterControl.audioImage.fillAmount);
    }

    private void OnEnable()
    {
        Actions.OnGameplay += PlayGameplay;
        Actions.OnGameOver += PlayEnemyCrash;
        Actions.OnPlayerHurt += PlayEnemyCrash;
        Actions.OnCollectApple += PlayAppleCollection;
        Actions.OnGameWin += PlayVictory;
    }


    private void OnDisable()
    {
        Actions.OnGameplay -= PlayGameplay;
        Actions.OnGameOver -= PlayEnemyCrash;
        Actions.OnPlayerHurt -= PlayEnemyCrash;
        Actions.OnCollectApple -= PlayAppleCollection;
        Actions.OnGameWin -= PlayVictory;
    }

    public void PlayMenu()
    {
        if (!musicSource.isPlaying || musicSource.clip != mainClip)
        {
            musicSource.clip = mainClip;
            musicSource.Play();
        }
    }


    private void PlayGameplay()
    {
        if (!musicSource.isPlaying || musicSource.clip != gameplayClip)
        {
            musicSource.clip = gameplayClip;
            musicSource.Play();
        }
    }

    private void PlayVictory()
    {
        if (sfxSource == null)
        {
            GameObject foundObject = GameObject.Find("SFXSource");
            sfxSource = GetComponent<AudioSource>();
        }
        sfxSource.PlayOneShot(sfxVictoryClip, 1f);
    }

    private void PlayEnemyCrash()
    {
        if (sfxSource == null)
        {
            GameObject foundObject = GameObject.Find("SFXSource");
            sfxSource = GetComponent<AudioSource>();
        }
        sfxSource.PlayOneShot(sfxCrashClip, 1f);
    }

    private void PlayAppleCollection()
    {
        if (sfxSource == null)
        {
            GameObject foundObject = GameObject.Find("SFXSource");
            sfxSource = GetComponent<AudioSource>();
        }
        sfxSource.PlayOneShot(sfxCollectClip, 1f);
    }



    // Sets volume

    public void IncreaseAudio(AudioMixerGroup mixerGroup)
    {
        AdjustAudio(mixerGroup, 0.1f);
    }

    public void DecreaseAudio(AudioMixerGroup mixerGroup)
    {
        AdjustAudio(mixerGroup, -0.1f);
    }

    private void AdjustAudio(AudioMixerGroup mixerGroup, float changeAmount)
    {
        AudioControl control = GetAudioControl(mixerGroup.name);
        if (control.audioImage != null)
        {
            SetAudio(control, changeAmount);
        }
    }

    private AudioControl GetAudioControl(string mixerName)
    {
        switch (mixerName)
        {
            case "SFX":
                return sfxControl;
            case "Music":
                return musicControl;
            case "Master":
                return masterControl;
            default:
                Debug.LogError($"AudioControl for '{mixerName}' not recognized.");
                return new AudioControl();
        }
    }

    private void SetAudio(AudioControl control, float changeAmount)
    {
        control.audioImage.fillAmount = Mathf.Clamp(control.audioImage.fillAmount + changeAmount, 0f, 1f);
        SetVolume(control.mixer, control.audioImage.fillAmount);

        if (control.audioImage.fillAmount <= 1f)
            Debug.Log("Its at 1");

        control.increaseButton.interactable = control.audioImage.fillAmount <= 1f;
        control.decreaseButton.interactable = control.audioImage.fillAmount >= 0.0001f;
    }

    public void SetVolume(AudioMixerGroup mixer, float fillAmount)
    {
        string mixerName = mixer.name;

        float minFillAmount = 0.0001f;
        float volumeValue = Mathf.Clamp(Mathf.Log10(Mathf.Max(fillAmount, minFillAmount)) * 20, -80f, 0f);
        audioMixer.SetFloat(mixerName, volumeValue);
    }


    public void StopMusic()
    {
        musicSource.Stop();
    }
}
