using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireStonePerk:Perk
{
    private int burningLevel = 1;
    public FireStonePerk()
    {
        this.rarity = PerkRarity.Uncommon;
        image = Resources.Load<Sprite>("Perks/fire_stone");
    }

    public override void OnBuy(Player owner)
    {
        owner.OnHit += OnHit;
    }

    public override string Description
    {
        get
        {
            return $"Pierre de feu\n\nVos attaques appliquent une br�lure de <color=green>{burningLevel}</color> points de d�g�ts.";
        }
    }

    public void OnHit(Enemy enemy)
    {
        enemy.debuffs.Add(new Burning(enemy));
    }
}

