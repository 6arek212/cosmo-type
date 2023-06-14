using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameStats
{
    public string accurecy { get; set; }
    public string waveReached { get; set; }
    public string wordsTyped { get; set; }
    public string charactersTyped { get; set; }
    public string charactersCorrect { get; set; }
}



/// <summary>
/// game timer class
/// </summary>
public class GameTimer
{
    private int time = 0;

    public void IncreseScond() => time++;
    public string GetTime()
    {
        int min = time / 60;
        int seconds = time % 60;
        string returnString = "";

        if (min < 10)
            returnString += "0";
        returnString += min.ToString() + ":";

        if (seconds < 10)
            returnString += "0";
        returnString += seconds.ToString();


        return returnString;
    }

}


/// <summary>
/// this class keep track of the game stats
/// </summary>
public class StatsManager : MonoBehaviour
{
    [SerializeField] GUIMeshText timer;
    [SerializeField] GUIMeshText accurecyUI;
    [SerializeField] GUIMeshText wordsTypedUI;

    [SerializeField] public int charactersTyped;
    [SerializeField] public int charactersCorrect;
    [SerializeField] public float typingSpeed;
    [SerializeField] public float accurecy;
    [SerializeField] public int waveReached;
    [SerializeField] public int wordsTyped;

    [SerializeField] GameTimer gametimer = new GameTimer();
    private Coroutine timerCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        timerCoroutine = StartCoroutine(startTimer());
    }

    // Update is called once per frame
    void Update()
    {
        string current_accurecy = charactersTyped > 0 ? Math.Round((float)charactersCorrect / charactersTyped * 100) + "%" : "0%";
        timer.UpdateText($"Time:{gametimer.GetTime()}");
        accurecyUI.UpdateText($"Accurecy:{current_accurecy}");
        wordsTypedUI.UpdateText($"Words:{wordsTyped}");
    }


    // return an object with the current stats
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


    // start the game timer
    private IEnumerator startTimer()
    {
        while (true)
        {
            gametimer.IncreseScond();
            yield return new WaitForSeconds(1);
        }
    }

    public void IncreaseCharactersTyped() => charactersTyped++;
    public void IncreaseCorrectCharactersTyped() => charactersCorrect++;
    public void IncreaseWordsTyped() => wordsTyped++;
    public void SetWaveReached(int wave)
    {
        waveReached = wave;
        StopCoroutine(timerCoroutine);
    }
}
