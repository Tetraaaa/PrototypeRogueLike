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
    public Player player;
    public GameBoard GameBoard;
    public Tilemap tilemap;

    // Start is called before the first frame update
    void Start()
    {
        GameBoard = new GameBoard(tilemap);
        PathFinder.Init(GameBoard.board);

        //var path = PathFinder.FindPath(GameBoard.Get(7, 3), GameBoard.Get(11, 3));
        //path.ForEach(t => Debug.Log(t.worldPos));

        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
