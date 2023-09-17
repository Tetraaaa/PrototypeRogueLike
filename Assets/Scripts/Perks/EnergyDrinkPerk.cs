using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyDrinkPerk : Perk
{
    private float healAmount = 0.25f;
    public EnergyDrinkPerk(Player owner)
    {
        rarity = PerkRarity.Uncommon;
        this.owner = owner;
        owner.OnLowHealth += OnLowHealth;
    }

    public string Description
    {
        get
        {
            return $"Lorsque vous tombez sous 20% de vos PV max, régénère {healAmount*100}% de vos PV max (usage unique).";
        }
    }

    public void OnLowHealth()
    {
        owner.Heal(Mathf.FloorToInt(owner.maxHP * healAmount));
        owner.perks.Remove(this);
    }
}
