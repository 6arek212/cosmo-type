using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitMenuMultiPlayer : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject ExitMenuPanel;
    public static bool MenuIsActive = false;
    private bool isGameOver = false;

    private void Update()
    {
        if (!Input.GetKeyDown(KeyCode.Escape) || isGameOver)
            return;

        if (MenuIsActive)
            HideMenu();
        else
            ShowMenu();
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
        isGameOver = true;
        PhotonNetwork.DestroyPlayerObjects(PhotonNetwork.LocalPlayer);
        PhotonNetwork.LeaveRoom();
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
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
