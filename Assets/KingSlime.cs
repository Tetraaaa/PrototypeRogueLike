using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Tilemaps;
using static KingSlime;

public class KingSlime : Enemy
{
    private bool willAttackPlayerNextTurn = false;

    private bool WillExplodeNextTurn = false;
    private bool WillSummonNextTurn = false;
    private bool WillHitNextTurn = false;

    private int slimesSummonedAndAlive = 0;

    private Animator animator;
    public GameObject CorruptedGroundPrefab;
    public List<CorruptedTile> CorruptedTiles = new List<CorruptedTile>();


    public class CorruptedTile
    {
        public GameTile tile;
        public int durationRemainingInTurns;
        public GameObject effectPrefab;
        public CorruptedTile(GameTile tile, int durationRemainingInTurns, GameObject effectPrefab)
        {
            this.tile = tile;
            this.durationRemainingInTurns = durationRemainingInTurns;
            this.effectPrefab = effectPrefab;
        }
    }

    public KingSlime()
    {
        attack = 5;
        maxHP = 100;
    }
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public override void PlayTurn()
    {
        OnTurnStart?.Invoke();
        CheckCorruptedTiles();

        if (WillExplodeNextTurn)
        {
            ExplosionAttack();
            return;
        }

        if (WillSummonNextTurn)
        {
            SummonAttack();
            return;
        }

        if (WillHitNextTurn)
        {
            HitAttack();
            return;
        }

        double distanceToPlayer = GameManager.Instance.GameBoard.GetDistance(CurrentTile, GameManager.Instance.Player.CurrentTile);
        bool areEmptyTilesAroundPlayer = GameManager.Instance.GameBoard.GetEmptyNeighbors(GameManager.Instance.Player.CurrentTile).Count > 0;

        if (distanceToPlayer > 4) {
            WalkTowardsPlayer();
        }
        else if (distanceToPlayer > 1)
        {
            //Le joueur est proche
            float chanceToWalkTowardsPlayer = 0.5f;
            float chanceToCastHitAttack = 0.25f;
            float chanceToCastSummon = 0.25f;

            if(slimesSummonedAndAlive >= 3) chanceToCastSummon = 0f;
            if(!areEmptyTilesAroundPlayer) chanceToCastHitAttack = 0f;

            switch (RNG.Outcome(chanceToCastHitAttack, chanceToCastSummon, chanceToWalkTowardsPlayer))
            {
                case 0:
                    HitAttack();
                    break;
                case 1:
                    SummonAttack();
                    break;
                case 2:
                    WalkTowardsPlayer();
                    break;
                default:
                    WalkTowardsPlayer();
                    break;
            }
        }
        else
        {
            //Le joueur est au cac : on lance un des trois sorts au hasard, mais avec + de chances de cast l'explosion

            float chanceToCastExplosion = 0.5f;
            float chanceToCastHitAttack = 0.35f;
            float chanceToCastSummon = 0.15f;

            if (!areEmptyTilesAroundPlayer) chanceToCastHitAttack = 0f;
            if (slimesSummonedAndAlive >= 3) chanceToCastSummon = 0f;

            switch (RNG.Outcome(chanceToCastExplosion, chanceToCastHitAttack, chanceToCastSummon))
            {
                case 0:
                    ExplosionAttack();
                    break;
                case 1:
                    HitAttack();
                    break;
                case 2:
                    SummonAttack();
                    break;
                default:
                    ExplosionAttack();
                    break;
            }
        }
    }

    /// <summary>
    /// Après une canalisation de 1 tour, explose en infligeant attaque*2 aux cases adjacentes. 
    /// Les cases touchées sont corrompues et infligent attaque*0.2 au joueur si il marche dessus, pendant 10 tours
    /// </summary>
    public void ExplosionAttack()
    {
        if(WillExplodeNextTurn)
        {
            animator.SetBool("WillCastExplosionNextTurn", false);
            List<GameTile> adjacentTiles = GameManager.Instance.GameBoard.GetNeighborsAndDiagonals(CurrentTile);
            if(adjacentTiles.Contains(GameManager.Instance.Player.CurrentTile))
            {
                GameManager.Instance.Player.TakeDamage(attack * 2, this);
            }

            foreach (var tile in adjacentTiles)
            {
                var effectPrefab = Instantiate(CorruptedGroundPrefab, tile.worldPos, Quaternion.identity, null);
                CorruptedTiles.Add(new CorruptedTile(tile, 10, effectPrefab));
                tile.OnWalkedOn += DamageEntityIfCorruptedTileIsWalkedOn;
            }

            WillExplodeNextTurn = false;
        }
        else
        {
            WillExplodeNextTurn = true;
            animator.SetBool("WillCastExplosionNextTurn", true);
        }
    }

