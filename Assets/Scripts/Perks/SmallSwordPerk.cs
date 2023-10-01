using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SmallSwordPerk : Perk
{
    private float attackMultiplier = 0.2f;
    public SmallSwordPerk()
    {
        rarity = PerkRarity.Common;
        image = Resources.Load<Sprite>("Perks/small_sword");
    }

    public override void OnBuy(Player owner)
    {
        owner.attackMultiplier += 0.2f;
    }

    public override string Description
    {
        get
        {
            return $"Petite Épée \n\nAugmente les dégâts des attaques de <color=green>{attackMultiplier * 100}</color>%";
        }
    }

}
