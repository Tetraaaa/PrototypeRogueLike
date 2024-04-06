using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteakPerk : Perk
{
    private int maxHPGain = 20;
    public SteakPerk()
    {
        this.rarity = PerkRarity.Common;
        image = Resources.Load<Sprite>("Perks/steak");
    }

    public override void OnBuy(Player owner)
    {
        owner.maxHP += maxHPGain;
        owner.currentHp += maxHPGain;
    }

    public override string Description
    {
        get
        {
            return $"Steak saignant\n\nAugmente vos PV max de <color=green>{maxHPGain}</color>.";
        }
    }

}
