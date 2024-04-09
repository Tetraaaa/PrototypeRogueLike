using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaverPerk : Perk
{
    private float collateralDamage = 0.2f;
    public CleaverPerk()
    {
        this.rarity = PerkRarity.Mythic;
        image = Resources.Load<Sprite>("Perks/cleaver");
    }

    public override void OnBuy(Player owner)
    {
        owner.OnHit += OnHit;
        this.owner = owner;
    }

    public override string Description
    {
        get
        {
            return $"Couperet\n\nVos attaques frappent également les ennemis à côté pour <color=green>{collateralDamage*100}%</color> de vos dégâts.";
        }
    }

    public void OnHit(Enemy enemy, ProjectileDirection hitDirection)
    {
        //On retire temporairement ce perk pendant le tour, pour éviter qu'il se redéclenche sur les ennemis qu'il permet de frapper
        owner.OnHit -= OnHit;
        GameTile tileHit = enemy.CurrentTile;
        List<GameTile> nextToTiles = GameManager.Instance.GameBoard.GetNextTo(tileHit, hitDirection);
        foreach (var tile in nextToTiles)
        {
            if(tile.entity != null && tile.entity.GetComponent<Enemy>() != null)
            {
                GameManager.Instance.Player.Hit(tile.entity, tile, hitDirection, collateralDamage);
            }
        }
        owner.OnHit += OnHit;
    }
}
