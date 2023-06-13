using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHover : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hoverSoundEffect;



    public void OnHover()
    {
        audioSource.PlayOneShot(hoverSoundEffect);
    }



}
