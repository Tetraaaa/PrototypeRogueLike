using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    private bool willAttackPlayerNextTurn = false;
    public override void Start()
    {
        base.Start();
        attack = 3;
    }

    public override void PlayTurn()
    {
        OnTurnStart?.Invoke();
        if(willAttackPlayerNextTurn)
        {
            HitPlayerIfStillThere();
            willAttackPlayerNextTurn = false;
            animator.SetBool("WillAttackPlayerNextTurn", false);
        }
        else if (playerPosition != null)
        {
            willAttackPlayerNextTurn = true;
            animator.SetBool("WillAttackPlayerNextTurn", true);
            SoundManager.Instance.SlimeWindUp();
        }
        else
        {
            CheckIfPlayerIsInContact();
            if (playerPosition == null) WalkTowardsPlayer();
        }
    }
}
