using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
public class ExistMenu : MonoBehaviourPunCallbacks
{
    [SerializeField] private GameObject ExitMenuPanel;
    [SerializeField] private GameObject Loading;
    [SerializeField] private bool enablePause;
    public static bool MenuIsActive = false;
    private bool isGameOver = false;
    private bool isLeavingRoom = false;
    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape) || isGameOver) return;

        if (MenuIsActive)
            HideMenu();
        else
            ShowMenu();
    }

    // show the game menu
    public void ShowMenu()
    {
        if (enablePause)
        {
            Time.timeScale = 0f;
        }
        MenuIsActive = true;
        ExitMenuPanel.SetActive(true);
    }

    // hide the game menu
    public void HideMenu()
    {
        if (enablePause)
        {
            Time.timeScale = 1f;
        }
        MenuIsActive = false;
        ExitMenuPanel.SetActive(false);
    }

    public void LeaveMultiPlayerLobby()
    {
        if (PhotonNetwork.InRoom)
        {
            isLeavingRoom = true;
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            PhotonNetwork.Disconnect();
            LoadMainMenu();
        }
    }

    public override void OnLeftRoom()
    {
        if (isLeavingRoom)
        {
            isLeavingRoom = false;
            PhotonNetwork.Disconnect();
        }
    }


    public override void OnDisconnected(DisconnectCause cause)
    {
            LoadMainMenu();
    }


    public void LoadMainMenu()
    {
        HideMenu();
        SceneManager.LoadScene("MAIN_MENU");
    }

    public void SetGameOver()
    {
        isGameOver = true;
    }

}