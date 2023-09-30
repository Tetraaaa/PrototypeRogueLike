using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFistsPerk:Perk
{
    private int burningLevel = 1;
    public FireFistsPerk()
    {
        this.rarity = PerkRarity.Uncommon;
    }

    public override void OnBuy(Player owner)
    {
        owner.OnHit += OnHit;
    }

    public override string Description
    {
        get
        {
            return $"Vos attaques appliquent une brûlure de {burningLevel} point de dégâts.";
        }
    }

    public void OnHit(Enemy enemy)
    {
        enemy.debuffs.Add(new Burning(enemy));
    }
}

