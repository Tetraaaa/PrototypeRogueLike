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
    public GameObject player;
    public GameBoard GameBoard;
    public Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        GameBoard = new GameBoard(tilemap);
        SetPlayerStartingPos();
        PathFinder.Init(GameBoard.board);

        //var path = PathFinder.FindPath(GameBoard.Get(34, -22), GameBoard.Get(29,-28));
        //Debug.Log(path);
        //path.ForEach(p => Debug.Log(p.worldPos));

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetPlayerStartingPos()
    {
        var playerTile = GameBoard.Get(player.transform.position);
        playerTile.entity = player;
        player.GetComponent<Player>().CurrentTile = playerTile;
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
