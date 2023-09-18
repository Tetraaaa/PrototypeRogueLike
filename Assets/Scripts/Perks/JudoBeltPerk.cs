using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudoBeltPerk : Perk
{
    private float dodgeChanceGain = 0.05f;
    public JudoBeltPerk(Player owner)
    {
        this.owner = owner;
        this.rarity = PerkRarity.Common;
        this.owner.dodgeChance += dodgeChanceGain;
    }

    public string Description
    {
        get
        {
            return $"Augmente les chances d'esquive de {dodgeChanceGain*100f}%";
        }
    }
}
