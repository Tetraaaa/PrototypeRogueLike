using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireFistsPerk:Perk
{
    private int burningLevel = 1;
    public FireFistsPerk(Player owner)
    {
        this.rarity = PerkRarity.Uncommon;
        this.owner = owner;
        this.owner.OnHit += OnHit;
    }

    public string Description
    {
        get
        {
            return $"Vos attaques appliquent une br�lure de {1} point de d�g�ts.";
        }
    }

    public void OnHit(Enemy enemy)
    {
        enemy.debuffs.Add(new Burning(enemy));
    }
}

