using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodenShieldPerk : Perk
{
    private float increasedBlockChance = 0.2f;
    public WoodenShieldPerk()
    {
        this.rarity = PerkRarity.Common;
        image = Resources.Load<Sprite>("Perks/wooden_shield");
    }

    public override void OnBuy(Player owner)
    {
        owner.parryChance += increasedBlockChance;
    }

    public override string Description
    {
        get
        {
            return $"Bouclier en bois\n\nAugmente les chances de parer (après une esquive) de <color=green>{increasedBlockChance * 100f}</color>%";

        }
    }
}
