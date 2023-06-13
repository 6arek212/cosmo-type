using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;

public class PlayerManager : MonoBehaviour
{
/*    PhotonView PV;
    private void Awake()
    {
        PV = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PV.IsMine) // if the photon view owend by the local player
        {
            CreateController();
        }
    }

    //create a player at random position
    void CreateController()
    {
      
        Transform spawnPoint = PlayerSpwanManager.Instance.GetSpawnPoint(PhotonNetwork.LocalPlayer.ActorNumber-1);
        PhotonNetwork.Instantiate(Path.Combine("PhotonPrefabs", "PlayerController"), spawnPoint.position, Quaternion.identity, 0);

    }*/
}
