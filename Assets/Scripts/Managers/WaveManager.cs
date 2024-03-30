using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    public int CurrentWave = 0;
    private List<GameObject> EnemiesAlive = new List<GameObject>();
    public GameObject SlimePrefab;
    public GameObject SkeletonPrefab;

    public Action OnPlayTurn;

    private List<Wave> waves = new List<Wave>{
        new Wave(new List<string>{"slime", "slime", "slime"}),
        new Wave(new List<string>{"slime", "slime", "skeleton","slime", "slime"}),
        new Wave(new List<string>{"slime", "skeleton", "skeleton","slime", "slime", "slime", "slime"}),
        new Wave(new List<string>{ "skeleton", "skeleton", "skeleton", "skeleton", "skeleton", "skeleton" }),
    };


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNextTurnAndPerformSideEffects()
    {
        if(EnemiesAlive.Count == 0) {
            SpawnNextWave();
        }

        PlayEnemiesTurn();
    }

    public void SpawnNextWave()
    {
        CurrentWave++;
        Wave currentEnemyWave = waves[CurrentWave-1];
        GameObject nextSpawnPrefab;

        foreach (string enemyToSpawn in currentEnemyWave.mobs)
        {
            if (enemyToSpawn == "slime")
            {
                nextSpawnPrefab = SlimePrefab;
            }
            else
            {
                nextSpawnPrefab = SkeletonPrefab;
            }
            GameTile tile = GameManager.Instance.GameBoard.GetRandomEmptyCell();
            GameObject entity = Instantiate(nextSpawnPrefab, new Vector3(tile.x, tile.y, 0), Quaternion.identity, null);
            tile.entity = entity;
            entity.GetComponent<Enemy>().CurrentTile = tile;
            EnemiesAlive.Add(entity);
        }
        UIManager.Instance.UpdateCurrentWave();
    }

    public void PlayEnemiesTurn()
    {
        foreach (GameObject enemy in EnemiesAlive.ToArray())
        {
            (enemy.GetComponent<MonoBehaviour>() as Enemy).PlayTurn();
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        Destroy(enemy);
        EnemiesAlive.Remove(enemy);
    }

    public void AddEnemy(GameObject enemy)
    {
        EnemiesAlive.Add(enemy);
    }
}
