using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public GameObject EnemyPrefab;
    private AudioSource audioSource;
    public FloatingDamage FloatingDamagePrefab;
    private int currentTurn = 0;
    private int currentWave = 1;
    private int enemiesThisRound = 15;
    public AudioClip hitSound;
    public bool isEnemyTurn = false;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartNextTurnAndPerformSideEffects()
    {
        currentTurn++;

        PlayEnemiesTurn();

        bool shouldSpawnEnemiesThisTurn = currentTurn % GameSettings.DEFAULT_DELAY_BETWEEN_ENEMY_SPAWNS_IN_TURNS == 0;
        if (shouldSpawnEnemiesThisTurn)
        {
            SpawnSomeEnemies();
        }

        isEnemyTurn = false;

    }

    public void SpawnSomeEnemies()
    {
        if (enemiesThisRound <= 0) return;
        int xPosition = Random.Range(-GameSettings.MAP_SIZE_IN_TILES/2, GameSettings.MAP_SIZE_IN_TILES/2);
        int yPosition = Random.Range(-GameSettings.MAP_SIZE_IN_TILES/2, GameSettings.MAP_SIZE_IN_TILES/2);
        Instantiate(EnemyPrefab, new Vector3(xPosition+.5f,yPosition+.5f,0), Quaternion.identity, null);
        enemiesThisRound--;
    }

    public void PlayEnemiesTurn()
    {
        foreach (var enemy in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            enemy.GetComponent<Enemy>().PlayTurn();
        }
    }

    public void PlayHitSound()
    {
        audioSource.PlayOneShot(hitSound);
    }

    public void ShowFloatingDamage(Vector3 position, int damage, Color color)
    {
        FloatingDamage floatingDamage = Instantiate(FloatingDamagePrefab, position, Quaternion.identity, null);
        floatingDamage.SetText(damage.ToString(), color);
        floatingDamage.gameObject.SetActive(true);
    }

}
