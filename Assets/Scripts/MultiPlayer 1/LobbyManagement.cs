using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System;



/*This class represents the lobby manager in the game. It handles various lobby-related functionalities and interactions.*/

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



    //Connect to the server.
    public void Connect()
    {
        Debug.Log("Connecting to Master");
        PhotonNetwork.ConnectUsingSettings();
        MenuManager.Instance.OpenMenu("loading");
    }

    //Callback when connected to the server.
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        //switch the scene for all the players 
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Callback when successfully joined the lobby. Load the lobby scene.
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        MenuManager.Instance.OpenMenu("lobby");
        if (playerName.text.Length > 0)
        {
            PhotonNetwork.NickName = playerName.text;
        }
       
    }

    // Open the lobby creation menu when the create room button is clicked.
    public void OnClickCreateRoomMenu()
    {
        MenuManager.Instance.OpenMenu("lobbyCreate");
    }

    // Create a room when the create button is clicked.
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

    // Join a room with the specified room name.
    public void JoinRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
        MenuManager.Instance.OpenMenu("loading");
    }

    // Check if the input fields for creating a room are valid.
    public bool CheckValidations()
    {
        if (string.IsNullOrEmpty(lobbyNameInput.text) || string.IsNullOrEmpty(maxPlayersInput.text))
        {
            return false;
        }
        return true;
    }

    // Callback when successfully joined a room.
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

  

    // Callback when the master client switches in the room.
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        startGameBtn.SetActive(PhotonNetwork.IsMasterClient);
    }

    // Callback when room creation fails.
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        errorText.text = "Room Creating Failed: " + message;
        MenuManager.Instance.OpenMenu("error");
    }

    // Leave the current room when the leave room button is clicked.
    public void OnClickLeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        MenuManager.Instance.OpenMenu("loading");
    }

    // Callback when left the current room.
    public override void OnLeftRoom()
    {
        Debug.Log("Left Room");
        PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        MenuManager.Instance.OpenMenu("lobby");
    }

    // Callback for updating the room list.
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

    // Callback when a new player enters the room.
    public override void OnPlayerEnteredRoom(Player newPlayer) 
    {
        Instantiate(playerListItemPrefab, playerListContent)
            .GetComponent<PlayerListItem>()
            .SetUp(newPlayer);
    }

    // Update the player list in the room.
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

    // Clear the input fields.
    private void ClearnInputs()
    {
        lobbyNameInput.text = "";
        maxPlayersInput.text = "";
    }

    // Update the player count in the room.
    public void UpdateRoomPlayersCount()
    {
        playerCount.text =
            PhotonNetwork.CurrentRoom.PlayerCount + "/" + PhotonNetwork.CurrentRoom.MaxPlayers;
    }

    // Check if the game is ready to start.
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

    // Start the game by loading the multiplayer scene.
    public void StartGame()
    {
        //load all the players at once
        PhotonNetwork.LoadLevel("MULTIPLAYER");
    }


}
