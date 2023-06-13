using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class TargetsManagerNetWork : MonoBehaviourPunCallbacks
{
    [SerializeField]
    PhotonView photonView;

    [SerializeField]
    bool ranodomSpawn;
    int _targetsAllowed = 1;
    private EnemySpawnManager enemySpawnerManager;

    private void Start()
    {
        enemySpawnerManager = GameObject
            .FindGameObjectWithTag("EnemySpawnManager")
            .GetComponent<EnemySpawnManager>();
    }

    public GameObject? FindTarget(GameObject gameObj, char searchChar)
    {
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
