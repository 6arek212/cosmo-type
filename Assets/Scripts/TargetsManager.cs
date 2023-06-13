using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using static UnityEngine.GraphicsBuffer;
using TMPro;


[System.Serializable]
public class WordDataList
{
    public List<InnerWord> words;
}


[System.Serializable]
public class InnerWord
{
    public List<Word> word;
    public EnemyType type;
}

public enum EnemyType
{
    REGULAR,
    BOSS,
}

public enum ModeType
{
    [Description("NormalMode")]
    NORMAL,

    [Description("LearningMode")]
    LEARNING,

    [Description("StoryMode")]
    STORY,

    [Description("TutorialMode")]
    TUTORIAL,
    [Description("MultiLobby")]
    MULTIPLAYER
}


public class TargetsManager : MonoBehaviour
{
    [SerializeField] Transform player;

    [SerializeField] GameObject enemy;

    [SerializeField] GameObject boss;


    [SerializeField] GameObject WavePanel;

    [SerializeField] private int wave = 1;

    [SerializeField] float currentIntialSpeed = 4;

    [SerializeField] float maxSpeed = 8;

    [SerializeField] float maxSpawnHDistance = 3f;

    [SerializeField] float maxSpawnVDistance = 3f;

    [SerializeField] ModeType mode;

    [SerializeField] float spawnDelay = 2f;

    [SerializeField] private float minSpawnDelay = .2f;

    [SerializeField] bool shuffleWords = true;

    [SerializeField] int maxParrallelEnemies = 14;

    [SerializeField] int currentParrallelEnemiesLimit = 6;

    [SerializeField] private List<GameObject> targets;
    public int Count { get { return targets.Count; } }

    [SerializeField] float speedIncrease = 0.04f;
    [SerializeField] int enmeyIncrease = 2;
    [SerializeField] float spawnDelayDecreas = .4f;

    private IList<InnerWord> loadedWords;

    private bool isUIMessageDiplayed = true;

    // Start is called before the first frame update
    void Start()
    {
        if (mode == ModeType.TUTORIAL)
            return;
        targets = new List<GameObject>();
        StartCoroutine(SpawnTargets());
    }


    // Loading words from json file
    private IEnumerator LoadWordsFromFile(string filename)

    {
        filename = string.Join("/", Application.streamingAssetsPath, "JsonFiles", mode.DisplayName(), filename);

        // In the Unity Editor, you need to use a different file access method

#if UNITY_EDITOR
        Debug.Log(filename);
        // Read the JSON file
        string jsonString = System.IO.File.ReadAllText(filename);

#else
        // In a built game, you need to use Unity's WWW or UnityWebRequest classes to read the file
        UnityWebRequest www = UnityWebRequest.Get(filename);
        yield return www.SendWebRequest();

        string jsonString = www.downloadHandler.text;
#endif

        // Deserialize the JSON data into a list of WordData objects
        yield return loadedWords = JsonUtility.FromJson<WordDataList>(jsonString).words;
        if (shuffleWords)
            loadedWords = loadedWords.Shuffle();
    }

    private IEnumerator SpawnTargets()
    {
        while (true)
        {
            // display wave started
            isUIMessageDiplayed = true;
            StartCoroutine(UpdateUI($"Wave {wave} starting"));
            yield return new WaitUntil(() => !isUIMessageDiplayed);

            // load words
            StartCoroutine(LoadWordsFromFile($"wave-{wave}.json"));
            yield return new WaitUntil(() => loadedWords != null);

            // spawn enemies
            StartCoroutine(Spawn());
            yield return new WaitUntil(() => loadedWords.Count == 0);
            yield return new WaitUntil(() => targets.Count == 0);

            // show animation for ending rouund and wait 
            yield return new WaitForSeconds(3);

            loadedWords = null;
            wave++;
            currentIntialSpeed = Mathf.Clamp(speedIncrease + currentIntialSpeed, 1, maxSpeed);
            spawnDelay = Mathf.Clamp(spawnDelay - spawnDelayDecreas, minSpawnDelay, float.MaxValue);
            currentParrallelEnemiesLimit = Mathf.Clamp(currentParrallelEnemiesLimit + enmeyIncrease, 1, maxParrallelEnemies);
        }
    }


    IEnumerator UpdateUI(string message)
    {

        WavePanel.SetActive(true);
        WavePanel.GetComponentInChildren<TMP_Text>().text = message ;
        yield return new WaitForSeconds(2);
        WavePanel.SetActive(false);
        isUIMessageDiplayed = false;
    }

    protected virtual IEnumerator Spawn()
    {
        InnerWord wordObj;
        while (loadedWords.Count > 0)
        {
            wordObj = loadedWords.First();
            Debug.Log($"Spawned word {wordObj.word.First().text}");

            // pick a spawner 
            Vector3 spawnPosition = GetRandomSpawnPosition();//spawners[Random.Range(0, spawners.Count)].position;

            if (wordObj.type == EnemyType.BOSS)
            {
                // set targets 
                GameObject boss = SpawnEnemy(new List<Word> { wordObj.word.First() }, wordObj.type, spawnPosition);
                PositionSpawner spawner = boss.GetComponent<PositionSpawner>();
                spawner.words = wordObj.word.GetRange(1, wordObj.word.Count - 1);
            }
            else
            {
                SpawnEnemy(wordObj.word, wordObj.type, spawnPosition);
            }
            yield return new WaitForSeconds(spawnDelay);
            loadedWords.Remove(wordObj);

            // limit parrallel enemies
            yield return new WaitUntil(() => currentParrallelEnemiesLimit > targets.Count);
        }
    }


    protected Vector3 GetRandomSpawnPosition()
    {
        float x = Random.Range(transform.position.x - maxSpawnHDistance, transform.position.x + maxSpawnHDistance);
        float y = Random.Range(transform.position.y - maxSpawnVDistance, transform.position.y + maxSpawnVDistance);
        return new Vector3(x, y);
    }

    protected GameObject GetEnemyByType(EnemyType type)
    {
        if (type == EnemyType.BOSS)
            return boss;
        return enemy;
    }

    public GameObject SpawnEnemy(List<Word> words, EnemyType type, Vector3 positon)
    {
        GameObject obj;
        TextType textType;
        Mover mover;
        obj = Instantiate(GetEnemyByType(type), positon, Quaternion.identity);
        mover = obj.GetComponent<Mover>();
        mover.playerTransform = player;
        textType = obj.GetComponent<TextType>();
        textType.SetWords(words);
        targets.Add(obj);
        mover.SetSpeed(currentIntialSpeed);
        return obj;
    }


    // find the next target with min distance to the gameobject that starts with char that we first typed
#nullable enable
    public GameObject? FindTarget(GameObject gameObj, char searchChar)
    {
        MeshText enemyText;
        float minDist = float.MaxValue;
        GameObject? target = null;

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
        targets.Remove(gameObject);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(maxSpawnHDistance * 2, maxSpawnVDistance * 2, 0));
    }
}