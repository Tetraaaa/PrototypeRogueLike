using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Perk
{
    protected PerkRarity rarity;
    protected Player owner;
    public Sprite image;

    public abstract string Description
    {
        get;
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