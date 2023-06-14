using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
public class TargetsManagerNetWork : MonoBehaviourPunCallbacks
{
    [SerializeField]
    PhotonView photonView;
    [SerializeField] private TMP_Text winnerName;
    [SerializeField] private TMP_Text winnerAccurcey;

    [SerializeField]
    bool ranodomSpawn;
    int _targetsAllowed = 1;
    private EnemySpawnManager enemySpawnerManager;
    private bool isGameOver = false;



    private void Start()
    {
   /*     statsManager = GameObject.FindGameObjectWithTag("StatsManager").GetComponent<StatsManagerNetwork>();*/
        enemySpawnerManager = GameObject
            .FindGameObjectWithTag("EnemySpawnManager")
            .GetComponent<EnemySpawnManager>();
    }

    public GameObject? FindTarget(GameObject gameObj, char searchChar)
    {

        if (isGameOver) return null;

        MeshText enemyText;
        float minDist = float.MaxValue;
        GameObject target = null;
        List<GameObject> targets = enemySpawnerManager.GetTargets();
   


        foreach (GameObject enemy in targets)
        {
            enemyText = enemy.GetComponentInChildren<MeshText>();
            TextTypeNetwork enemyTextType = enemy.GetComponent<TextTypeNetwork>();

            float dist = Vector3.Distance(gameObj.transform.position, enemy.transform.position);
            if (
                !enemyTextType.isTaken
                && enemyText.Length > 0
                && enemyText.FirstChar() == searchChar
                && dist < minDist
            )
            {
                target = enemy;
                minDist = dist;
            }
        }

        if (target)
        {
            TextTypeNetwork targetTextType = target.GetComponent<TextTypeNetwork>();
            targetTextType.isTaken = true;
        }
        if (target)
        {
            LockTarget(target);
        }

        return target;
    }

    public void LockTarget(GameObject target)
    {
        photonView.RPC("LockTargetRPC", RpcTarget.All, target.GetComponent<PhotonView>().ViewID);
    }

    [PunRPC]
    public void LockTargetRPC(int viewId)
    {
        PhotonView otherPhotonView = PhotonView.Find(viewId);
        TextTypeNetwork targetTextType = otherPhotonView.gameObject.GetComponent<TextTypeNetwork>();
        targetTextType.isTaken = true;
    }

    public void RemoveTarget(GameObject gameObject)
    {
        photonView.RPC(
            "RemoveTargetRPC",
            RpcTarget.All,
            gameObject.GetComponent<PhotonView>().ViewID
        );
    }


    public void CheckWinner()
    {
        Player highestAccuracyPlayer = null;
        float highestAccuracy = 0f;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            Debug.Log(player.CustomProperties);

            if (player.CustomProperties.TryGetValue("accurecy", out object accurecy))
            {
                Debug.Log(accurecy);
                if (float.TryParse(accurecy.ToString().TrimEnd('%'), out float accuracy))
                {
                    if (accuracy > highestAccuracy)
                    {
                        highestAccuracy = accuracy;
                        highestAccuracyPlayer = player;
                    }
                }
             
            }
         
        }

        if (highestAccuracyPlayer != null)
        {
            winnerName.text = highestAccuracyPlayer.NickName + " WINS!!!";
            winnerAccurcey.text = "Accurcey "+highestAccuracy;
        }

        isGameOver = true;

    }




    [PunRPC]
    private void RemoveTargetRPC(int viewId)
    {
        enemySpawnerManager.RemoveTarget(PhotonView.Find(viewId).gameObject);

    }



    public int Count
    {
        get { return enemySpawnerManager.Count; }
    }
}
