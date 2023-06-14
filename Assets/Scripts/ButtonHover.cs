using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// handling the button hover
/// </summary>
public class ButtonHover : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip hoverSoundEffect;



    public void OnHover()
    {
        audioSource.PlayOneShot(hoverSoundEffect);
    }



}
