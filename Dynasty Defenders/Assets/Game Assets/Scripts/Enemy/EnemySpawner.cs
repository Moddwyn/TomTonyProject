using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dreamteck.Splines;
using NaughtyAttributes;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [AllowNesting] public List<EnemyContainer> enemyContainers = new List<EnemyContainer>();
    [ReadOnly] public List<Enemy> enemiesInWorld = new List<Enemy>();

    [HorizontalLine]
    public float breakTimeBetweenRounds = 5;
    public float timeBetweenSpawns = 1.5f;
    public List<int> enemyPerRound = new List<int>();
    [ReadOnly] public int currentRound;

    SplineComputer spline;

    void Awake()
    {
        spline = FindObjectOfType<SplineComputer>();

        foreach (var ec in enemyContainers)
            ec.pool = GameObject.FindGameObjectWithTag(ec.poolTag).GetComponent<ObjectPooler>();
    }

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    void Update()
    {
        enemiesInWorld = enemiesInWorld.Where(x=>x.gameObject.activeInHierarchy).ToList();
    }

    IEnumerator SpawnRoutine()
    {
        int spawnAmount = enemyPerRound[currentRound];
        yield return new WaitForSeconds(breakTimeBetweenRounds);
        for (int i = 0; i < spawnAmount; i++)
        {
            SpawnRandomEnemy();
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        currentRound++;
        if (currentRound < enemyPerRound.Count) StartCoroutine(SpawnRoutine());
    }

    void SpawnRandomEnemy()
    {
        if (enemyContainers.Count == 0) return;

        float totalChance = enemyContainers.Sum(container => container.pickChance);
        float randomPoint = Random.Range(0, totalChance);
        float cumulative = 0f;

        EnemyContainer pickedEnemy = enemyContainers
            .OrderByDescending(container => container.pickChance)
            .FirstOrDefault(container => (cumulative += container.pickChance) >= randomPoint);

        if (pickedEnemy == null) pickedEnemy = enemyContainers.OrderByDescending(container => container.pickChance).First();

        Vector3 firstNodePosition = spline.Evaluate(0.0).position;

        Enemy newEnemy = pickedEnemy.pool.GrabFromPool(firstNodePosition + (Vector3.up/0.5f), Quaternion.identity).GetComponent<Enemy>();
        newEnemy.splineFollower.spline = spline;
        newEnemy.pool = pickedEnemy.pool;

        enemiesInWorld.Add(newEnemy);
    }

    public bool IsChancePicked(float chancePercent)
    {
        chancePercent = Mathf.Clamp(chancePercent, 0f, 100f);
        
        float randomValue = Random.Range(0f, 100f);
        
        return randomValue <= chancePercent;
    }

    [System.Serializable]
    public class EnemyContainer
    {
        public Enemy enemy;
        public float pickChance = 100;
        [Tag] public string poolTag;

        [HideInInspector] public ObjectPooler pool;
    }
}
