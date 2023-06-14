using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using Photon.Pun;
public class GameStatsNetwork
{
    public string accurecy { get; set; }
    public string waveReached { get; set; }
    public string wordsTyped { get; set; }
    public string charactersTyped { get; set; }
    public string charactersCorrect { get; set; }
}



public class StatsManagerNetwork : MonoBehaviour
{

    [SerializeField] GUIMeshText timer;
    [SerializeField] GUIMeshText accurecyUI;
    [SerializeField] GUIMeshText wordsTypedUI;

/*    [SerializeField] TMP_Text accurecyText;*/
    [SerializeField]
    private PhotonView photonView;


    [SerializeField] public int charactersTyped;
    [SerializeField] public int charactersCorrect;
    [SerializeField] public float typingSpeed;
    [SerializeField] public float accurecy;
    [SerializeField] public int waveReached;
    [SerializeField] public int wordsTyped;

    [SerializeField] int time = 0;
    private Coroutine timerCoroutine;



    void Start()
    {
      
        timerCoroutine = StartCoroutine(startTimer());
    }



    public GameStats GetStats()
    {
        return new GameStats
        {

            accurecy = charactersTyped > 0 ? Math.Round((float)charactersCorrect / charactersTyped * 100) + "%" : "0%",
            charactersCorrect = charactersCorrect + "",
            charactersTyped = charactersTyped + "",
            waveReached = waveReached + "",
            wordsTyped = wordsTyped + ""
        };
    }

    private IEnumerator startTimer()
    {
        while (true)
        {
            time++;
            yield return new WaitForSeconds(1);
        }
    }

    public void IncreaseCharactersTyped()
    {
        charactersTyped++;
    }
    public void IncreaseCorrectCharactersTyped()
    {
        charactersCorrect++;
    }

    public void IncreaseWordsTyped() => wordsTyped++;


    public void SetWaveReached(int wave)
    {
        waveReached = wave;
        StopCoroutine(timerCoroutine);
    }
}
