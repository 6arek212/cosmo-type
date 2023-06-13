using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    Slider volumeSlider;
    private const float fullVolume = 1;
    [SerializeField]
    private AudioSource audioSource;

    private void Start()
    {
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
            Save(fullVolume);
            /*  Load();*/
        }
        else
        {
            Load();
        }
    }
}
