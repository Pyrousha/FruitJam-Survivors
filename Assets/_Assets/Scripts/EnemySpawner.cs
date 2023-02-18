using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Singleton<EnemySpawner>
{
    private List<EnemyHp> enemies = new List<EnemyHp>();
    [SerializeField] private float minDistToPlayer;
    [SerializeField] private float spawnCooldown;
    private float nextSpawnTime = -1;
    [SerializeField] private GameObject enemyPrefab;

    private Transform mainCamTransform;

    void Awake()
    {
        mainCamTransform = Camera.main.transform;
    }

    void Start()
    {
        SpawnNewEnemy();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
            SpawnNewEnemy();
    }

    public void OnEnemyKilled(EnemyHp _enemy)
    {
        enemies.Remove(_enemy);
    }

    private void SpawnNewEnemy()
    {
        float angle = Random.Range(0, Mathf.PI * 2);
        Vector3 spawnPos = new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0) * minDistToPlayer + mainCamTransform.position;
        spawnPos.z = 0;

        EnemyHp newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity).GetComponent<EnemyHp>();
        enemies.Add(newEnemy);

        nextSpawnTime = Time.time + spawnCooldown;
    }

    public Vector3 FindDirectionToNearestEnemy(Vector3 _position)
    {
        if (enemies.Count == 0)
        {
            //return random direction if no enemies exist
            float angle = Random.Range(0, Mathf.PI * 2);
            return new Vector3(Mathf.Sin(angle), Mathf.Cos(angle), 0);
        }

        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        foreach (EnemyHp enemy in enemies)
        {
            Transform potentialTarget = enemy.transform;
            Vector3 directionToTarget = potentialTarget.position - _position;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget.position - _position;
    }
}
