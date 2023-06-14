using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class ExitMenuMultiPlayer : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject ExitMenuPanel;
    public static bool MenuIsActive = false;
    private bool isGameOver = false;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape)|| isGameOver) return;

        if (MenuIsActive)
        {
            HideMenu();
        }
        else
        {
            ShowMenu();
        }


    }


    public void ShowMenu()
    {
        MenuIsActive = true;
        ExitMenuPanel.SetActive(true);
    }

    public void HideMenu()
    {
   
        MenuIsActive = false;
        ExitMenuPanel.SetActive(false);
    }


    public void LeaveGame()
    {
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LeaveRoom();
    }
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        isGameOver = true;
        Debug.Log("master switched");
        LeaveGame();
    }


    public override void OnLeftRoom()
    {

        //GO THE LOBBY
        PhotonNetwork.LocalPlayer.CustomProperties.Clear();
        SceneManager.LoadScene("MULTIPLAYERMODE");
    
    }

    public void LeaveMultiPlayerLobby()
    {
        HideMenu();
        PhotonNetwork.Disconnect();
     
        SceneManager.LoadScene("MAIN_MENU");
        
    }

}
