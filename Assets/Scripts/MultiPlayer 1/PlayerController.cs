using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerController : MonoBehaviourPunCallbacks
{
/*    [SerializeField]
    private PhotonView photonView;
    [SerializeField] TMP_Text accurecyText;

    private StatsManagerNetwork statsManager;

    private void Awake()
    {
        statsManager = GameObject.FindGameObjectWithTag("StatsManager").GetComponent<StatsManagerNetwork>();
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        photonView.RPC("UpdateAccurecy", RpcTarget.All);
    }

    [PunRPC]
    public void UpdateAccurecy()
    {
        if (!statsManager) return;
        GameStats stats = statsManager.GetStats();
        accurecyText.text = stats.accurecy;
    }*/

}
