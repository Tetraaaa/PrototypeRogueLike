using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilexPerk : Perk
{
    private float bonusCritDamage = 0.1f;
    public SilexPerk(Player owner)
    {
        this.rarity = PerkRarity.Common;
        this.owner = owner;
        this.owner.critDamageMultiplier += bonusCritDamage;
    }

    public override string Description
    {
        get
        {
            return $"Augmente les dégâts infligés par les coups critiques de {bonusCritDamage * 100}%";
        }
    }
}
