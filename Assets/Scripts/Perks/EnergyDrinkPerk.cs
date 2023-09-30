using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnergyDrinkPerk : Perk
{
    private float healAmount = 0.25f;
    public EnergyDrinkPerk()
    {
        rarity = PerkRarity.Uncommon;
        image = Resources.Load<Sprite>("Perks/energy_drink");
    }

    public override void OnBuy(Player owner)
    {
        owner.OnLowHealth += OnLowHealth;
    }


    public override string Description
    {
        get
        {
            return $"Boisson Énergisante\n\nLorsque vous tombez sous 20% de vos PV max, régénère <color=green>{healAmount*100}</color>% de vos PV max (usage unique).";
        }
    }

    public void OnLowHealth()
    {
        owner.Heal(Mathf.FloorToInt(owner.maxHP * healAmount));
        owner.perks.Remove(this);
    }
}
