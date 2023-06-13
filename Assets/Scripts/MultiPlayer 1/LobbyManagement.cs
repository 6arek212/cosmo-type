using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;
using Assets.Scripts;
using System;
//this class represents the lobby manager on the game.

public class LobbyManagement : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private TMP_InputField lobbyNameInput;

    [SerializeField]
    private TMP_InputField maxPlayersInput;

    [SerializeField]
    public TMP_Text roomName;

    [SerializeField]
    private TMP_InputField playerName;

    [SerializeField]
    public TMP_Text playerCount;

    [SerializeField]
    private TMP_Text errorText;

    [SerializeField]
    Transform roomListContent;

    [SerializeField]
    private GameObject roomItemPrefab;

    [SerializeField]
    Transform playerListContent;

    [SerializeField]
    private GameObject playerListItemPrefab;

    [SerializeField]
    private GameObject startGameBtn;

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();



    //connect to the server
    public void Connect()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
        MenuManager.Instance.OpenMenu("loading");
    }

    //callback when connect to the server
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        //switch the scene for all the players 
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    //callback when we succssfully joined the lobby in the server
    // load the lobby scene
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        MenuManager.Instance.OpenMenu("lobby");
        if (playerName.text.Length > 0)
        {
            PhotonNetwork.NickName = playerName.text;
        }
       
    }

    public void OnClickCreateRoomMenu()
    {
        MenuManager.Instance.OpenMenu("lobbyCreate");
    }

    //when create btn clicked, create room
    public void CreateRoom()
    {
        if (!CheckValidations())
            return;
        int lobbyLimit = Int32.Parse(maxPlayersInput.text);
        PhotonNetwork.CreateRoom(
            lobbyNameInput.text,
            new RoomOptions() { MaxPlayers = lobbyLimit , IsVisible = true ,BroadcastPropsChangeToAll=true}
        );
        MenuManager.Instance.OpenMenu("loading");
    }

    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
        MenuManager.Instance.OpenMenu("loading");
    }

    public bool CheckValidations()
    {
        if (string.IsNullOrEmpty(lobbyNameInput.text) || string.IsNullOrEmpty(maxPlayersInput.text))
        {
            return false;
        }
        return true;
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");
        MenuManager.Instance.OpenMenu("room");
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        UpdateRoomPlayersCount();
        UpdatePlayerList(); // when we joind the room 
        ClearnInputs();
        CheckGameReady();
  
    }

    //if the master client leaves the room
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameBtn.SetActive(PhotonNetwork.IsMasterClient);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creating Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");
        MenuManager.Instance.OpenMenu("lobby");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
       
        foreach (Transform trams in roomListContent)
        {
            Destroy(trams.gameObject);
        }

        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
                continue;
            Instantiate(roomItemPrefab, roomListContent).GetComponent<RoomItem>().SetUp(info);
        }
    }

    //new player enters the room
    public override void OnPlayerEnteredRoom(Player newPlayer) 
    {
        Instantiate(playerListItemPrefab, playerListContent)
            .GetComponent<PlayerListItem>()
            .SetUp(newPlayer);
    }

    public void UpdatePlayerList()
    {
        Player[] players = PhotonNetwork.PlayerList;

        foreach (Transform child in playerListContent)
        {
            Destroy(child.gameObject);
        }
        foreach (Player player in players)
        {
            PlayerListItem newPlayer = Instantiate(playerListItemPrefab, playerListContent)
                .GetComponent<PlayerListItem>();
            newPlayer.SetUp(player);
       
        }

    }

    private void ClearnInputs()
    {
        lobbyNameInput.text = "";
        maxPlayersInput.text = "";
    }

    public void UpdateRoomPlayersCount()
    {
        playerCount.text =
            PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    public void CheckGameReady()
    {
        bool isGameReady = true;
        foreach (Player player in PhotonNetwork.CurrentRoom.Players.Values)
        {
            if (
                !player.CustomProperties.ContainsKey("isReady")
                || !(bool)player.CustomProperties["isReady"]
            )
            {
                isGameReady = false;
                break;
            }
        }
        startGameBtn.SetActive(PhotonNetwork.IsMasterClient && isGameReady);
    }

    public void StartGame()
    {
        //load all the players at once
        PhotonNetwork.LoadLevel("MULTIPLAYER");
    }


}
