using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteakPerk : Perk
{
    private int maxHPGain = 20;
    public SteakPerk(Player owner)
    {
        this.owner = owner;
        this.owner.maxHP += maxHPGain;
        this.rarity = PerkRarity.Common;
    }

    public string Description
    {
        get
        {
            return $"Augmente vos PV max de {maxHPGain}.";
        }
    }

}
