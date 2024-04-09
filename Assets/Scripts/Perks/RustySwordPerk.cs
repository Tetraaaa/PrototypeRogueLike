using Unity.VisualScripting;
using UnityEngine;

public class RustySwordPerk : Perk
{
    float critChanceMultiplier = 0.1f;
    float critDamageMultiplier = 10f;
    public RustySwordPerk()
    {
        this.rarity = PerkRarity.Corrupted;
        image = Resources.Load<Sprite>("Perks/rusty_sword");
        isUnique = true;
    }

    public override void OnBuy(Player owner)
    {
        owner.critChanceMultiplier*= critChanceMultiplier;
        owner.critDamageMultiplier*= critDamageMultiplier;
    }

    public override string Description
    {
        get
        {
            return $"Épée rouillée\n\nChances de coup critique divisées par <color=green>10</color>. Dégâts critiques multipliés par <color=green>10</color>";
        }
    }
}
