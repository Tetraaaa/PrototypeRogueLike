using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilexPerk : Perk
{
    private float bonusCritDamage = 0.1f;
    public SilexPerk()
    {
        this.rarity = PerkRarity.Common;
        image = Resources.Load<Sprite>("Perks/silex");
    }

    public override void OnBuy(Player owner)
    {
        owner.critDamageMultiplier += bonusCritDamage;
    }

    public override string Description
    {
        get
        {
            return $"Silex\n\nAugmente les dégâts infligés par les coups critiques de <color=green>{bonusCritDamage * 100}</color>%";
        }
    }
}
