using Fusion;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PositionSpawner : MonoBehaviour
{
    [SerializeField] TargetsManager targetMananger;

    [SerializeField] List<Transform> spawns;

    [SerializeField] float continueMovingDelay = 6f;

    [SerializeField] float spawnDelay = 3f;

    private Mover mover;
    private float eps = 3f;
    private bool spawnStarted = false;

    public List<Word> words { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        targetMananger = GameObject.FindGameObjectWithTag("TargetsManager").GetComponent<TargetsManager>();
        mover = GetComponent<Mover>();
    }


    private float DistFromCenter() => Mathf.Abs(Camera.main.transform.position.y - transform.position.y);

    // Update is called once per frame
    void Update()
    {
        if (!spawnStarted && DistFromCenter() < eps)
        {
            mover.enabled = false;
            spawnStarted = true;
            StartCoroutine(BossEnemySpawner());
        }
    }

    IEnumerator BossEnemySpawner()
    {
        Vector3 pos;
        int spawnerNum = 0;
        foreach (Word word in words)
        {
            pos = spawns[spawnerNum].position;
            GameObject enemy = targetMananger.SpawnEnemy(new List<Word> { word }, EnemyType.REGULAR, pos);
            yield return new WaitForSeconds(spawnDelay);
            spawnerNum = (spawnerNum + 1) % spawns.Count;
        }

        yield return new WaitForSeconds(continueMovingDelay);
        mover.enabled = true;
    }
}
