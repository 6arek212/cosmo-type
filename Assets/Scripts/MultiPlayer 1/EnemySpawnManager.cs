using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.Networking;

// this class represents the spawn of enemies in the game

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField]
    private Transform[] spawnPoints;

    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    List<GameObject> targets;

    [SerializeField]
    bool ranodomSpawn;

    int _targetsAllowed = 1;

    [SerializeField]
    private PhotonView photonView;
    private const string TargetsListKey = "TargetsList";
    public int Count
    {
        get { return targets.Count; }
    }

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnTargets();
        }
   
    
    
    }

    private async Task<List<List<Word>>> LoadWordsFromFile(string filename)
    {
        filename = string.Join("/", Application.streamingAssetsPath, filename);

        // In the Unity Editor, you need to use a different file access method

#if UNITY_EDITOR

        // Read the JSON file
        string jsonString = System.IO.File.ReadAllText(filename);

#else
        // In a built game, you need to use Unity's WWW or UnityWebRequest classes to read the file
        UnityWebRequest www = UnityWebRequest.Get(filename);
        var operation = www.SendWebRequest();

        while (!operation.isDone)
            await Task.Yield();

        string jsonString = www.downloadHandler.text;
#endif
        // Deserialize the JSON data into a list of WordData objects
        return JsonUtility.FromJson<WordDataList>(jsonString).words.Select(s => s.word).ToList();
    }

    public async Task SpawnTargets()
    {
        List<List<Word>> words = await LoadWordsFromFile("words-en.json");
        targets = new List<GameObject>();
        StartCoroutine(SpawnRoutine(words));
    }

    //spawn the enemies at random spawn points.

    IEnumerator SpawnRoutine(List<List<Word>> words)
    {
        GameObject obj;
        TextTypeNetwork textType;
        int delay = 1;

        foreach (List<Word> wordList in words)
        {
            Vector3 randomPostion = spawnPoints[
                UnityEngine.Random.Range(0, spawnPoints.Length)
            ].position;
            obj = PhotonNetwork.Instantiate(
                Path.Combine("PhotonPrefabs", "EnemyNetWork"),
                randomPostion,
                Quaternion.identity
            );

            textType = obj.GetComponent<TextTypeNetwork>();
            textType.SetWords(wordList);
            photonView.RPC("AddTarget", RpcTarget.All, obj.GetComponent<PhotonView>().ViewID);
          
            yield return new WaitUntil(() => targets.Count != _targetsAllowed);
        }
    }

    // send rpc, that new target has been added to the game.

    [PunRPC]
    private void AddTarget(int viewID)
    {
        PhotonView targetView = PhotonView.Find(viewID);
        targets.Add(targetView.gameObject);

    }

    public List<GameObject> GetTargets()
    {
        return targets;
    }

    //remove target from the game
    public void RemoveTarget(GameObject target)
    {
        targets.Remove(target);
        _targetsAllowed++;
    }



}