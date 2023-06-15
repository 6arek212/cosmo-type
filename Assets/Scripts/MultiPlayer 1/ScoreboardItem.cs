using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class ScoreboardItem : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_Text userNameText;

    [SerializeField]
    private TMP_Text accurecyText;

    [SerializeField]
    private TMP_Text wordsText;

    [SerializeField]
    private TMP_Text finalScoreText;

    Player player;

    public void Setup(Player player)
    {
        userNameText.text = player.NickName;
        this.player = player;
        UpdateStats(player);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer != this.player)
            return;
        Debug.Log("---------------------Changing" + changedProps);

        if (changedProps.ContainsKey("accurecy"))
        {
            Debug.Log("---------------------accurecyyyyyyyyyyyy" + changedProps);
            UpdateStats(targetPlayer);
        }
    }

    public void UpdateStats(Player player)
    {
        if (player.CustomProperties.TryGetValue("accurecy", out object accurecy))
        {
            accurecyText.text = accurecy.ToString();
        }
    }
}
