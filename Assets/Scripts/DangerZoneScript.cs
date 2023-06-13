using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This Script represents the danger zone in buttom.

public class DangerZoneScript : MonoBehaviour
{
    private const string enemyTag = "Enemy";
    private float dely = 0.5f;
    private StatsManager statsManager;
    private TargetsManager targetsManager;

    private void Start()
    {
        targetsManager = GameObject.FindGameObjectWithTag("TargetsManager").GetComponent<TargetsManager>();
        statsManager = GameObject.FindGameObjectWithTag("StatsManager").GetComponent<StatsManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != enemyTag)
            return;

        statsManager.SetWaveReached(targetsManager.wave);
        GameStats stats = statsManager.GetStats();
        Debug.Log(statsManager.GetStats().accurecy);
    }

    //go to the game over scene.
    private void GameOver()
    {
    }
}
