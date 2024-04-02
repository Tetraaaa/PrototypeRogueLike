using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingSlime : Enemy
{
    private bool willAttackPlayerNextTurn = false;

    private bool WillExplodeNextTurn = false;
    private Animator animator;

    public KingSlime()
    {
        attack = 15;
        maxHP = 100;
    }
    public override void Start()
    {
        base.Start();
        animator = GetComponent<Animator>();
    }

    public override void PlayTurn()
    {
        OnTurnStart?.Invoke();
        if (WillExplodeNextTurn)
        {
            ExplosionAttack();
            return;
        }

        if (willAttackPlayerNextTurn)
        {
            HitPlayerIfStillThere();
            willAttackPlayerNextTurn = false;
        }
        else if (playerPosition != null)
        {
            //Le joueur est au cac : choisir quel sort lancer
            ExplosionAttack();
            willAttackPlayerNextTurn = true;
        }
        else
        {
            CheckIfPlayerIsInContact();
            if (playerPosition == null) WalkTowardsPlayer();
        }
    }

    public void ExplosionAttack()
    {
        if(WillExplodeNextTurn)
        {
            animator.SetBool("WillCastExplosionNextTurn", false);
            WillExplodeNextTurn = false;
        }
        else
        {
            WillExplodeNextTurn = true;
            animator.SetBool("WillCastExplosionNextTurn", true);
        }
    }
}
