using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallSwordPerk : Perk
{
    private float attackMultiplier = 0.2f;
    public SmallSwordPerk(Player owner)
    {
        this.owner = owner;
        owner.attackMultiplier += 0.2f;
        rarity = PerkRarity.Common;
    }

    public string Description
    {
        get
        {
            return $"Augmente les dégâts des attaques de {attackMultiplier*100}%";
        }
    }


}
