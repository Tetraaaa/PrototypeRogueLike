using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoxingGlovePerk : Perk
{
    int attackCounter = 0;
    public BoxingGlovePerk()
    {
        this.rarity = PerkRarity.Uncommon;
        image = Resources.Load<Sprite>("Perks/boxing_glove");
    }
    public override void OnBuy(Player owner)
    {
        owner.OnHit += OnHit;
    }
    public override string Description
    {
        get
        {
            return $"Gant de boxe\n\nToutes les trois attaques, votre prochaine attaque repoussera l’ennemi de 1 case.";
        }
    }
    public void OnHit(Enemy enemy, ProjectileDirection hitDirection)
    {
        attackCounter++;

        if (attackCounter == 3) {
            GameTile targetTile = GameManager.Instance.GameBoard.GetAdjacent(enemy.CurrentTile, hitDirection);
            if (targetTile != null && targetTile.IsWalkable)
            {
                enemy.movePoint.position = targetTile.worldPos;
                GameManager.Instance.GameBoard.MoveEntity(enemy.CurrentTile, targetTile);
                enemy.CurrentTile = GameManager.Instance.GameBoard.Get(enemy.movePoint.position);
            }

            attackCounter = 0;
        }

    }
}
