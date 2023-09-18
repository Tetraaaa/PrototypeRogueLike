using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : Enemy
{
    public ProjectileDirection ProjectileDirection;
    public int DistanceToTravel = 1;

    public override void PlayTurn()
    {
        if (DistanceToTravel <= 0) Remove();
        GameTile targetTile = GameManager.Instance.GameBoard.GetAdjacent(CurrentTile, ProjectileDirection);
        if(targetTile != null)
        {
            if (!targetTile.IsWalkable()) Remove();
            movePoint.position = targetTile.worldPos;
            GameManager.Instance.GameBoard.MoveEntity(CurrentTile, targetTile);
            CurrentTile = targetTile;
        }

        DistanceToTravel--;
    }

    public void Remove()
    {
        CurrentTile.entity = null;
        Destroy(movePoint.gameObject);
        WaveManager.Instance.RemoveEnemy(gameObject);
    }

}

public enum ProjectileDirection
{
    Up,
    Left,
    Down,
    Right
}
