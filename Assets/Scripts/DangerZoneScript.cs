using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
// This Script represents the danger zone in buttom.

public class DangerZoneScript : MonoBehaviour
{
    private const string enemyTag = "Enemy";
    private float dely = 0.5f;
    private StatsManager statsManager;
    private TargetsManager targetsManager;

    [SerializeField]
    private TMP_Text finalScoreText;


    [SerializeField]
    private TMP_Text accuracyText;

    [SerializeField]
    private TMP_Text waveReachedText;
    [SerializeField]
    private GameObject statsPanel;

    [SerializeField]
    private GameObject canvasManager;

   

    private void Start()
    {
        targetsManager = GameObject.FindGameObjectWithTag("TargetsManager").GetComponent<TargetsManager>();
        statsManager = GameObject.FindGameObjectWithTag("StatsManager").GetComponent<StatsManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != enemyTag)
            return;

        GameOver();
    }

    //go to the game over scene.
    private void GameOver()
    {
        Time.timeScale = 0f;
        statsPanel.SetActive(true);
        statsManager.SetWaveReached(targetsManager.wave);
        GameStats stats = statsManager.GetStats();
        accuracyText.text = String.Format(stats.accurecy, 123.47);
        finalScoreText.text = stats.wordsTyped;
        waveReachedText.text = stats.waveReached;
        canvasManager.GetComponentInChildren<ExistMenu>().SetGameOver();
        Debug.Log(statsManager.GetStats().accurecy);
    }
    public void LeaveGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MAIN_MENU");

    }

}
