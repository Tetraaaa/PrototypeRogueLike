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

    public override string Description
    {
        get
        {
            return $"Augmente les chances de parer (après une esquive) de <color=red>{increasedBlockChance * 100f}</color>%";

        }
    }
}
