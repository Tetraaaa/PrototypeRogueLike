using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RiggedDicePerk : Perk
{
    float multiplierBeforeHit = 1f;
    public RiggedDicePerk()
    {
        this.rarity = PerkRarity.Corrupted;
        image = Resources.Load<Sprite>("Perks/rigged_dice");
    }

    public override void OnBuy(Player owner)
    {
        owner.OnBeforeHit += OnBeforeHit;
        owner.OnAfterHit += OnAfterHit;
    }

    public override string Description
    {
        get
        {
            return $"Dé pipé\n\nVos attaques infligent désormais entre 50% et 200% de vos dégâts.";
        }
    }

    public void OnBeforeHit(Enemy enemy, ProjectileDirection hitDirection)
    {
        multiplierBeforeHit = GameManager.Instance.Player.attackMultiplier;
        GameManager.Instance.Player.attackMultiplier *= RNG.Range(0.5f, 2f);
    }

    public void OnAfterHit()
    {
        GameManager.Instance.Player.attackMultiplier = multiplierBeforeHit;
    }
}
