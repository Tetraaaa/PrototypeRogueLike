using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ForbiddenArtsPerk : Perk
{
    private int explosionDamage = 4;
    public ForbiddenArtsPerk()
    {
        this.rarity = PerkRarity.Uncommon;
        image = Resources.Load<Sprite>("Perks/forbidden_arts");
    }

    public override void OnBuy(Player owner)
    {
        owner.OnAfterHit += OnAfterHit;
    }

    public override string Description
    {
        get
        {
            return $"Arcanes interdits\n\nQuand un ennemi est tué, il explose en infligeant <color=green>{explosionDamage}</color> dégâts sur les cases adjacentes.";
        }
    }

    public void OnAfterHit(bool isEnemyDead, GameTile tileHit)
    {
        if(isEnemyDead)
        {
            bool damageDealt = false;
            List<GameTile> neighbors = GameManager.Instance.GameBoard.GetNeighbors(tileHit);
            foreach (GameTile neighbor in neighbors)
            {
                if (neighbor.entity && neighbor.entity.GetComponent<Enemy>()!=null)
                {
                    neighbor.entity.GetComponent<Enemy>().TakeDamage(explosionDamage, GameManager.Instance.Player.gameObject);
                    damageDealt = true;
                }
            }

            if (damageDealt)
            {
                SoundManager.Instance.ForbiddenArts();
                AnimationManager.Instance.ForbiddenArts(tileHit.worldPos);
            }
        }
    }
}
