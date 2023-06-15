using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.UI;


/*This class represents the picking character of the player*/


public class CharcaterPickerManager : MonoBehaviourPunCallbacks
{
    ExitGames.Client.Photon.Hashtable playerProperties = new ExitGames.Client.Photon.Hashtable();

    [SerializeField]
    Sprite[] avatars;

    [SerializeField]
    Image SelectedAvatar;

    const string PLAYER_AVATAR_KEY = "playerAvatar";

    public void Awake()
    {
        playerProperties[PLAYER_AVATAR_KEY] = 0;
    }

    public void OnClickLeftArrow()
    {
        if ((int)playerProperties[PLAYER_AVATAR_KEY] == 0)
        {
            playerProperties[PLAYER_AVATAR_KEY] = avatars.Length - 1;
        }
        else
        {
            playerProperties[PLAYER_AVATAR_KEY] = (int)playerProperties[PLAYER_AVATAR_KEY] - 1;
        }

        int avatarIndex = (int)playerProperties[PLAYER_AVATAR_KEY];

        SelectedAvatar.sprite = avatars[avatarIndex];

        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public void OnClickRightArrow()
    {
        Debug.Log(playerProperties);

        if ((int)playerProperties[PLAYER_AVATAR_KEY] == avatars.Length - 1)
        {
            playerProperties[PLAYER_AVATAR_KEY] = 0;
        }
        else
        {
            playerProperties[PLAYER_AVATAR_KEY] = (int)playerProperties[PLAYER_AVATAR_KEY] + 1;
        }
        int avatarIndex = (int)playerProperties[PLAYER_AVATAR_KEY];

        SelectedAvatar.sprite = avatars[avatarIndex];
        //notify all players that custom prop of a player is changed
        PhotonNetwork.SetPlayerCustomProperties(playerProperties);
    }

    public Sprite[] GetAvatars()
    {
        return avatars;
    }

    public void UpdatePlayerAvater(Player player, Image playerAvatar)
    {
        if (player.CustomProperties.ContainsKey(PLAYER_AVATAR_KEY))
        {
            int avatarIndex = (int)player.CustomProperties[PLAYER_AVATAR_KEY];
            playerAvatar.sprite = avatars[avatarIndex];
        }
        else
        {
            playerProperties[PLAYER_AVATAR_KEY] = 0;
        }
    }
}


