using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    private AudioSource audioSource;
    public FloatingDamage FloatingDamagePrefab;
    public AudioClip hitSound;
    public GameObject ThunderFeetAnimation;
    public GameObject PlayerGameObject;
    [HideInInspector]
    public Player Player;
    public GameBoard GameBoard;
    public Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        Player = PlayerGameObject.GetComponent<Player>();
        GameBoard = new GameBoard(tilemap);
        SetPlayerStartingPos();
        HomeMadePathfinder.Init(GameBoard);

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerStartingPos()
    {
        var playerTile = GameBoard.Get(PlayerGameObject.transform.position);
        playerTile.entity = PlayerGameObject;
        Player.CurrentTile = playerTile;
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

    public void ChooseNewPerks()
    {

    }

}
