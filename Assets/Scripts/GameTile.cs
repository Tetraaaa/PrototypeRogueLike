using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile
{
    public int x;
    public int y;
    private int tilesetX;
    private int tilesetY;
    public Vector3 worldPos;
    public GameObject? entity = null;
    public bool hasCollision = false;

    public GameTile Parent = null;

    public GameTile(int x, int y, int tilesetX, int tilesetY, bool hasCollision = false)
    {
        this.x = x;
        this.y = y;
        this.tilesetX = tilesetX;
        this.tilesetY = tilesetY;
        this.hasCollision = hasCollision;
    }

    public bool IsWalkable()
    {
        return !hasCollision && entity == null;
    }



}
