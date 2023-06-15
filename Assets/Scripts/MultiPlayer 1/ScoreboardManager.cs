using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class ScoreboardManager : MonoBehaviourPunCallbacks
{
    [SerializeField]
    Transform container;

    [SerializeField]
    GameObject scoreboardItemPrefab;
    Dictionary<Player, ScoreboardItem> scoreboardItems = new Dictionary<Player, ScoreboardItem>();
    private StatsManagerNetwork statsManager;

    private void Start()
    {
        InitializeBoard();
    }

    public void InitializeBoard()
    {
        /*   foreach(Player player in PhotonNetwork.PlayerList)
           {
               AddScoreboardItem(player);
           }*/
        foreach (Transform trams in container)
        {
            Destroy(trams.gameObject);
        }

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            AddScoreboardItem(player);
        }
    }

    public void OnEnable()
    {
        Debug.Log("ENABLE");
        InitializeBoard();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        AddScoreboardItem(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        RemoveScoreboardItem(otherPlayer);
    }

    public void AddScoreboardItem(Player player)
    {
        ScoreboardItem item = Instantiate(scoreboardItemPrefab, container)
            .GetComponent<ScoreboardItem>();
        statsManager = GameObject
            .FindGameObjectWithTag("StatsManager")
            .GetComponent<StatsManagerNetwork>();
        if (statsManager != null)
        {
            item.Setup(player);
            scoreboardItems[player] = item;
        }
    }

    public void RemoveScoreboardItem(Player player)
    {
        Destroy(scoreboardItems[player].gameObject);
        scoreboardItems.Remove(player);
    }
}
