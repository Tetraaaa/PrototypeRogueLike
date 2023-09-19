using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameManager : Singleton<GameManager>
{
    private AudioSource audioSource;
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

        Player.perks.Add(new FireFistsPerk(Player));
        Player.perks.Add(new ThunderFeetPerk(Player));

        UIManager.Instance.Init();
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

    public void ChooseNewPerks()
    {

    }

}
