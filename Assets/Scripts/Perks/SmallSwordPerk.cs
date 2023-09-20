using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SmallSwordPerk : Perk
{
    private float attackMultiplier = 0.2f;
    public SmallSwordPerk(Player owner)
    {
        this.owner = owner;
        owner.attackMultiplier += 0.2f;
        rarity = PerkRarity.Common;
        image = Resources.Load<Sprite>("Perks/small_sword");
    }

    public override string Description
    {
        get
        {
            return $"Augmente les dégâts des attaques de <color=ff0000>{attackMultiplier * 100}</color>%";
        }
    }

}
