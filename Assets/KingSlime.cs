using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using static KingSlime;

public class KingSlime : Enemy
{
    private bool willAttackPlayerNextTurn = false;

    private bool WillExplodeNextTurn = false;
    private bool WillSummonNextTurn = false;
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

        if (willAttackPlayerNextTurn)
        {
            HitPlayerIfStillThere();
            willAttackPlayerNextTurn = false;
        }
        else if (playerPosition != null)
        {
            //Le joueur est au cac : choisir quel sort lancer
            switch (Random.Range(0, 2))
            {
                case 0:
                    ExplosionAttack();
                    break;
                case 1:
                    SummonAttack();
                    break;
                case 2:
                    ExplosionAttack();
                    break;
                default:
                    break;
            }
            willAttackPlayerNextTurn = true;
        }
        else
        {
            CheckIfPlayerIsInContact();
            if (playerPosition == null) WalkTowardsPlayer();
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
            List<GameTile> adjacentTiles = GameManager.Instance.GameBoard.GetNeighbors(CurrentTile);
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
                WaveManager.Instance.Summon("slime", tile);
            }
        }
        else
        {
            WillSummonNextTurn = true;
            animator.SetBool("WillSummonNextTurn", true);
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
}
