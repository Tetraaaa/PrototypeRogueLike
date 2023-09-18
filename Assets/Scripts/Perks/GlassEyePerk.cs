using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassEyePerk : Perk
{
    private float bonusCritChance = 0.1f;
    public GlassEyePerk(Player owner)
    {
        this.owner = owner;
        owner.critChance += bonusCritChance;
        this.rarity = PerkRarity.Common;
    }

    public string Description
    {
        get
        {
            return $"Augmente les chances de coup critique de  {bonusCritChance * 100}%";
        }
    }
}
