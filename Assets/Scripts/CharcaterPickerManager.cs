using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;
public class CharcaterPickerManager : MonoBehaviourPunCallbacks
{

    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    [SerializeField] Sprite[] avatars;
    [SerializeField] Image SelectedAvatar;



    public void Awake()
    {
        playerProperties["playerAvatar"] = 0;
    }



    public void OnClickLeftArrow()
    {
  
        if ((int)playerProperties["playerAvatar"] == 0)
        {
            playerProperties["playerAvatar"] = avatars.Length - 1;
        }
        else
        {
       
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] - 1;
        }

        int avatarIndex = (int)playerProperties["playerAvatar"];
 
        SelectedAvatar.sprite = avatars[avatarIndex];

        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void OnClickRightArrow()
    {
        Debug.Log(playerProperties);

       if ((int)playerProperties["playerAvatar"] == avatars.Length - 1)
        {
            playerProperties["playerAvatar"] = 0;
        }
        else
        {
       
            playerProperties["playerAvatar"] = (int)playerProperties["playerAvatar"] + 1;
        }
        int avatarIndex = (int)playerProperties["playerAvatar"];
 
        SelectedAvatar.sprite = avatars[avatarIndex];
        //notify all players that custom prop of a player is changed
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }


    public Sprite[] GetAvatars()
    {
        return avatars;
    }

    public void UpdatePlayerAvater(Player player,Image playerAvatar)
    {

        if (player.CustomProperties.ContainsKey("playerAvatar"))
        {

            int avatarIndex = (int)player.CustomProperties["playerAvatar"];
            playerAvatar.sprite = avatars[avatarIndex];
        } else
        {
            playerProperties["playerAvatar"] = 0;

        }
   


    }




}
