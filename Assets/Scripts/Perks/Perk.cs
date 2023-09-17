using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Perk
{
    protected PerkRarity rarity;
    protected Player owner;
}

public enum PerkRarity
{
    Common,
    Uncommon,
    Mythic,
    Exotic,
    Corrupted
}