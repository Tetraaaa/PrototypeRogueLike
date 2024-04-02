using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : Enemy
{
    //Ennemi au comportement fuyard : Si le joueur arrive à son Cac, il va choisir de fuir en priorité. Sinon, si le joueur est aligné avec lui, il lui lance un os (projectile qui parcourt une distance de scanRange max dans une direction fixe)
    private int scanRange = 5;
    protected GameObject Bone;
    private int BoneCooldownInTurns = 3;
    private int CurrentBoneCooldown = 0;

    public Skeleton()
    {
        attack = 6;
        maxHP = 20;
    }

    public override void Start()
    {
        base.Start();
    }

    public override void PlayTurn()
    {
        if(CurrentBoneCooldown != 0) CurrentBoneCooldown--;
        OnTurnStart?.Invoke();
        if (playerPosition != null && CurrentBoneCooldown == 0)
        {
            ThrowBone();
        }
        else
        {
            CheckIfPlayerIsAligned();
            if (playerPosition == null) WalkTowardsPlayer();
        }
    }

    public void CheckIfPlayerIsAligned()
    {
        GameTile playerTile = GameManager.Instance.Player.CurrentTile;
        if (playerTile.x != CurrentTile.x && playerTile.y != CurrentTile.y) return; //Le joueur n'est ni aligné sur l'axe x ni sur l'axe y
        if(playerTile.x == CurrentTile.x && Mathf.Abs(playerTile.y - CurrentTile.y) <= scanRange)
        {
            playerPosition = GameManager.Instance.Player.CurrentTile;
        }
        else if(playerTile.y == CurrentTile.y && Mathf.Abs(playerTile.x - CurrentTile.x) <= scanRange)
        {
            playerPosition = GameManager.Instance.Player.CurrentTile;
        }
    }

    public void ThrowBone()
    {
        ProjectileDirection boneDirection;
        //Ici on part du principe que la position du joueur (celle qu'on a) est forcément alignée avec nous, donc on check juste les coordonnées en x et y pour savoir dans quelle direction lancer l'os
        if(playerPosition.x == CurrentTile.x)
        {
            if(playerPosition.y < CurrentTile.y)
            {
                boneDirection = ProjectileDirection.Down;
            }
            else
            {
                boneDirection = ProjectileDirection.Up;
            }
        }
        else if(playerPosition.y == CurrentTile.y)
        {
            if (playerPosition.x < CurrentTile.x)
            {
                boneDirection = ProjectileDirection.Left;
            }
            else
            {
                boneDirection = ProjectileDirection.Right;
            }
        }
        else
        {
            throw new System.Exception("wow il pue la merde cet algo wtf x)");
        }

        playerPosition = null;
        GameTile spawnTile = GameManager.Instance.GameBoard.GetAdjacent(CurrentTile, boneDirection);

        if (spawnTile == null ||  !spawnTile.IsWalkable()) return;

        GameObject boneGameObject = Instantiate(Bone, spawnTile.worldPos, Quaternion.identity, null);
        Bone bone = boneGameObject.GetComponent<Bone>();
        bone.ProjectileDirection = boneDirection;
        bone.DistanceToTravel = scanRange;
        bone.CurrentTile = spawnTile;
        bone.thrower = this;
        spawnTile.entity = boneGameObject;
        WaveManager.Instance.AddEnemy(boneGameObject);

        CurrentBoneCooldown = BoneCooldownInTurns;
    }
}
