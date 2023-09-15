using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : Singleton<WaveManager>
{
    private int CurrentWave = 1;
    private int CurrentTurn = 0;
    private bool IsEnemyTurn = false;
    private int EnemiesRemainingThisRound = 15;
    private List<GameObject> EnemiesAlive = new List<GameObject>();
    public GameObject SlimePrefab;


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
        CurrentTurn++;

        PlayEnemiesTurn();

        bool shouldSpawnEnemiesThisTurn = CurrentTurn % GameSettings.DEFAULT_DELAY_BETWEEN_ENEMY_SPAWNS_IN_TURNS == 0;
        if (shouldSpawnEnemiesThisTurn)
        {
            SpawnSomeEnemies();
        }

        IsEnemyTurn = false;

    }

    public void PlayEnemiesTurn()
    {
        foreach (var enemy in EnemiesAlive)
        {
            enemy.GetComponent<Enemy>().PlayTurn();
        }
    }

    public void SpawnSomeEnemies()
    {
        if (EnemiesRemainingThisRound <= 0) return;
        GameTile tile = GameManager.Instance.GameBoard.GetRandomEmptyCell();

        GameObject entity = Instantiate(SlimePrefab, new Vector3(tile.x, tile.y, 0), Quaternion.identity, null);
        tile.entity = entity;
        EnemiesAlive.Add(entity);

        EnemiesRemainingThisRound--;
    }

    public void SpawnEnemy()
    {

    }
}
