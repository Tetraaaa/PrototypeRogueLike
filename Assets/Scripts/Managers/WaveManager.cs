using System;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    public int CurrentWave = 0;
    private List<GameObject> EnemiesAlive = new List<GameObject>();
    public GameObject SlimePrefab;
    public GameObject SkeletonPrefab;
    public GameObject KingSlimePrefab;

    public Action OnPlayTurn;

    private List<Wave> waves = new List<Wave>{
        new Wave(new List<string>{"slime", "slime"}),
        new Wave(new List<string>{"slime", "slime","slime", "slime"}),
        new Wave(new List<string>{"slime", "slime", "slime", "skeleton"}),
        new Wave(new List<string>{ "skeleton", "skeleton", "slime", "slime"}),
        new Wave(new List<string>{ "skeleton", "skeleton", "slime", "slime", "slime", "slime"}),
        new Wave(new List<string>{ "skeleton", "skeleton", "skeleton", "skeleton", "slime"}),
        new Wave(new List<string>{ "skeleton", "skeleton", "skeleton", "slime", "slime", "slime", "slime",  "slime",  "slime" }),
        new Wave(new List<string>{"slime", "skeleton", "skeleton","slime", "slime", "slime", "slime", "skeleton", "skeleton", "skeleton", "skeleton"}),
        new Wave(new List<string>{"slime", "skeleton", "skeleton","slime", "slime", "skeleton", "skeleton", "skeleton", "skeleton", "skeleton", "skeleton", "skeleton"}),
        new Wave(new List<string>{"king_slime"})
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

        foreach (string enemyToSpawn in currentEnemyWave.mobs)
        {
            Summon(enemyToSpawn, GameManager.Instance.GameBoard.GetRandomEmptyCell());
        }
        UIManager.Instance.UpdateCurrentWave();
    }

    public void Summon(string enemyName, GameTile tile)
    {
        GameObject nextSpawnPrefab = null;

        switch (enemyName) {
            case "slime":
                nextSpawnPrefab = SlimePrefab;
                break;
            case "skeleton":
                nextSpawnPrefab = SkeletonPrefab;
                break;
            case "king_slime":
                nextSpawnPrefab= KingSlimePrefab;
                break;
            default: 
                break;
        }

        if (!nextSpawnPrefab) return;

        GameObject entity = Instantiate(nextSpawnPrefab, new Vector3(tile.x, tile.y, 0), Quaternion.identity, null);
        tile.entity = entity;
        entity.GetComponent<Enemy>().CurrentTile = tile;
        EnemiesAlive.Add(entity);
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
