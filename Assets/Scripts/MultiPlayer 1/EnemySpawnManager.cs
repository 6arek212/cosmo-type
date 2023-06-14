using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.IO;
using System;
using System.Threading.Tasks;
using System.Linq;
using UnityEngine.Networking;
using Assets.Scripts;
using TMPro;

// this class represents the spawn of enemies in the game

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    List<GameObject> targets;

    [SerializeField] GameObject WavePanel;

    [SerializeField] int maxWave = 4;

    [SerializeField] int wave = 1;

    [SerializeField] float spawnDelay = 2f;

    [SerializeField] private float minSpawnDelay = .2f;

    [SerializeField] float maxSpeed = 8;

    [SerializeField] float currentIntialSpeed = 4;

    [SerializeField] int maxParrallelEnemies = 14;

    [SerializeField] int currentParrallelEnemiesLimit = 6;

    [SerializeField] float maxSpawnHDistance = 3f;

    [SerializeField] float maxSpawnVDistance = 3f;

    [SerializeField] float uiDelay = 3.5f;

    [SerializeField] float speedIncrease = 0.04f;
    [SerializeField] int enmeyIncrease = 2;
    [SerializeField] float spawnDelayDecreas = .4f;

    private List<List<Word>> loadedWords;

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
            StartCoroutine(SpawnTargets());
        }
    }

    private IEnumerator LoadWordsFromFile(string filename)
    {
        filename = string.Join("/", Application.streamingAssetsPath, "JsonFiles", "MultiPlayerMode", filename);

        // In the Unity Editor, you need to use a different file access method

#if UNITY_EDITOR

        // Read the JSON file
        string jsonString = System.IO.File.ReadAllText(filename);

#else
        // In a built game, you need to use Unity's WWW or UnityWebRequest classes to read the file
        UnityWebRequest www = UnityWebRequest.Get(filename);
        yield return www.SendWebRequest();

        string jsonString = www.downloadHandler.text;
#endif
        // Deserialize the JSON data into a list of WordData objects
        yield return loadedWords = JsonUtility.FromJson<WordDataList>(jsonString).words.Select(s => s.word).ToList().Shuffle().ToList();
    }

    public IEnumerator SpawnTargets()
    {
        while (true)
        {
            yield return StartCoroutine(LoadWordsFromFile($"wave-{wave}.json"));

            // update UI text
            yield return new WaitForSeconds(1);
            yield return StartCoroutine(UpdateUI($"Wave {wave} Starting"));

            // start spawner
            targets = new List<GameObject>();
            yield return StartCoroutine(SpawnRoutine(loadedWords));

            loadedWords = null;
            wave++;
            currentIntialSpeed = Mathf.Clamp(speedIncrease + currentIntialSpeed, 1, maxSpeed);
            spawnDelay = Mathf.Clamp(spawnDelay - spawnDelayDecreas, minSpawnDelay, float.MaxValue);
            currentParrallelEnemiesLimit = Mathf.Clamp(currentParrallelEnemiesLimit + enmeyIncrease, 1, maxParrallelEnemies);

            if (wave > maxWave)
            {
                // end game 
                onEndGame();
                break;
            }
        }
    }

    private void onEndGame()
    {
        Debug.Log("Game Ended");
    }

    IEnumerator UpdateUI(string message)
    {
        WavePanel.SetActive(true);
        WavePanel.GetComponentInChildren<TMP_Text>().text = message;
        yield return new WaitForSeconds(uiDelay);
        WavePanel.SetActive(false);
        yield return new WaitForSeconds(1);
    }


    //spawn the enemies at random spawn points.

    IEnumerator SpawnRoutine(List<List<Word>> words)
    {
        GameObject obj;
        TextTypeNetwork textType;
        int delay = 1;

        foreach (List<Word> wordList in words)
        {
            obj = PhotonNetwork.Instantiate(
                Path.Combine("PhotonPrefabs", "EnemyNetWork"),
                GetRandomSpawnPosition(),
                Quaternion.identity
            );

            textType = obj.GetComponent<TextTypeNetwork>();
            textType.SetWords(wordList);

            // set speed
            obj.GetComponent<MoverNetwork>().SetSpeed(currentIntialSpeed);

            photonView.RPC("AddTarget", RpcTarget.All, obj.GetComponent<PhotonView>().ViewID);

            yield return new WaitForSeconds(spawnDelay);

            yield return new WaitUntil(() => currentParrallelEnemiesLimit > targets.Count);
        }

        yield return new WaitUntil(() => targets.Count == 0);
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
    }

    protected Vector3 GetRandomSpawnPosition()
    {
        float x = UnityEngine.Random.Range(transform.position.x - maxSpawnHDistance, transform.position.x + maxSpawnHDistance);
        float y = UnityEngine.Random.Range(transform.position.y - maxSpawnVDistance, transform.position.y + maxSpawnVDistance);
        return new Vector3(x, y);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(maxSpawnHDistance * 2, maxSpawnVDistance * 2, 0));
    }
}