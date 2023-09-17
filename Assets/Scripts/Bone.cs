using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
{
    public Transform movePoint;
    public GameTile CurrentTile;
    public ProjectileDirection ProjectileDirection;
    public int DistanceToTravel = 1;
    private float moveSpeed = 7f;

    public void Start()
    {
        movePoint.parent = null;
        WaveManager.Instance.OnPlayTurn += PlayTurn;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, movePoint.position, moveSpeed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        WaveManager.Instance.OnPlayTurn -= PlayTurn;
    }

    public void PlayTurn()
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
        Destroy(gameObject);
    }

}

public enum ProjectileDirection
{
    Up,
    Left,
    Down,
    Right
}
