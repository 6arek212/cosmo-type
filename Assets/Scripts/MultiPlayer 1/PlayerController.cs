using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerController : MonoBehaviourPunCallbacks
{

    [SerializeField] private TMP_Text accurecyText;
    Player player;
    

    public void SetUp(Player player)
    {
        this.player = player;
        UpdatePlayerStats(player);

    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (this.player != targetPlayer) return;
        if (changedProps.ContainsKey("accurecy"))
        {
            UpdatePlayerStats(targetPlayer);
            Debug.Log("---------------------accurecyyyyyyyyyyyy" + changedProps);
            /* UpdateStats(targetPlayer);*/
        }

    }
    public void UpdatePlayerStats(Player player)
    {
        if (player.CustomProperties.TryGetValue("accurecy", out object accurecy))
        {
            accurecyText.text = accurecy.ToString();
            Debug.Log(accurecy);
        }
        else
        {
            accurecyText.text = "0%";
        }




    }

}
