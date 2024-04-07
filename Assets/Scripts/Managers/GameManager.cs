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
    public Tilemap collidersLayer;

    private List<Perk> commonPerkPool;
    private List<Perk> uncommonPerkPool;
    private List<Perk> mythicPerkPool;
    private List<Perk> exoticPerkPool;
    private List<Perk> corruptedPerkPool;

    // Start is called before the first frame update
    void Start()
    {
        Player = PlayerGameObject.GetComponent<Player>();
        GameBoard = new GameBoard(tilemap, collidersLayer);
        SetPlayerStartingPos();
        HomeMadePathfinder.Init(GameBoard);
        UIManager.Instance.Init();

        commonPerkPool = new List<Perk>() { new SmallSwordPerk(), new GlassEyePerk(), new SteakPerk(), new JudoBeltPerk(), new WoodenShieldPerk(), new SilexPerk() };
        uncommonPerkPool = new List<Perk>() { new FireStonePerk(), new EnergyDrinkPerk(), new BoxingGlovePerk(), new ForbiddenArtsPerk(), new ThunderFeetPerk() };
        mythicPerkPool = new List<Perk>() { };
        exoticPerkPool = new List<Perk>() { };
        corruptedPerkPool = new List<Perk>() { new RiggedDicePerk() };
    }

    public void SetPlayerStartingPos()
    {
        var playerTile = GameBoard.Get(PlayerGameObject.transform.position);
        playerTile.entity = PlayerGameObject;
        Player.CurrentTile = playerTile;
    }

    public void ChooseNewPerks()
    {
        float chanceToPickCommonPerk = 1f;

        int currentPlayerLevel = Instance.Player.level - 1;

        //Proba de roll un perk uncommon : 10% x niveau actuel du joueur (maximum 60%)
        float chanceToPickUncommonPerk = Mathf.Clamp(0.1f * currentPlayerLevel, 0f, 0.6f);
        //Proba de roll un perk mythique : 5% x niveau actuel du joueur (maximum 30%)
        float chanceToPickMythicPerk = Mathf.Clamp(0.05f * currentPlayerLevel, 0f, 0.3f);
        //Proba de roll un perk exotique : 3% x niveau actuel du joueur (maximum 20%)
        float chanceToPickExoticPerk = Mathf.Clamp(0.03f * currentPlayerLevel, 0f, 0.2f);
        //Proba de roll un perk corrompu : 1% x niveau actuel du joueur (maximum 10%)
        float chanceToPickCorruptedPerk = Mathf.Clamp(0.01f * currentPlayerLevel, 0f, 0.1f);

        List<Perk> pickedPerks = new List<Perk>();
        while(pickedPerks.Count < 3)
        {
            //On tire au hasard la pool qu'on va prendre pour ce perk
            List<Perk> pool = commonPerkPool;
            int poolPicked = RNG.Outcome(chanceToPickCommonPerk, chanceToPickUncommonPerk, chanceToPickMythicPerk, chanceToPickExoticPerk, chanceToPickCorruptedPerk);
            if(poolPicked == 0) pool = commonPerkPool;
            if(poolPicked == 1) pool = uncommonPerkPool;
            if(poolPicked == 2) pool = mythicPerkPool;
            if(poolPicked == 3) pool = exoticPerkPool;
            if(poolPicked == 4) pool = corruptedPerkPool;

            //Si la pool est vide on fallback à la pool de perks communs
            if (pool.Count == 0) pool = commonPerkPool;

            Perk picked;
            int tries = 0;
            do
            {
                //Si on arrive pas trois fois de suite à tirer au sort un perk qu'on a pas déjà eu : on fallback à la pool de perks communs
                if(tries > 3) pool = commonPerkPool;
                tries++;
                int itemIndex = Random.Range(0, pool.Count);
                picked = pool[itemIndex];
            } while (pickedPerks.Contains(picked));
            pickedPerks.Add(picked);
        }
        UIManager.Instance.OpenPerksMenu(pickedPerks);
    }

}
