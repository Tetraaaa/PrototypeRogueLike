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
        UIManager.Instance.Init();
    }

    public void SetPlayerStartingPos()
    {
        var playerTile = GameBoard.Get(PlayerGameObject.transform.position);
        playerTile.entity = PlayerGameObject;
        Player.CurrentTile = playerTile;
    }

    public void ChooseNewPerks()
    {
        List<Perk> defaultPerkPool = new List<Perk>() { new SmallSwordPerk(), new GlassEyePerk(), new SteakPerk(), new JudoBeltPerk(), new WoodenShieldPerk(), new SilexPerk() };
        List<Perk> pickedPerks = new List<Perk>();
        while(pickedPerks.Count < 3)
        {
            Perk picked = null;
            do
            {
                int itemIndex = Random.Range(0, defaultPerkPool.Count);
                picked = defaultPerkPool[itemIndex];
            } while (pickedPerks.Contains(picked));
            pickedPerks.Add(picked);
        }
        UIManager.Instance.OpenPerksMenu(pickedPerks);
    }

}
