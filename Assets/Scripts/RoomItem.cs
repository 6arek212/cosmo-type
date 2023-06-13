using UnityEngine;
using TMPro;
using Photon.Realtime;


public class RoomItem : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI lobbyNameText;
    [SerializeField] private TextMeshProUGUI playersText;
    [SerializeField] private TextMeshProUGUI gameModeText;
     private LobbyManagement manager;

    RoomInfo info;


    private void Start()
    {
        
        manager = GameObject
            .FindGameObjectWithTag("LobbyManagement")
            .GetComponent<LobbyManagement>();

    }


    public void SetUp(RoomInfo info)
    {
        this.info = info;
        lobbyNameText.text = info.Name;
        playersText.text = info.PlayerCount + "/" + info.MaxPlayers;
    }


    public void UpdateRoom(string name,int maxPlayers,int numbOfPlayers)
    {
        lobbyNameText.text = name;
        playersText.text = numbOfPlayers + "/" + maxPlayers;
     /*   gameModeText.text = lobby.Data[LobbyManager.KEY_GAME_MODE].Value;*/

    }


    public void OnClickRoom()
    {
   /*     LobbyManagement.Instance.JoinRoom(lobbyNameText.text);*/
        manager.JoinRoom(lobbyNameText.text);
    }


}
