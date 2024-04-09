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
            return $"�p�e rouill�e\n\nChances de coup critique divis�es par <color=green>10</color>. D�g�ts critiques multipli�s par <color=green>10</color>";
        }
    }
}
