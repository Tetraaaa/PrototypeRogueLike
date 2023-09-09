using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject EnemyPrefab;
    private int currentTurn = 0;

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
        currentTurn++;
        bool shouldSpawnEnemiesThisTurn = currentTurn % GameSettings.DEFAULT_DELAY_BETWEEN_ENEMY_SPAWNS_IN_TURNS == 0;
        if (shouldSpawnEnemiesThisTurn)
        {
            SpawnSomeEnemies();
        }

    }

    public void SpawnSomeEnemies()
    {
        int xPosition = Random.Range(-GameSettings.MAP_SIZE_IN_TILES/2, GameSettings.MAP_SIZE_IN_TILES/2);
        int yPosition = Random.Range(-GameSettings.MAP_SIZE_IN_TILES/2, GameSettings.MAP_SIZE_IN_TILES/2);
        Instantiate(EnemyPrefab, new Vector3(xPosition+.5f,yPosition+.5f,0), Quaternion.identity, null);
    }

}
