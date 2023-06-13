using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks
{

    /*    [SerializeField] List<GameObject> targets;
    
    
        [SerializeField] PhotonView photonView;
    
        [SerializeField] bool ranodomSpawn;
        int _targetsAllowed = 1;
    
        public int Count { get { return targets.Count; } }
    
        public static event Action<GameObject> OnTargetRemoved;
    
        private EnemySpawnManager enemySpawnerManager;
    
    
    
    
        private void Start()
        {
    
            enemySpawnerManager = GameObject.FindGameObjectWithTag("EnemySpawnManager").GetComponent<EnemySpawnManager>();
    
        }
    
        public GameObject? FindTarget(GameObject gameObj, char searchChar)
        {
            if (!photonView.IsMine) return null;
    
    
            MeshText enemyText;
            float minDist = float.MaxValue;
            GameObject? target = null;
    
    
            var targets = enemySpawnerManager.GetTargets();
            foreach (GameObject enemy in targets)
            {
                enemyText = enemy.GetComponentInChildren<MeshText>();
                float dist = Vector3.Distance(gameObj.transform.position, enemy.transform.position);
                if (enemyText.Length > 0 && enemyText.FirstChar() == searchChar && dist < minDist)
                {
                    target = enemy;
                    minDist = dist;
                }
            }
    
            return target;
        }
    
        public void RemoveTarget(GameObject gameObject)
        {
            if (!photonView.IsMine) return;
    
            enemySpawnerManager.RemoveTarget(gameObject);
            targets.Remove(gameObject);
            OnTargetRemoved?.Invoke(gameObject);
            _targetsAllowed++;
        }
    
    
        public void AddTarget(GameObject target)
        {
            if (!photonView.IsMine) return;
            enemySpawnerManager.RemoveTarget(target);
            targets.Add(target);
        }
    
        public int Count { get { return enemySpawnerManager.Count; } }
    */
}
