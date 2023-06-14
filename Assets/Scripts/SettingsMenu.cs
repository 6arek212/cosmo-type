using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    Slider volumeSlider;
    private const float initalValume = 0.004690917f;



    private AudioSource audioSource;

    private void Start()
    {
        audioSource = BackgroundMusic.Instance.musicPlayer;
        InitMusicVolume();
    }

    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume;

        Save(volume);
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("musicVolume");
    }

    private void Save(float volume)
    {
        PlayerPrefs.SetFloat("musicVolume", volume);
    }

    private void InitMusicVolume()
    {
        if (!PlayerPrefs.HasKey("musicVolume"))
        {
            ChangeVolume(initalValume);
            volumeSlider.value = initalValume;
            /*  Load();*/
        }
        else
        {
            Load();
        }
    }
}
