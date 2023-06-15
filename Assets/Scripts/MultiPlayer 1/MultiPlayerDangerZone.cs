using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using Photon.Realtime;
using Photon.Pun;

// This Script represents the danger zone in buttom.

public class MultiPlayerDangerZone : MonoBehaviour
{
    private const string enemyTag = "Enemy";
    private float dely = 0.5f;

    [SerializeField]
    GameObject endGamePanel;

    [SerializeField]
    private PhotonView photonView;

    private TargetsManagerNetWork targetsManager;

    private void Start()
    {
        targetsManager = GameObject
            .FindGameObjectWithTag("TargetsManager")
            .GetComponent<TargetsManagerNetWork>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag != enemyTag)
            return;

        GameOver();
    }

    [PunRPC]
    //go to the game over scene.
    public void GameOverRPC()
    {
        /*        if (!photonView.IsMine) return;*/
        endGamePanel.SetActive(true);
        targetsManager.CheckWinner();
    }

    public void GameOver() => photonView.RPC(nameof(GameOverRPC), RpcTarget.All);
}
