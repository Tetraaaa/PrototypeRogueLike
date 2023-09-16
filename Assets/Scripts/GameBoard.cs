using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GameBoard
{
    public List<GameTile> board;
    public GameBoard(Tilemap tilemap)
    {
        board = new List<GameTile>();
        tilemap.CompressBounds(); 
        //On place l'origine en haut à gauche (au lieu d'en bas à gauche) pour faciliter
        tilemap.origin = new Vector3Int(-32, 31, 0);

        //En x, on va vers la droite donc de -32 à 31
        for (int x = -32, i = 0; x < 32; x++, i++)
        {
            //En y, on va en bas donc de 31 à -32
            for (int y = 31, j = 0; y >= -32; y--, j--)
            {
                board.Add(new GameTile(i, j, x, y, tilemap.HasTile(new Vector3Int(x, y, 0))));
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
        return board.Find(t => t.x == x && t.y == y);
    }

    public GameTile Get(Vector3 pos)
    {
        return board.Find(t => t.x == (int)pos.x && t.y == (int)pos.y);
    }

    public void MoveEntity(GameTile currentTile, GameTile newTile)
    {
        if (!currentTile.entity) return;
        newTile.entity = currentTile.entity;
        currentTile.entity = null;
    }

    public GameTile GetRandomEmptyCell()
    {
        GameTile tile;
        do
        {
            tile = board[Random.Range(0, board.Count)];
        } while (tile.hasCollision || tile.entity != null);

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


}
