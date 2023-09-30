using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlassEyePerk : Perk
{
    private float bonusCritChance = 0.1f;
    public GlassEyePerk()
    {
        this.rarity = PerkRarity.Common;
        image = Resources.Load<Sprite>("Perks/glass_eye");
    }

    public override void OnBuy(Player owner)
    {
        owner.critChance += bonusCritChance;
    }

    public override string Description
    {
        get
        {
            return $"Oeil de verre\n\nAugmente les chances de coup critique de <color=red>{bonusCritChance * 100}</color>%";
        }
    }
}
