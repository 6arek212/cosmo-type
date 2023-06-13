using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System.IO;

public class RoomManager : MonoBehaviourPunCallbacks
{
    public static RoomManager Instance; //singlton pattern
    public static int count = 0;

    private void Awake()
    {

        /*       if (Instance == null)
               {
                   Instance = this;
               }
               else
               {
                   if (Instance != this)
                   {
                       Destroy(Instance.gameObject);
                       Instance = this;
                   }

               }
               DontDestroyOnLoad(this.gameObject);*/
        if (Instance)
        {
            Destroy(gameObject); // there can only be one
            return;
        }
        DontDestroyOnLoad(gameObject);
        Instance = this;
    }

    public override void OnEnable()
    {
        base.OnEnable();
        SceneManager.sceneLoaded += onSceneLoaded;
    }

    void onSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
    {
        Debug.Log(count);
        if (scene.buildIndex == 1) // we are in the game.
        {

            PhotonNetwork.Instantiate(
                Path.Combine("PhotonPrefabs", "PlayerManager"),
                Vector3.zero,
                Quaternion.identity
            );
            count++;
        }
        
    }
}
