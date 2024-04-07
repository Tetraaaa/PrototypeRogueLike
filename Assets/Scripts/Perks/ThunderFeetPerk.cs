using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderFeetPerk:Perk
{
    private int damage = 2;

    public ThunderFeetPerk()
    {
        this.rarity = PerkRarity.Uncommon;
        image = Resources.Load<Sprite>("Perks/thunder_feet");
    }

    public override void OnBuy(Player owner)
    {
        owner.OnMove += OnMove;
    }

    public override string Description
    {
        get
        {
            return $"Bottes de foudre\n\nLorsque vous vous déplacez, le sol autour de vous s'électrise, infligeant <color=green>{damage}</color> points de dégâts aux cases adjacentes";
        }
    }

    public void OnMove(GameTile targetTile)
    {
        List<GameTile> neighbors = GameManager.Instance.GameBoard.GetNeighbors(targetTile);
        foreach (GameTile neighbor in neighbors)
        {
            AnimationManager.Instance.ThunderFeet(neighbor.worldPos);
            if (neighbor.entity) neighbor.entity.GetComponent<Enemy>().TakeDamage(damage, GameManager.Instance.Player.gameObject);
        }
    }
}
