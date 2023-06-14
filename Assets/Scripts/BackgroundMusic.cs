using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{

    public AudioSource musicPlayer; // the bgm audio source and/or whatever else you need
    public static BackgroundMusic Instance { get; private set; }

    void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject); // there can only be one
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

}
