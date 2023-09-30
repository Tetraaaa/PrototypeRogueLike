using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JudoBeltPerk : Perk
{
    private float dodgeChanceGain = 0.05f;
    public JudoBeltPerk()
    {
        this.rarity = PerkRarity.Common;
        image = Resources.Load<Sprite>("Perks/judo_belt");
    }

    public override void OnBuy(Player owner)
    {
        owner.dodgeChance += dodgeChanceGain;
    }

    public override string Description
    {
        get
        {
            return $"Ceinture blanche de judo\n\nAugmente les chances d'esquive de <color=green>{dodgeChanceGain*100f}</color>%";
        }
    }
}
