using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Perk
{
    protected PerkRarity rarity;
    protected Player owner;
    public Sprite image;
    public bool isUnique;

    public abstract void OnBuy(Player owner);

    public PerkRarity Rarity { get { return rarity; } }

    public abstract string Description
    {
        get;
    }

    public override string ToString()
    {
        return GetType().Name + " (" + rarity + ")";
    }

}

public enum PerkRarity
{
    Common,
    Uncommon,
    Mythic,
    Exotic,
    Corrupted
}