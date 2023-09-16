using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThunderFeetPerk:Perk
{
    private int damage = 2;
    private Player owner;

    public ThunderFeetPerk(Player owner)
    {
        this.owner = owner;
        this.owner.OnMove += OnMove;
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
