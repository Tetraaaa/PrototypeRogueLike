using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderFeetPerk:Perk
{
    private int damage = 2;

    public ThunderFeetPerk(Player owner)
    {
        this.rarity = PerkRarity.Uncommon;
        this.owner = owner;
        this.owner.OnMove += OnMove;
    }

    public override string Description
    {
        get
        {
            return $"Lorsque vous vous d�placez, le sol autour de vous s'�lectrise, infligeant {damage} points de d�g�ts aux cases adjacentes";
        }
    }

    public void OnMove(GameTile targetTile)
    {
        List<GameTile> neighbors = GameManager.Instance.GameBoard.GetNeighbors(targetTile);
        foreach (GameTile neighbor in neighbors)
        {
            AnimationManager.Instance.ThunderFeet(neighbor.worldPos);
            if (neighbor.entity) neighbor.entity.GetComponent<Enemy>().TakeDamage(damage, GameManager.Instance.Player);
        }
    }
}
