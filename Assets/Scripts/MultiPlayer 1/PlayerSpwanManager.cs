using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerSpwanManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;

    [SerializeField]
    private GameObject[] PlayersCharactersPrefabs;
    ExitGames.Client.Photon.Hashtable playerProperties = PhotonNetwork.LocalPlayer.CustomProperties;

    void Awake()
    {
        SpawnPlayer();
    }

    public void SpawnPlayer()
    {
        int characterIndex = GetPlayerCharacter();
        Transform spawnPoint = spawnPoints[PhotonNetwork.LocalPlayer.ActorNumber - 1];
        PhotonNetwork.Instantiate(
            Path.Combine("PhotonPrefabs", PlayersCharactersPrefabs[characterIndex].name),
            spawnPoint.position,
            Quaternion.identity,
            0
        );
        /*     playerController.SetUp(PhotonNetwork.LocalPlayer);*/
    }

    public Transform GetSpawnPoint(int playerIndex)
    {
        return spawnPoints[playerIndex];
    }

    public int GetPlayerCharacter()
    {
        if (!playerProperties.ContainsKey("playerAvatar"))
            return 0;

        int avatarIndex = (int)playerProperties["playerAvatar"];
        return avatarIndex;
    }
}
