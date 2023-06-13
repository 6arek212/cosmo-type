using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This Script represents the danger zone in buttom.

public class DangerZoneScript : MonoBehaviour
{
    private const string enemyTag = "Enemy";
    private float dely = 0.5f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(enemyTag))
        {
            Invoke("GameOver", dely);
        }
    }

    //go to the game over scene.
    private void GameOver()
    {
        int lastSceneIndex = SceneManager.sceneCountInBuildSettings - 1;
        SceneManager.LoadScene(lastSceneIndex);
    }
}
