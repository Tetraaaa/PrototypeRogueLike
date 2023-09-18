using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenShieldPerk : Perk
{
    private float increasedBlockChance = 0.2f;
    public WoodenShieldPerk(Player owner)
    {
        this.owner = owner;
        this.rarity = PerkRarity.Common;
        this.owner.parryChance += increasedBlockChance;
    }

    public string Description
    {
        get
        {
            return $"Augmente les chances de parer (après une esquive) de {increasedBlockChance * 100f}%";
        }
    }
}
