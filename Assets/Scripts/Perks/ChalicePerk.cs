using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChalicePerk : Perk
{
    private float healAmount = 0.5f;
    public ChalicePerk()
    {
        rarity = PerkRarity.Exotic;
        image = Resources.Load<Sprite>("Perks/chalice");
        isUnique = true;
    }

    public override void OnBuy(Player owner)
    {
        this.owner = owner;
        owner.OnDeath += OnDeath;
    }


    public override string Description
    {
        get
        {
            return $"Chalice\n\nLorsque vous mourrez, revenez à la vie avec <color=green>{healAmount * 100}</color>% de vos PV max (usage unique).";
        }
    }

    public void OnDeath()
    {
        owner.currentHp = 0; //Si les PV étaient passés dans le négatif on sauve quand même le joueur
        owner.Heal(Mathf.FloorToInt(owner.maxHP * healAmount));
        owner.OnDeath -= OnDeath;
        owner.perks.Remove(this);
    }
}
