using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Drawing;

public class GameBoard
{
    public Dictionary<Point, GameTile> board;
    public Tilemap tilemap;
    public Tilemap collidersLayer;

    public GameBoard(Tilemap tilemap, Tilemap collidersLayer)
    {
        this.tilemap = tilemap;
        this.collidersLayer = collidersLayer;
        board = new Dictionary<Point, GameTile>();
        collidersLayer.CompressBounds(); 

        //En x, on va vers la droite donc de -32 à 31
        for (int x = -32, i = 0; x < 32; x++, i++)
        {
            //En y, on va en bas donc de 31 à -32
            for (int y = 31, j = 0; y >= -32; y--, j--)
            {
                board.Add(new Point(i,j), new GameTile(i, j, x, y, collidersLayer.HasTile(new Vector3Int(x, y, 0))));
            }
        }
    }

    public void SetEntityOnTile(int x, int y, GameObject? entity)
    {
        var tile = Get(x, y);
        tile.entity = entity;
    }

    public GameTile Get(int x, int y)
    {
        GameTile tile;
        board.TryGetValue(new Point(x, y), out tile);
        return tile;
    }

    public GameTile Get(Vector3 pos)
    {
        return Get((int)pos.x, (int)pos.y);
    }

    public void MoveEntity(GameTile currentTile, GameTile newTile)
    {
        if (!currentTile.entity) return;
        newTile.entity = currentTile.entity;
        currentTile.entity = null;
        newTile.OnWalkedOn?.Invoke(newTile.entity);
    }

    public GameTile GetRandomEmptyCell()
    {
        GameTile tile;
        do
        {
            tile = board.ElementAt(UnityEngine.Random.Range(0, board.Count)).Value;
        } while (!tile.IsWalkable);

        return tile;
    }

    public List<GameTile> GetNeighbors(GameTile tile)
    {
        List<GameTile> neighbors = new List<GameTile>();

        neighbors.Add(GameManager.Instance.GameBoard.Get(tile.x - 1, tile.y));
        neighbors.Add(GameManager.Instance.GameBoard.Get(tile.x + 1, tile.y));
        neighbors.Add(GameManager.Instance.GameBoard.Get(tile.x, tile.y - 1));
        neighbors.Add(GameManager.Instance.GameBoard.Get(tile.x, tile.y + 1));
        neighbors.RemoveAll(x => x == null);
        return neighbors;
    }

    public List<GameTile> GetEmptyNeighbors(GameTile tile)
    {
        List<GameTile> neighbors = GetNeighbors(tile);
        neighbors.RemoveAll(x => !x.IsWalkable);
        return neighbors;
    }

    public List<GameTile> GetNeighborsAndDiagonals(GameTile tile)
    {
        List<GameTile> neighbors = GetNeighbors(tile);
        neighbors.Add(GameManager.Instance.GameBoard.Get(tile.x - 1, tile.y - 1));
        neighbors.Add(GameManager.Instance.GameBoard.Get(tile.x + 1, tile.y - 1));
        neighbors.Add(GameManager.Instance.GameBoard.Get(tile.x - 1, tile.y + 1));
        neighbors.Add(GameManager.Instance.GameBoard.Get(tile.x + 1, tile.y + 1)) ;
        neighbors.RemoveAll(x => x == null);
        return neighbors;
    }


    public GameTile GetAdjacent(GameTile tile, ProjectileDirection direction)
    {
        switch (direction)
        {
            case ProjectileDirection.Up:
                return Up(tile);
            case ProjectileDirection.Down:
                return Down(tile);
            case ProjectileDirection.Left:
                return Left(tile);
            case ProjectileDirection.Right:
                return Right(tile);
            default:
                return null;
        }
    }

    public GameTile Up(GameTile tile)
    {
        return Get(tile.x, tile.y + 1);
    }
    public GameTile Down(GameTile tile)
    {
        return Get(tile.x, tile.y - 1);
    }
    public GameTile Left(GameTile tile)
    {
        return Get(tile.x - 1, tile.y);
    }

    public GameTile Right(GameTile tile)
    {
        return Get(tile.x + 1, tile.y);
    }

    public double GetDistance(GameTile tileA, GameTile tileB)
    {
        return Math.Sqrt(Math.Pow(tileA.x - tileB.x, 2) + Math.Pow(tileA.y - tileB.y, 2));
    }
}


