using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : Enemy
{
    public override void Start()
    {
        base.Start();
        attack = 3;
    }

    public override void PlayTurn()
    {
        OnTurnStart?.Invoke();
        if (playerPosition != null)
        {
            HitPlayerIfStillThere();
        }
        else
        {
            CheckIfPlayerIsInContact();
            if (playerPosition == null) WalkTowardsPlayer();
        }
    }
}
