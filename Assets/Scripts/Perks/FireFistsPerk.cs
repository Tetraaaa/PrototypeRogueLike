using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFistsPerk:Perk
{
    Player target;
    public FireFistsPerk(Player target)
    {
        this.target = target;
        this.target.OnHit += OnHit;
    }

    public void OnHit(Enemy enemy)
    {
        enemy.debuffs.Add(new Burning(enemy));
    }
}