    /// <summary>
    /// Après une canalisation de 1 tour, invoque deux slimes sur des cases aléatoires autour du joueur. 
    /// </summary>
    public void SummonAttack()
    {
        if (WillSummonNextTurn)
        {
            WillSummonNextTurn = false;
            animator.SetBool("WillSummonNextTurn", false);
            List<GameTile> adjacentTiles = GameManager.Instance.GameBoard.GetNeighborsAndDiagonals(GameManager.Instance.Player.CurrentTile);
            List<GameTile> pickedTiles = new List<GameTile>();
            while (adjacentTiles.Count > 0 && pickedTiles.Count < 2)
            {
                GameTile picked = adjacentTiles[Random.Range(0, adjacentTiles.Count)];
                if(picked.hasCollision || picked.entity != null)
                {
                    adjacentTiles.Remove(picked);
                }
                else
                {
                    pickedTiles.Add(picked);
                }
            }
            foreach (var tile in pickedTiles)
            {
                GameObject slimeSummoned = WaveManager.Instance.Summon("slime", tile);
                slimeSummoned.GetComponent<Enemy>().OnDeath += OnSummonedSlimeDeath;
                slimesSummonedAndAlive++;
            }
        }
        else
        {
            WillSummonNextTurn = true;
            animator.SetBool("WillSummonNextTurn", true);
        }

    }

    public void HitAttack()
    {
        if (WillHitNextTurn)
        {
            WillHitNextTurn = false;
            animator.SetBool("WillHitNextTurn", false);

            //On récupère les cases adjacentes au joueur qui sont libres
            List<GameTile> adjacentTilesAroundPlayer = GameManager.Instance.GameBoard.GetEmptyNeighbors(GameManager.Instance.Player.CurrentTile);

            if (adjacentTilesAroundPlayer.Count < 1) return;

            //On récupère le carré de 3x3 cases dont le joueur est au centre
            List<GameTile> tilesAroundPlayer = GameManager.Instance.GameBoard.GetNeighborsAndDiagonals(GameManager.Instance.Player.CurrentTile);
            tilesAroundPlayer.Add(GameManager.Instance.Player.CurrentTile);

            //On prend une case au hasard dans les cases libres adjacentes au joueur
            GameTile pickedTile = adjacentTilesAroundPlayer[Random.Range(0, adjacentTilesAroundPlayer.Count)];

            tilesAroundPlayer.Remove(pickedTile);

            foreach (GameTile tile in tilesAroundPlayer)
            {
                GameManager.Instance.GameBoard.tilemap.SetTileFlags(new Vector3Int(tile.tilesetX, tile.tilesetY), TileFlags.None);
                GameManager.Instance.GameBoard.tilemap.SetColor(new Vector3Int(tile.tilesetX, tile.tilesetY), Color.red);
            }

            while (adjacentTilesAroundPlayer.Count > 0 && pickedTile == null)
            {
                if (pickedTile.hasCollision || pickedTile.entity != null)
                {
                    adjacentTilesAroundPlayer.Remove(pickedTile);
                }
            }
        }
        else
        {
            WillHitNextTurn = true;
            animator.SetBool("WillHitNextTurn", true);
        }
    }

    public void CheckCorruptedTiles()
    {
        List<CorruptedTile> tilesToRemove = new List<CorruptedTile>();
        foreach (var corruptedTile in CorruptedTiles)
        {
            corruptedTile.durationRemainingInTurns -= 1;
            if (corruptedTile.durationRemainingInTurns <= 0)
            {
                Destroy(corruptedTile.effectPrefab);
                corruptedTile.tile.OnWalkedOn -= DamageEntityIfCorruptedTileIsWalkedOn;
                tilesToRemove.Add(corruptedTile);
            }
        }
        foreach (var tileToRemove in tilesToRemove)
        {
            CorruptedTiles.Remove(tileToRemove);
        }
    }

    public void DamageEntityIfCorruptedTileIsWalkedOn(GameObject entity)
    {
        if(entity.GetComponent<Player>() != null)
        {
            GameManager.Instance.Player.TakeDamage((int)(attack*0.2), this);
        }
        else if(entity.GetComponent<Enemy>() != null)
        {
            entity.GetComponent<Enemy>().TakeDamage((int)(attack * 0.2), gameObject);
        }

    }

    public void OnSummonedSlimeDeath()
    {
        slimesSummonedAndAlive -= 1;
    }
}
